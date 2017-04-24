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
        private float framecount;
        private float timeSinceLastUpdate;
        private float frameCounter;

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;


            framecount++;
            timeSinceLastUpdate += elapsed;
            if (timeSinceLastUpdate > 1)
            {
                frameCounter = framecount / timeSinceLastUpdate;

                g.Window.Title = "FPS: " + frameCounter;

                framecount = 0;
                timeSinceLastUpdate -= 1;
            }
        }

        public WindowTitleFPSSystem(Game game)
        {
            g = game;
            framecount = 0f;
            timeSinceLastUpdate = 0.0f;
            frameCounter = 0.0f;
        }
    }
}
