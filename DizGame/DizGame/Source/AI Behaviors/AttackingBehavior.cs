using System.Linq;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Managers;
using GameEngine.Source.Components;

namespace DizGame.Source.AI_Behaviors
{
    // todo gör så att AI:n inte roterar direkt mot närmsta fienden utan gör så att den vänder sig mod den och gör så att den inte kan skjuta innan den här helt vänd mot fienden
    /// <summary>
    /// A Behavior that makes the AI shoot at the closest enemy
    /// </summary>
    public class AttackingBehavior : AiBehavior
    {
        private float time;
        private float coolDown;

        /// <summary>
        /// Constructor
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
            var physComp = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(AIComp.ID);
            //physComp.Velocity = new Vector3(0, physComp.Velocity.Y, 0); // todo temp
            physComp.Velocity = Vector3.Zero; // todo temp
            physComp.Acceleration = Vector3.Zero;  // todo temp

            time -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            transformComp.Rotation = GetRotationToClosestEnenmy(AIComp);
            transformComp.Position = new Vector3(transformComp.Position.X, GetHeight(transformComp.Position), transformComp.Position.Z);
            if (worldComp.Day % worldComp.ModulusValue == 0 && worldComp.Day != 0 && time < 0)
            {
                var rot = GetRotationForAimingAtEnemy(AIComp);

                EntityFactory.Instance.CreateBullet("Bullet", transformComp.Position + transformComp.Forward * 7, new Vector3(.1f, .1f, .1f), 100, 1000, transformComp.Rotation + new Vector3(rot, 0, 0), AIComp.DamagePerShot);
                time = AIComp.ShootingCooldown;
            }

            BehaviorStuff(AIComp, transformComp, worldComp);
        }

        /// <summary>
        /// Check whether the AI chould change behavior
        /// If it should then the method changes the behavior
        /// </summary>
        /// <param name="AIComp"> The AI component of the AI </param>
        /// <param name="transformComp"> The transorm component of the AI </param>
        /// <param name="worldComp"> The world component </param>
        private void BehaviorStuff(AIComponent AIComp, TransformComponent transformComp, WorldComponent worldComp)
        {
            if (worldComp.Day % worldComp.ModulusValue == 0 && AIComp.AttackingDistance + AIComp.Hysteria < AIComp.CurrentBehaivior.DistanceToClosestEnemy)
            {
                AIComp.ChangeBehavior("Chase", transformComp.Rotation);
            }

            if (worldComp.Day % worldComp.ModulusValue != 0)
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

        /// <summary>
        /// Override of object.ToString
        /// Returns the name of the behavior
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Attacking";
        }
    }
}
