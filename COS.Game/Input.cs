using System.Collections.Generic;
using SFML.Window;

namespace COS.Game
{
    public class Input : IPostUpdateSystem
    {
        private readonly HashSet<Keyboard.Key> _keysDown = new HashSet<Keyboard.Key>();
        private readonly HashSet<Keyboard.Key> _keysPressed = new HashSet<Keyboard.Key>();
        private readonly HashSet<Keyboard.Key> _keysReleased = new HashSet<Keyboard.Key>();

        public readonly bool HasKeyboard = true;

        public bool IsKeyPressed(Keyboard.Key key) => _keysPressed.Contains(key);
        public bool IsKeyDown(Keyboard.Key key) => _keysPressed.Contains(key) || _keysDown.Contains(key);
        public bool IsKeyReleased(Keyboard.Key key) => _keysReleased.Contains(key);

        internal void OnKeyPressed(object _, KeyEventArgs args)
        {
            var key = args.Code;
            if (_keysDown.Contains(key)) return;
            _keysPressed.Add(key);
        }

        internal void OnKeyReleased(object _, KeyEventArgs args)
        {
            var key = args.Code;
            _keysPressed.Remove(key);
            _keysDown.Remove(key);
            _keysReleased.Add(key);
        }

        public void PostUpdate()
        {
            // Move keys from pressed to down
            foreach (var key in _keysPressed)
            {
                _keysDown.Add(key);
            }
            _keysPressed.Clear();
            
            // Clear list of keys released since last update
            _keysReleased.Clear();
        }
    }
}