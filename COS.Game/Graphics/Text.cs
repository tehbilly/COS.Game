using System.IO;
using System.Numerics;
using System.Reflection;
using SFML.Graphics;
using SFML.System;
using SfmlFont = SFML.Graphics.Font;
using SfmlText = SFML.Graphics.Text;

namespace COS.Game.Graphics
{
    public class Text : ITransformable2D<Vector2>
    {
        internal SfmlText SfmlText;

        public static Font DefaultFont { get; set; }
        
        private Font _font;
        public Font Font
        {
            get => _font;
            set { 
                _font = value;
                SfmlText.Font = value.SfmlFont;
            }
        }

        internal Text(SfmlText sfmlText)
        {
            LoadDefaultFont();
            SfmlText = sfmlText;
        }

        public Text(string text, Font font)
        {
            LoadDefaultFont();
            SfmlText = new SfmlText(text, font.SfmlFont);
        }

        public Text(string text)
        {
            LoadDefaultFont();
            SfmlText = new SfmlText(text, DefaultFont.SfmlFont);
        }

        private static void LoadDefaultFont()
        {
            if (DefaultFont != null) return;
            
            var stream = Assembly.GetAssembly(typeof(Text))
                .GetManifestResourceStream("COS.Game.Resources.PixelGameFont.ttf");
            DefaultFont = new Font(stream);
        }

        public string DisplayedString
        {
            get => SfmlText.DisplayedString;
            set => SfmlText.DisplayedString = value;
        }

        public Vector2 Position
        {
            get => SfmlText.Position.ToHidden();
            set => SfmlText.Position = new Vector2f(value.X, value.Y);
        }

        public Vector2 Scale
        {
            get => SfmlText.Scale.ToHidden();
            set => SfmlText.Scale = new Vector2f(value.X, value.Y);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            SfmlText.Draw(target, states);
        }
    }

    public class Font
    {
        internal readonly SfmlFont SfmlFont;

        internal Font(SfmlFont sfmlFont)
        {
            SfmlFont = sfmlFont;
        }

        public Font(string fileName)
        {
            SfmlFont = new SfmlFont(fileName);
        }

        public Font(Stream stream)
        {
            SfmlFont = new SfmlFont(stream);
        }
    }
}