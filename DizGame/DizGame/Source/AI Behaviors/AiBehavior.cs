using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.AI_States
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AiBehavior
    {
        /// <summary>
        /// The ID of the closest enemy
        /// </summary>
        public int ClosestEnemy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float DistanceToClosestEnemy { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="AIComp"></param>
        /// <param name="gameTime"></param>
        public abstract void Update(AIComponent AIComp, GameTime gameTime);


        /// <summary>
        /// 
        /// </summary>
        public void FindClosestEnemy(AIComponent AIComp)
        {
            DistanceToClosestEnemy = float.MaxValue;

            var PlayerIds = ComponentManager.Instance.GetAllEntitiesWithComponentType<PlayerComponent>();
            var AiIds = ComponentManager.Instance.GetAllEntitiesWithComponentType<AIComponent>();

            var transformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);
            //float distance = float.MaxValue;

            // Find wich Enemy Entity is the closest one
            //int closestEntity = AIComp.ID;
            foreach (var entityId in PlayerIds)
            {
                //var playComp = ComponentManager.Instance.GetEntityComponent<PlayerComponent>(entityId);
                var transComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entityId);

                var dist = Vector3.Distance(transformComp.Position, transComp.Position);
                if (dist < DistanceToClosestEnemy)
                {
                    //closestEntity = entityId;
                    ClosestEnemy = entityId;
                    DistanceToClosestEnemy = dist;
                }
            }

            // todo ska vi kolla mot andra AI:s eller inte?
            foreach (var EntityId in AiIds)
            {
                var transComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(EntityId);

                var dist = Vector3.Distance(transformComp.Position, transComp.Position);
                if (dist < DistanceToClosestEnemy)
                {
                    //closestEntity = EntityId;
                    ClosestEnemy = EntityId;
                    DistanceToClosestEnemy = dist;
                }
            }
        }


        // todo, vad ska den returnera
        /// <summary>
        /// 
        /// </summary>
        public void GetRotation()
        {

        }

    }
}
