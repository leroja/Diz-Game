using GameEngine.Source.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DizGame.Source.Components;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// 
    /// </summary>
    public class HealthSystem : IUpdate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var comps = ComponentManager.GetAllEntitiesWithComponentType<HealthComponent>();

            foreach (var id in comps)
            {
                var healthComp = ComponentManager.GetEntityComponent<HealthComponent>(id);
                
            }
        }
    }
}
