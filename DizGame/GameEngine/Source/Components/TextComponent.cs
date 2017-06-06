using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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
        /// Position of the text using X and Y coordinates of the screen
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// Color of the text
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// The text's font e.g. Arial etc
        /// </summary>
        public SpriteFont Font { get; set; }
        /// <summary>
        /// An bool to check if text should be drawn
        /// </summary>
        public bool IsVisible { get; set; }
        /// <summary>
        /// Dictionary used if more then one text is wanted for an entity. 
        /// </summary>
        public Dictionary<string, TextComponent> Children { get; set; }
        /// <summary>
        /// Set an background rectangle
        /// </summary>
        public Texture2D BackgroundRectangle { get; set; }
        /// <summary>
        /// Bool if rectangle is visible (background)
        /// </summary>
        public bool IsBackgroundVisible { get; set; }
        /// <summary>
        /// Set background color
        /// </summary>
        public Color BackgroundColor { get; set; }
        /// <summary>
        /// Sets the backgrounds opacity (see through)
        /// </summary>
        public float BackgroundOpacity { get; set; }
        #endregion Public Configuration

        /// <summary>
        /// Constructor which takes in all the necessary parameters
        /// and the sets them.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="font"></param>
        /// <param name="isVisible"></param>
        /// <param name="isBackgroundVisible"></param>
        /// <param name="backgroundOpacity"></param>
        /// <param name="backgroundColor"></param>
        public TextComponent(string text, Vector2 position, Color color, SpriteFont font, bool isVisible, Color? backgroundColor = null, bool isBackgroundVisible = false, float backgroundOpacity = 1f)
        {
            Text = text;
            Position = position;
            Color = color;
            Font = font;
            IsVisible = isVisible;

            BackgroundColor = backgroundColor ?? Color.Black;
            IsBackgroundVisible = isBackgroundVisible;
            BackgroundOpacity = backgroundOpacity;
        }

        /// <summary>
        /// Is used if multiple text is wanted
        /// </summary>
        /// <param name="key"></param>
        /// <param name="textComponents"></param>
        public TextComponent(List<string> key, List<TextComponent> textComponents)
        {
            Children = new Dictionary<string, TextComponent>();
            for (int i = 0; i < key.Count; i++)
                Children.Add(key[i], textComponents[i]);
        }
    }
}
