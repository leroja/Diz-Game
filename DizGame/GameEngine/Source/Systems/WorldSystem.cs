using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;

namespace GameEngine.Source.Systems
{
    public class WorldSystem : IUpdate
    {
        public override void Update(GameTime gameTime)
        {
            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>())
            {
                UpdateTime(entityID, gameTime);
            }
        }
        /// <summary>
        /// Update the World time using the gameTime
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="gameTime"></param>
        private void UpdateTime(int entityID, GameTime gameTime)
        {
            WorldComponent world = ComponentManager.GetEntityComponent<WorldComponent>(entityID);

            if (world.Noon && world.Hour == 12 && world.Second > 1)
                world.Noon = false;

            world.Hour += (float)gameTime.ElapsedGameTime.TotalSeconds / world.DefineHour;
            world.Minute += (float)gameTime.ElapsedGameTime.TotalSeconds * 60 / world.DefineHour;
            world.Second += (float)gameTime.ElapsedGameTime.TotalSeconds * 60 * 60 / world.DefineHour;
            world.Millisecond += (float)gameTime.ElapsedGameTime.TotalSeconds * 60 * 60 * 60 / world.DefineHour;

            if (world.Minute > 60)
                world.Minute = 0;
            if (world.Second > 60)
                world.Second = 0;
            if (world.Millisecond > 100)
                world.Millisecond = 0;

            if (world.Hour > world.Notation)
            {
                world.Hour = 0;
                if (world.Notation == WorldComponent.HOURS12 && !world.Noon)
                    world.Noon = true;
                else if (world.Notation == WorldComponent.HOURS12 && world.Noon)
                {
                    world.Noon = false;
                    world.Day++;
                }
                if (world.Notation == WorldComponent.HOURS24)
                    world.Day++;
            }
        }
    }
}
