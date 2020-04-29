using System;
using System.Numerics;
using COS.Game.Graphics;

namespace COS.Game.Windows
{
    public static class Game
    {
        public static void Main(string[] args)
        {
            using var engine = new Engine(new WindowOptions("GameWindow", 800, 600));
            engine.Systems.Add(new MyInitSystem(engine));

            // Since I don't have any interesting drawerings happening, make a jiggly rectangle instead
            var rectangle = new Rectangle2D();

            // Here's using the base Entity class as-is, and just adding components to define it
            var hero = new Entity();
            var sprite = new Sprite(Texture.FromFile("Assets/hero.png"))
            {
                Position = new Vector2(600, 400)
            };
            hero.AddComponent(new SpriteComponent(sprite));
            
            // Either way it's done, I'll probably add a delegate directly on Engine like AddEntity, instead of
            // directly accessing the EntityManager
            engine.EntityManager.Add(hero);
            engine.EntityManager.Add(new Hero());

            engine.Systems.Add(new MyUpdateSystem(engine, rectangle));
            engine.Systems.Add(new MyDrawSystem(engine, rectangle));


            engine.Run();
        }
    }

    public class MyInitSystem : IInitSystem
    {
        private readonly Engine _engine;

        public MyInitSystem(Engine engine)
        {
            _engine = engine;
        }

        public void Init()
        {
            var logger = _engine.Logger.ForContext(typeof(MyInitSystem));
            logger.Info("Setting UpdatesPerSecond to 10");
            _engine.UpdatesPerSecond = 10;
        }
    }

    public class MyUpdateSystem : IUpdateSystem
    {
        private readonly Random _random = new Random();
        private readonly Engine _engine;
        private readonly Rectangle2D _rectangle;

        public MyUpdateSystem(Engine engine, Rectangle2D rectangle)
        {
            _engine = engine;
            _rectangle = rectangle;
        }

        public void Update(float dt)
        {
            var window = _engine.Window;

            foreach (var entity in _engine.EntityManager.EntitiesWithComponent<SpriteComponent>())
            {
                var sprite = entity.GetComponent<SpriteComponent>().Sprite;

                // Adjust scale between 85% and 100% randomly
                var newScale = _random.NextDouble() * 0.15 + 0.85;
                sprite.Scale = new Vector2((float) newScale);

                SkitterX(sprite, _random.Next(0, 6));
                SkitterY(sprite, _random.Next(0, 6));
            }


            var padding = _random.Next(5, 10);
            var centerX = window.Size.X / 2;
            var centerY = window.Size.Y / 2;
            _rectangle.Position = new Vector2(centerX - _rectangle.Width / 2, centerY - _rectangle.Height / 2);
            _rectangle.Size = new Vector2(window.Size.X / 4 - padding, window.Size.Y / 4 - padding);
        }

        private void SkitterX(Sprite sprite, float padding)
        {
            var newX = sprite.Position.X + padding;
            if (_random.NextDouble() > 0.6) newX -= padding * 2;
            if (newX < 0) newX += padding;
            if (newX + sprite.Size.X > _engine.Window.Size.X)
                newX = _engine.Window.Size.X - sprite.Size.X;
            sprite.SetX(newX);
        }

        private void SkitterY(Sprite sprite, float padding)
        {
            var newY = sprite.Position.X + padding;
            if (_random.NextDouble() > 0.6) newY -= padding * 2;
            if (newY < 0) newY += padding;
            if (newY + sprite.Size.Y > _engine.Window.Size.Y)
                newY = _engine.Window.Size.Y - sprite.Size.Y;
            sprite.SetY(newY);
        }
    }

    public class MyDrawSystem : IDrawSystem
    {
        private readonly Engine _engine;
        private readonly Rectangle2D _rectangle;

        public MyDrawSystem(Engine engine, Rectangle2D rectangle)
        {
            _engine = engine;
            _rectangle = rectangle;
        }

        public void Draw()
        {
            // This DrawRectangle is just from me testing and will go away
            _engine.Window.DrawRectangle(_rectangle.Width, _rectangle.Height, _rectangle.X, _rectangle.Y);
            
            // Here's where I was thinking of ways that extending a base class rather than implementing an interface
            // might let me make things a little bit cleaner. Either way, this totally works!
            var entities = _engine.EntityManager.EntitiesWithComponent<SpriteComponent>();
            foreach (var entity in entities)
            {
                _engine.Window.Draw(entity.GetComponent<SpriteComponent>().Sprite);
            }
        }
    }

    // Here's a class extending the Entity type and overriding the Initialize() method to configure components.
    // I guess it's kind've like an entity factory in some systems I've seen.
    public class Hero : Entity
    {
        public override void Initialize()
        {
            AddComponent(new SpriteComponent(new Sprite(Texture.FromFile("Assets/hero.png"))));
        }
    }

    public class SpriteComponent : Component
    {
        public Sprite Sprite { get; }

        public SpriteComponent(Sprite sprite)
        {
            Sprite = sprite;
        }
    }
}