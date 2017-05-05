using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    public class TextComponent : IComponent
    {
        #region Public Configuration
        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public SpriteFont Font { get; set; }
        public bool IsVisible { get; set; }
        #endregion Public Configuration

        public TextComponent(string text, Vector2 position, Color color, SpriteFont font, bool isVisible)
        {
            Text = text;
            Position = position;
            Color = color;
            Font = font;
            IsVisible = isVisible;
        }
    }
}
