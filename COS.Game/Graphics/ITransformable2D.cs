using System.Numerics;

namespace COS.Game.Graphics
{
    public interface ITransformable2D<TScale>
    {
        Vector2 Position { get; set; }
        TScale Scale { get; set; }
    }
}