using System.Linq;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Managers;
using GameEngine.Source.Components;
using DizGame.Source.Systems;
using DizGame.Source.Factories;

namespace DizGame.Source.AI_Behaviors
{
    // TODO insert some spread on the shots, e.g. add a little rotation
    /// <summary>
    /// A Behavior that makes the AI shoot at the closest enemy
    /// </summary>
    public class AttackingBehavior : AiBehavior
    {
        private float time;
        private float coolDown;
        private float desiredRotation;
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
            desiredRotation = rotation.Y;
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
            physComp.Velocity = Vector3.Zero;
            physComp.Acceleration = Vector3.Zero;

            time -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            desiredRotation = GetRotationTo(AIComp, ComponentManager.Instance.GetEntityComponent<TransformComponent>(ClosestEnemy).Position).Y;
            transformComp.Rotation = new Vector3(0, TurnToFace(desiredRotation, transformComp.Rotation.Y, AIComp.TurningSpeed * 4 * (float)gameTime.ElapsedGameTime.TotalSeconds), 0);
            transformComp.Position = new Vector3(transformComp.Position.X, MovingSystem.GetHeight(transformComp.Position), transformComp.Position.Z);
            if (worldComp.Day % worldComp.ModulusValue == 0 && worldComp.Day != 0 && time < 0)
            {
                if (ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(AIComp.ID).CurrentAmmoInMag > 0 && desiredRotation == transformComp.Rotation.Y)
                {
                    var rot = GetRotationForAimingAtEnemy(AIComp);

                    EntityFactory.Instance.CreateBullet("Bullet", transformComp.Position + transformComp.Forward * 7, new Vector3(.1f, .1f, .1f), 100, 1000, transformComp.Rotation + new Vector3(rot, 0, 0), AIComp.DamagePerShot, AIComp.ID);
                    ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(AIComp.ID).CurrentAmmoInMag--;
                    time = AIComp.ShootingCooldown;
                }
            }

            BehaviorStuff(AIComp, transformComp, worldComp);
        }

        /// <summary>
        /// Check whether the AI should change behavior
        /// If it should then the method changes the behavior
        /// </summary>
        /// <param name="AIComp"> The AI component of the AI </param>
        /// <param name="transformComp"> The transform component of the AI </param>
        /// <param name="worldComp"> The world component </param>
        private void BehaviorStuff(AIComponent AIComp, TransformComponent transformComp, WorldComponent worldComp)
        {
            if (worldComp.Day % worldComp.ModulusValue == 0 && AIComp.AttackingDistance + AIComp.Hysteria < AIComp.CurrentBehaivior.DistanceToClosestEnemy)
            {
                AIComp.ChangeBehavior("Chase", transformComp.Rotation);
            }

            if (worldComp.Day % worldComp.ModulusValue != 0)
            {
                if (AIComp.HaveBehavior("Patrol"))
                {
                    AIComp.ChangeBehavior("Patrol", transformComp.Rotation);
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