using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// Component used for writing 2D Text on the screen
    /// </summary>
    public class TextComponent : IComponent
    {
        #region Public Configuration
        /// <summary>
        /// Text that will be drawn
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Position of the text using X and Y cordinates of the screen
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// Color of the text
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// The text's font eg. Arial etc
        /// </summary>
        public SpriteFont Font { get; set; }
        /// <summary>
        /// An bool to check if text should be drawn
        /// </summary>
        public bool IsVisible { get; set; }
        #endregion Public Configuration
        /// <summary>
        /// Constructor which takes in all the nessecary parameters
        /// and the sets them.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="font"></param>
        /// <param name="isVisible"></param>
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
