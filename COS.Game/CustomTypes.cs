namespace COS.Game
{
    public class Vector2u
    {
        private SFML.System.Vector2u _vector2U;

        public uint X
        {
            get => _vector2U.X;
            set => _vector2U.X = value;
        }
        public uint Y
        {
            get => _vector2U.Y;
            set => _vector2U.Y = value;
        }
        
        public Vector2u(uint x, uint y)
        {
            _vector2U = new SFML.System.Vector2u(x, y);
        }
        
        internal Vector2u(SFML.System.Vector2u vector2U)
        {
            _vector2U = vector2U;
        }

        public bool Equals(SFML.System.Vector2u other)
        {
            return _vector2U.Equals(other);
        }
    }
    
    public class Vector2i
    {
        private SFML.System.Vector2i _vector2I;

        public int X
        {
            get => _vector2I.X;
            set => _vector2I.X = value;
        }
        public int Y
        {
            get => _vector2I.Y;
            set => _vector2I.Y = value;
        }
        
        public Vector2i(int x, int y)
        {
            _vector2I = new SFML.System.Vector2i(x, y);
        }
        
        internal Vector2i(SFML.System.Vector2i vector2I)
        {
            _vector2I = vector2I;
        }

        public bool Equals(SFML.System.Vector2i other)
        {
            return _vector2I.Equals(other);
        }
    }
}