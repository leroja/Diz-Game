using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.RandomStuff
{
    /// <summary>
    /// The lensflare effect is made up from several individual flare graphics,
    /// which move across the screen depending on the position of the sun. This
    /// helper class keeps track of the position, size, and color for each flare.
    /// </summary>
    public class Flare
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        /// <param name="color"></param>
        /// <param name="textureName"></param>
        public Flare(float position, float scale, Color color, string textureName)
        {
            Position = position;
            Scale = scale;
            Color = color;
            TextureName = textureName;
        }
        /// <summary>
        /// Flare position
        /// </summary>
        public float Position { get; set; }
        /// <summary>
        /// Flare scale
        /// </summary>
        public float Scale { get; set; }
        /// <summary>
        /// Flare color
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// Texture name
        /// </summary>
        public string TextureName { get; set; }
        /// <summary>
        /// Flare texture ex PNG picture.
        /// </summary>
        public Texture2D Texture { get; set; }
    }
}
