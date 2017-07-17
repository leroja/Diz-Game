using GameEngine.Source.Systems;
using System.Linq;
using Microsoft.Xna.Framework;
using DizGame.Source.Components;
using GameEngine.Source.Components;
using System.Threading.Tasks;

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

            Parallel.ForEach(AIIds, EntId =>
            {
                var AIComponent = ComponentManager.GetEntityComponent<AIComponent>(EntId);
                AIComponent.CurrentBehaivior.FindClosestEnemy(AIComponent);
                AIComponent.CurrentBehaivior.FindClosestResource(AIComponent);

                AIComponent.CurrentBehaivior.Update(AIComponent, gameTime);
            });
        }
    }
}