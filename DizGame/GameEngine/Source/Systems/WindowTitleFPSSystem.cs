using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// A basic system that writes the current FPS in the window title of the game
    /// </summary>
    public class WindowTitleFPSSystem : IUpdate
    {
        private Game g;
        private int framecount;
        private TimeSpan timeSinceLastUpdate;
        private int frameCounter;

        /// <summary>
        /// Updates the FPS in the window title
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            TimeSpan elapsed = gameTime.ElapsedGameTime;


            framecount++;
            timeSinceLastUpdate += elapsed;
            if (timeSinceLastUpdate > TimeSpan.FromSeconds(1))
            {

                //frameCounter = framecount / timeSinceLastUpdate;
                frameCounter = framecount;

                g.Window.Title = "FPS: " + frameCounter;

                framecount = 0;
                timeSinceLastUpdate -= TimeSpan.FromSeconds(1); ;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game"></param>
        public WindowTitleFPSSystem(Game game)
        {
            g = game;
            framecount = 0;
            timeSinceLastUpdate = TimeSpan.Zero;
            frameCounter = 0;
        }
    }
}
