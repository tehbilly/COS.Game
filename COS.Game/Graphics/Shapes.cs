using System.Numerics;

namespace COS.Game.Graphics
{
    public class Square2D : ITransformable2D<float>
    {
        public Vector2 Position { get; set; }
        public float Size { get; set; }
        public float Scale { get; set; }
        
        public float X
        {
            get => Position.X;
            set => Position = new Vector2(value, Position.Y);
        }

        public float Y
        {
            get => Position.Y;
            set => Position = new Vector2(Position.X, value);
        }

        public float Width
        {
            get => Size;
            set => Size = value;
        }

        public float Height
        {
            get => Size;
            set => Size = value;
        }
    }

    public class Rectangle2D : ITransformable2D<Vector2>
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Scale { get; set; }

        public float X
        {
            get => Position.X;
            set => Position = new Vector2(value, Position.Y);
        }

        public float Y
        {
            get => Position.Y;
            set => Position = new Vector2(Position.X, value);
        }

        public float Width
        {
            get => Size.X;
            set => Size = new Vector2(value, Size.Y);
        }

        public float Height
        {
            get => Size.Y;
            set => Size = new Vector2(Size.X, value);
        }

        public void SetScale(float scale)
        {
            Scale = new Vector2(scale, scale);
        }
    }
}