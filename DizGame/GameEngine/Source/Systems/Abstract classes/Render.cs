using Microsoft.Xna.Framework;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// An abstract class for rendering systems
    /// </summary>
    public abstract class IRender : ISystem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Draw(GameTime gameTime);

    }
}
