using System;
using System.Numerics;
using COS.Game.Graphics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Sprite = COS.Game.Graphics.Sprite;
using Text = COS.Game.Graphics.Text;

namespace COS.Game
{
    // TODO: Determine if we really _need_ to prevent leaking SFML types?

    public class GameWindow : IDisposable
    {
        private readonly RenderWindow _window;

        public GameWindow(WindowOptions options)
        {
            _window = new RenderWindow(new VideoMode(options.Width, options.Height), options.Title, (Styles) options.Flags);
        }

        public void Dispose() => _window.Dispose();

        public void Close() => _window.Close();

        public void Clear() => _window.Clear();

        public void Clear(Color color) => _window.Clear(color);

        public void Display() => _window.Display();

        public void SetTitle(string title) => _window.SetTitle(title);

        public void SetIcon(uint width, uint height, byte[] pixels) => _window.SetIcon(width, height, pixels);

        public void SetVisible(bool visible) => _window.SetVisible(visible);

        public void SetMouseCursorVisible(bool show) => _window.SetMouseCursorVisible(show);

        public void SetMouseCursorGrabbed(bool grabbed) => _window.SetMouseCursorGrabbed(grabbed);

        public void SetVerticalSyncEnabled(bool enable) => _window.SetVerticalSyncEnabled(enable);

        public void SetKeyRepeatEnabled(bool enable) => _window.SetKeyRepeatEnabled(enable);

        public bool SetActive() => _window.SetActive();

        public bool SetActive(bool active) => _window.SetActive(active);

        internal void SetFramerateLimit(uint limit) => _window.SetFramerateLimit(limit);

        internal event EventHandler<KeyEventArgs> KeyPressed
        {
            add => _window.KeyPressed += value;
            remove => _window.KeyPressed -= value;
        }

        internal event EventHandler<KeyEventArgs> KeyReleased
        {
            add => _window.KeyReleased += value;
            remove => _window.KeyReleased -= value;
        }

        public void Draw(Text text)
        {
            _window.Draw(text.SfmlText);
        }

        public void Draw(Sprite sprite)
        {
            _window.Draw(sprite.SfmlSprite);
        }

        public void Draw(Rectangle2D rect)
        {
            _window.Draw(new RectangleShape(new Vector2f(rect.Width, rect.Height))
            {
                FillColor = Color.Green,
                OutlineColor = Color.Cyan,
                OutlineThickness = 2,
                Position = new Vector2f(rect.X, rect.Y),
                Origin = new Vector2f(0, 0),
            });
        }

        public bool IsOpen => _window.IsOpen;

        // TODO: Perhaps create (u)int versions?
        public Vector2 Position
        {
            get => new Vector2(_window.Position.X, _window.Position.Y);
            set => _window.Position = new SFML.System.Vector2i((int) value.X, (int) value.Y);
        }

        public Vector2 Size
        {
            get => new Vector2(_window.Size.X, _window.Size.Y);
            set => _window.Size = new SFML.System.Vector2u((uint) value.X, (uint) value.Y);
        }

        public event EventHandler Closed
        {
            add => _window.Closed += value;
            remove => _window.Closed -= value;
        }

        // TODO: Wrap the SizeEventArgs type
        public event EventHandler<SizeEventArgs> Resized
        {
            add => _window.Resized += value;
            remove => _window.Resized -= value;
        }

        public event EventHandler LostFocus
        {
            add => _window.LostFocus += value;
            remove => _window.LostFocus -= value;
        }

        public event EventHandler GainedFocus
        {
            add => _window.GainedFocus += value;
            remove => _window.GainedFocus -= value;
        }

        public void WaitAndDispatchEvents()
        {
            _window.WaitAndDispatchEvents();
        }

        public void DispatchEvents()
        {
            _window.DispatchEvents();
        }
    }

    public struct WindowOptions
    {
        public WindowOptions(string title, uint width, uint height, WindowFlags flags = WindowFlags.Default)
        {
            Title = title;
            Width = width;
            Height = height;
            Flags = flags;
        }

        public string Title { get; }
        public uint Width { get; }
        public uint Height { get; }

        public WindowFlags Flags { get; }
    }

    // Wraps SFML.Window.Styles
    [Flags]
    public enum WindowFlags
    {
        /// <summary>No border / title bar (this flag and all others are mutually exclusive)</summary>
        None = 0,

        /// <summary>Title bar + fixed border</summary>
        Titlebar = 1,

        /// <summary>Titlebar + resizable border + maximize button</summary>
        Resize = 2,

        /// <summary>Titlebar + close button</summary>
        Close = 4,

        /// <summary>Fullscreen mode (this flag and all others are mutually exclusive))</summary>
        Fullscreen = 8,

        /// <summary>Default window style (titlebar + resize + close)</summary>
        Default = Close | Resize | Titlebar,
    }
}