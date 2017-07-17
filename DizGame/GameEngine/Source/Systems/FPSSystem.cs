using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Source.Components;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// A basic system that calculates the current FPS of the game
    /// </summary>
    public class FPSSystem : IUpdate
    {
        private int framecount;
        private TimeSpan timeSinceLastUpdate;
        private int frameCounter;

        private int id;

        /// <summary>
        /// Updates the FPS
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            TimeSpan elapsed = gameTime.ElapsedGameTime;

            framecount++;
            timeSinceLastUpdate += elapsed;
            if (timeSinceLastUpdate > TimeSpan.FromSeconds(1))
            {
                frameCounter = framecount;

                var t = ComponentManager.GetEntityComponent<TextComponent>(id);
                t.Text = "FPS: " + frameCounter;

                framecount = 0;
                timeSinceLastUpdate -= TimeSpan.FromSeconds(1); ;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="font"> Font of the FPS text </param>
        /// <param name="EntityId"> A new entityID that the FPS text shall use </param>
        public FPSSystem(SpriteFont font, int EntityId)
        {
            framecount = 0;
            timeSinceLastUpdate = TimeSpan.Zero;
            frameCounter = 0;

            id = EntityId;

            var t = new TextComponent("Fps", Vector2.Zero, Color.Green, font, true);
            ComponentManager.AddComponentToEntity(id, t);
        }
    }
}