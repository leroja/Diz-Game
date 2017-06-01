using Microsoft.Xna.Framework;
using System;
using System.Threading;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// An abstract class for rendering systems
    /// </summary>
    public abstract class IRender : ISystem
    {
        private static object _lock = new object();
        private Thread _thread;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// 
        public abstract void Draw(GameTime gameTime);
        /// <summary>
        /// Thread safe function 
        /// </summary>
        /// <param name="gameTime"></param>
        public void DrawSafe(GameTime gameTime)
        {
            lock (_lock)
            {
                if (_thread == null)
                {
                    _thread = new Thread(() => Draw(gameTime));
                    _thread.Start();
                }

            }
        }

    }
}
