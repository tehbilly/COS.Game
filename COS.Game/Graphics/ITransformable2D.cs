using System.Numerics;

namespace COS.Game.Graphics
{
    public interface ITransformable2D<TSize, TScale>
    {
        Vector2 Position { get; set; }
        TSize Size { get; set; }
        TScale Scale { get; set; }
    }
}