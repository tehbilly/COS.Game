using System.Numerics;
using COS.Game.Graphics;
using SFML.Window; // TODO: Either figure out a clean way to wrap keyboard keys, or give up on wrapping

namespace COS.Game.Windows
{
    public static class Game
    {
        public static void Main(string[] args)
        {
            var engineBuilder = new EngineBuilder()
                // I don't know if using a property is nicer than a getter method, but here it is.
                .WindowOptions
                    .Name("Game Window")
                    .Width(1024)
                    .Height(768)
                    .Flags(WindowFlags.Titlebar | WindowFlags.Resize | WindowFlags.Close)
                    // Calling the End{Section} method lets you keep using the builder fluently
                    .EndWindowOptions();

            // Can use the builder without the End{Section} methods if you want
            engineBuilder.Systems
                // Just a hacky hack to display registering an instance of something to be injected
                    .Register(new Rectangle2D())
                    // Registering systems by type
                    .Register<MyInitSystem>()
                    .Register<MyUpdateSystem>()
                    .Register<MyDrawSystem>()
                    .Register<DebugTextHandler>();

            // Build the engine, finalizing the DI container and all that jazz. Everything else remains the same
            using var engine = engineBuilder.Build();
            engine.EntityManager.Add(new Hero());
            
            var text = engine.EntityManager.CreateEntity("Debug Info");
            text.AddComponent(new TextComponent(new Text("Elapsed Time: 0.00")));

            engine.Run();
        }
    }

    // EngineBuilder.Services.Register<Type>() registers a class and allows it to be resolvable by either it's type
    // directly _or_ by implemented interfaces. Drastically simplifies the code to manage/find all IInitSystems, etc.
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
            _engine.UpdatesPerSecond = 120;
        }
    }

    public class MyUpdateSystem : IUpdateSystem
    {
        private readonly EntityManager _entityManager;
        private readonly Input _input;
        private readonly GameWindow _window;
        private readonly Rectangle2D _rectangle;
        private const int Padding = 10;

        public MyUpdateSystem(EntityManager entityManager, Input input, GameWindow window, Rectangle2D rectangle)
        {
            _entityManager = entityManager;
            _input = input;
            _window = window;
            _rectangle = rectangle;
        }

        public void Update(float dt)
        {
            foreach (var entity in _entityManager.EntitiesWithComponent<HeroSpriteComponent>())
            {
                if (!(entity is Hero hero)) continue;
                var speed = hero.Speed;

                var sprite = entity.GetComponent<HeroSpriteComponent>().Sprite;

                var x = sprite.Position.X;
                var y = sprite.Position.Y;
                if (_input.IsKeyDown(Keyboard.Key.Right))
                    x += speed * dt;
                else if (_input.IsKeyDown(Keyboard.Key.Left))
                    x -= speed * dt;
                if (_input.IsKeyDown(Keyboard.Key.Down))
                    y += speed * dt;
                else if (_input.IsKeyDown(Keyboard.Key.Up))
                    y -= speed * dt;

                if (x < 0) x = 0;
                var maxX = _window.Size.X - sprite.Size.X;
                if (x > maxX) x = maxX;
                if (y < 0) y = 0;
                var maxY = _window.Size.Y - sprite.Size.Y;
                if (y > maxY) y = maxY;

                sprite.SetX(x);
                sprite.SetY(y);
            }

            var windowSize = _window.Size;
            var centerX = windowSize.X / 2;
            var centerY = windowSize.Y / 2;
            _rectangle.Position = new Vector2(centerX - _rectangle.Width / 2, centerY - _rectangle.Height / 2);
            _rectangle.Size = new Vector2(windowSize.X / 4 - Padding, windowSize.Y / 4 - Padding);
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
            _engine.Window.Draw(_rectangle);

            // Here's where I was thinking of ways that extending a base class rather than implementing an interface
            // might let me make things a little bit cleaner. Either way, this totally works!
            var entities = _engine.EntityManager.EntitiesWithComponent<HeroSpriteComponent>();
            foreach (var entity in entities)
            {
                _engine.Window.Draw(entity.GetComponent<HeroSpriteComponent>().Sprite);
            }
        }
    }

    // Here's a class extending the Entity type and overriding the Initialize() method to configure components.
    // I guess it's kind've like an entity factory in some systems I've seen.
    public class Hero : Entity
    {
        public int Speed { get; } = 150;
        public override void Initialize() =>
            AddComponent(new HeroSpriteComponent(new Sprite(Texture.FromFile("Assets/hero.png"))
            {
                Position = new Vector2(100, 100)
            }));
    }

    public class HeroSpriteComponent : Component
    {
        public Sprite Sprite { get; }

        public HeroSpriteComponent(Sprite sprite)
        {
            Sprite = sprite;
        }
    }

    public class TextComponent : Component
    {
        public Text Text { get; }
        
        public TextComponent(Text text)
        {
            Text = text;
        }
    }
    
    public class DebugTextHandler : IUpdateSystem, IDrawSystem
    {
        private readonly EntityManager _entityManager;
        private readonly GameWindow _window;
        private float _elapsed;

        public DebugTextHandler(EntityManager entityManager, GameWindow window)
        {
            _entityManager = entityManager;
            _window = window;
        }

        public void Update(float dt)
        {
            _elapsed += dt;
            foreach (var entity in _entityManager.EntitiesWithComponent<TextComponent>())
            {
                var text = entity.GetComponent<TextComponent>();
                text.Text.DisplayedString = $"Elapsed time: {_elapsed:0.000}s";
            }
        }

        public void Draw()
        {
            foreach (var entity in _entityManager.EntitiesWithComponent<TextComponent>())
                _window.Draw(entity.GetComponent<TextComponent>().Text);
        }
    }
}