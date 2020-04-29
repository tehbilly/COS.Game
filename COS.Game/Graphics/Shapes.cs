using System.Numerics;

namespace COS.Game.Graphics
{
    public class Square2D : ITransformable2D<float, float>
    {
        public Vector2 Position { get; set; }
        public float Size { get; set; }
        public float Scale { get; set; }
        
        public float X => Position.X;
        public float Y => Position.Y;
        public float Width => Size;
        public float Height => Size;
    }

    public class Rectangle2D : ITransformable2D<Vector2, Vector2>
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Scale { get; set; }

        public float X => Position.X;
        public float Y => Position.Y;
        public float Width => Size.X;
        public float Height => Size.Y;

        public void SetScale(float x, float y)
        {
            Size = new Vector2(x, y);
        }

        public void SetScale(float scale)
        {
            Size = new Vector2(scale, scale);
        }
    }
}