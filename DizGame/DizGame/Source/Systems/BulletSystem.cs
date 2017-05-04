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
    public class BulletSystem : IUpdate
    {
        /// <summary>
        /// A list containing the ids of the bullets that shall be removed because they have exceeded their max range
        /// </summary>
        private List<int> toDelete = new List<int>();

        /// <summary>
        /// Currently this system only checks if any of the bullets have exceeded their max range
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var compIds = ComponentManager.GetAllEntitiesWithComponentType<BulletComponent>();
            
            foreach (var id in compIds)
            {
                var bulletComponent = ComponentManager.GetEntityComponent<BulletComponent>(id);
                var mouseComp = ComponentManager.GetEntityComponent<MouseComponent>(id);
                var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(id);

                var curPos = transformComp.Position;

                var curRange = Vector3.Distance(transformComp.Position, bulletComponent.StartPos);
                if (curRange > bulletComponent.MaxRange)
                {
                    toDelete.Add(id);
                }

                // Todo, temp
                //transformComp.Position += transformComp.Forward *(float)10 *(float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            foreach (var id in toDelete)
            {
                ComponentManager.RemoveEntity(id);
                ComponentManager.RecycleID(id);
            }
            toDelete.Clear();
        }
    }
}
