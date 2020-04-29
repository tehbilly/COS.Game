using System.Numerics;

namespace COS.Game
{
    public static class Extensions
    {
        // TODO: See if this is ok, or if we should hide things behind setters instead
        public static Vector2 ToHidden(this SFML.System.Vector2f v)
        {
            return new Vector2(v.X, v.Y);
        }
        
        public static Vector2u ToHidden(this SFML.System.Vector2u v)
        {
            return new Vector2u(v.X, v.Y);
        }
    }
}