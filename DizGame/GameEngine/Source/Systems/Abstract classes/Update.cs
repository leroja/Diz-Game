using Microsoft.Xna.Framework;
using System.Threading;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// An abstract class for updating systems
    /// </summary>
    public abstract class IUpdate : ISystem
    {
        private static object _lock = new object();
        private Thread _thread;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Thread safe update
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateSafe(GameTime gameTime)
        {
            lock (_lock)
            {
                if (_thread == null)
                {
                    _thread = new Thread(() => Update(gameTime));
                    _thread.Start();
                }
                
            }
        }
    }
}
