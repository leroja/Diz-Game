using Microsoft.Xna.Framework;
using System;
using System.Threading;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// An abstract class for updating systems
    /// </summary>
    public abstract class IUpdate : ISystem
    {
        private static object _lock = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Thread safe update
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateThreadSafe(GameTime gameTime)
        {
            lock (_lock)
            {
                Update(gameTime);
            }
        }
    }
}