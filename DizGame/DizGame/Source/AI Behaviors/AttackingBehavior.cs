using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Managers;
using GameEngine.Source.Components;

namespace DizGame.Source.AI_Behaviors
{
    /// <summary>
    /// 
    /// </summary>
    public class AttackingBehavior : AiBehavior
    {
        private float time;
        private float coolDown;


        /// <summary>
        /// 
        /// </summary>
        public AttackingBehavior(float coolDown)
        {
            this.time = coolDown;
            this.coolDown = coolDown;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotation"> The current rotation of the AI </param>
        public override void OnEnter(Vector3 rotation)
        {
            this.time = coolDown;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AIComp"></param>
        /// <param name="gameTime"></param>
        public override void Update(AIComponent AIComp, GameTime gameTime)
        {
            var worldTemp = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<WorldComponent>();
            var worldComp = (WorldComponent)worldTemp.Values.First();
            var transformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);
            
            

            time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            transformComp.Rotation = GetRotationToClosestEnenmy(AIComp);
            
            if (worldComp.Day % 2 == 0 && worldComp.Day != 0 && time < 0)
            {
                var rot = GetUpwardsRotationToClosestEnenmy(AIComp);

                EntityFactory.Instance.CreateBullet("Bullet", transformComp.Position, new Vector3(.1f, .1f, .1f), transformComp.Forward, 100, 10, transformComp.Rotation + new Vector3(rot, 0, 0));
                time = AIComp.ShootingCooldown;
            }

            BehaviorStuff(AIComp, transformComp, worldComp);
            
        }

        private void BehaviorStuff(AIComponent AIComp, TransformComponent transformComp, WorldComponent worldComp)
        {
            // todo, kanske inte ska ha den här för att AIn blir enklare att undvika
            if (worldComp.Day % 2 == 0 && AIComp.AttackingDistance + AIComp.Hysteria < AIComp.CurrentBehaivior.DistanceToClosestEnemy)
            {

                AIComp.ChangeBehavior("Chase", transformComp.Rotation);
            }

            if (worldComp.Day % 2 != 0)
            {
                if (AIComp.HaveBehavior("Patroll"))
                {
                    AIComp.ChangeBehavior("Patroll", transformComp.Rotation);
                }
                else
                {
                    AIComp.ChangeBehavior("Evade", transformComp.Rotation);
                }
            }
        }

    }
}
