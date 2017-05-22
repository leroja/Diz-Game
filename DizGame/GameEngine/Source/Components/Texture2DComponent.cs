using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// Component used to draw 2D textures.
    /// </summary>
    public class Texture2DComponent : IComponent
    {
        /// <summary>
        /// Active texture.
        /// </summary>
        public Texture2D Texture { get; set; }
        /// <summary>
        /// Height of the texture.
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Width of the texture.
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Color
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// If wanting to scale the texture.
        /// </summary>
        public Vector2 Scale { get; set; }
        /// <summary>
        /// The textures rectangle.
        /// </summary>
        public Rectangle SourceRect { get; set; }
        /// <summary>
        /// If the texture should be drawn.
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// Amount of frames if animation is wanted.
        /// </summary>
        public int Frames { get; set; }
        /// <summary>
        /// Speed of the animation.
        /// </summary>
        public int FrameTime { get; set; }
        /// <summary>
        /// If the animation should loop.
        /// </summary>
        public bool Looping { get; set; }
        /// <summary>
        /// Set which frame is active.
        /// </summary>
        public int CurrentFrame { get; set; }
        /// <summary>
        /// Set the aminmations elapsedtime.
        /// </summary>
        public int ElapsedTime { get; set; }
        /// <summary>
        /// Position of the texture.
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// Rotation of the texture.
        /// </summary>
        public float Rotation { get; set; }
        /// <summary>
        /// Tectures rotation origin.
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// Construtor that takes in all parameters
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="color"></param>
        /// <param name="scale"></param>
        /// <param name="active"></param>
        /// <param name="frames"></param>
        /// <param name="frameTime"></param>
        /// <param name="looping"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        public Texture2DComponent(Texture2D texture, Color color, Vector2 scale, bool active, int frames, int frameTime, bool looping, Vector2 position, float rotation, Vector2 origin)
        {
            Texture = texture;
            Height = texture.Height;
            Width = texture.Width;
            Color = color;
            Scale = scale;
            Active = active;
            Frames = frames;
            FrameTime = frameTime;
            Looping = looping;
            Position = position;
            Rotation = rotation;
            Origin = origin;

            CurrentFrame = 0;
            ElapsedTime = 0;
        }

        /// <summary>
        /// Constructor which takes only Texture2D as parameter
        /// and set the rest to default values.
        /// </summary>
        /// <param name="texture"></param>
        public Texture2DComponent(Texture2D texture)
        {
            Texture = texture;
            Height = texture.Height;
            Width = texture.Width;
            Color = Color.LimeGreen;
            Scale = Vector2.One;
            Active = true;
            Frames = 1;
            FrameTime = 1;
            Looping = false;
            Position = new Vector2(0, 0);
            Rotation = 0;
            Origin = new Vector2(0, Height);

            CurrentFrame = 0;
            ElapsedTime = 0;
        }
    }
}
