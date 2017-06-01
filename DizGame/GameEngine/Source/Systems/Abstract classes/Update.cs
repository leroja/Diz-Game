using Microsoft.Xna.Framework;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// An abstract class for updating systems
    /// </summary>
    public abstract class IUpdate : ISystem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);
    }
}
