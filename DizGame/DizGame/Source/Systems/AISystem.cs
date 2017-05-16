using GameEngine.Source.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DizGame.Source.Components;
using GameEngine.Source.Components;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// A system for updating all of the AIs
    /// </summary>
    public class AISystem : IUpdate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var AIIds = ComponentManager.GetAllEntitiesWithComponentType<AIComponent>();
            var worldTemp = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<WorldComponent>();
            var worldComp = (WorldComponent)worldTemp.Values.First();

            foreach (var EntId in AIIds)
            {
                var AIComponent = ComponentManager.GetEntityComponent<AIComponent>(EntId);
                AIComponent.CurrentBehaivior.FindClosestEnemy(AIComponent);
                
                AIComponent.CurrentBehaivior.Update(AIComponent, gameTime);
            }
        }
        
    }
}
