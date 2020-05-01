using System;
using System.Numerics;
using SFML.Graphics;
using SFML.System;
using SFMLSprite = SFML.Graphics.Sprite;

namespace COS.Game.Graphics
{
    public sealed class Sprite : ITransformable2D<Vector2>, IDisposable
    {
        internal readonly SFMLSprite SfmlSprite;

        public readonly Texture Texture;
        
        public Sprite(Texture texture)
        {
            Texture = texture;
            SfmlSprite = new SFMLSprite(texture.SfmlTexture);
        }
        
        public Sprite(Texture texture, Vector4 rect)
        {
            SfmlSprite = new SFMLSprite(texture.SfmlTexture, new IntRect((int) rect.X, (int) rect.Y, (int) rect.W, (int) rect.Z));
        }

        public Vector2 Size => new Vector2(Texture.Size.X, Texture.Size.Y);

        public Vector2 Position
        {
            get => SfmlSprite.Position.ToHidden();
            set => SfmlSprite.Position = new Vector2f(value.X, value.Y);
        }

        public float Rotation
        {
            get => SfmlSprite.Rotation;
            set => SfmlSprite.Rotation = value;
        }

        public Vector2 Scale
        {
            get => SfmlSprite.Scale.ToHidden();
            set => SfmlSprite.Scale = new Vector2f(value.X, value.Y);
        }

        public Vector2 Origin
        {
            get => SfmlSprite.Origin.ToHidden();
            set => SfmlSprite.Origin = new Vector2f(value.X, value.Y);
        }

        public void Dispose()
        {
            SfmlSprite?.Dispose();
        }

        public void SetX(float x)
        {
            Position = new Vector2(x, Position.Y);
        }

        public void SetY(float y)
        {
            Position = new Vector2(Position.X, y);
        }
    }
}