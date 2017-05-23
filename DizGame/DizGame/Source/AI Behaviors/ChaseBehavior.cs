using System;
using System.Linq;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using DizGame.Source.Systems;

namespace DizGame.Source.AI_Behaviors
{
    /// <summary>
    /// A state that makes the AI Chase either another AI or the Player
    /// </summary>
    public class ChaseBehavior : AiBehavior
    {
        private float currentTimeForRot;
        private float desiredRotation;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotation"> The current rotation of the AI </param>
        public override void OnEnter(Vector3 rotation)
        {
            currentTimeForRot = 0;
            desiredRotation = rotation.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AIComp"></param>
        /// <param name="gameTime"></param>
        public override void Update(AIComponent AIComp, GameTime gameTime)
        {
            currentTimeForRot += (float)gameTime.ElapsedGameTime.TotalSeconds;

            var transformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);
            var animComp = ComponentManager.Instance.GetEntityComponent<AnimationComponent>(AIComp.ID);
            var physComp = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(AIComp.ID);
            
            physComp.Velocity = transformComp.Forward * 10;
            physComp.Acceleration = new Vector3(physComp.Acceleration.X, 0, physComp.Acceleration.Z);

            var height = MovingSystem.GetHeight(transformComp.Position);

            transformComp.Position = new Vector3(transformComp.Position.X, height, transformComp.Position.Z);

            if (transformComp.Position.X >= AIComp.Bounds.Height)
            {
                transformComp.Position -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
                if (AIComp.HaveBehavior("Patroll"))
                {
                    AIComp.ChangeBehavior("Patroll", transformComp.Rotation);
                }
                else
                {
                    AIComp.ChangeBehavior("Wander", transformComp.Rotation);
                }
            }
            else if (transformComp.Position.X <= 3)
            {
                transformComp.Position -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
                if (AIComp.HaveBehavior("Patroll"))
                {
                    AIComp.ChangeBehavior("Patroll", transformComp.Rotation);
                }
                else
                {
                    AIComp.ChangeBehavior("Wander", transformComp.Rotation);
                }
            }
            else if (transformComp.Position.Z <= -AIComp.Bounds.Width)
            {
                transformComp.Position -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
                if (AIComp.HaveBehavior("Patroll"))
                {
                    AIComp.ChangeBehavior("Patroll", transformComp.Rotation);
                }
                else
                {
                    AIComp.ChangeBehavior("Wander", transformComp.Rotation);
                }
            }
            else if (transformComp.Position.Z >= -3)
            {
                transformComp.Position -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
                if (AIComp.HaveBehavior("Patroll"))
                {
                    AIComp.ChangeBehavior("Patroll", transformComp.Rotation);
                }
                else
                {
                    AIComp.ChangeBehavior("Wander", transformComp.Rotation);
                }
            }
            else
            {
                if (currentTimeForRot > AIComp.UpdateFrequency)
                {
                    desiredRotation = GetRotationToClosestEnenmy(AIComp).Y;
                    currentTimeForRot = 0f;
                }
                transformComp.Rotation = new Vector3(0, TurnToFace(desiredRotation, transformComp.Rotation.Y, AIComp.TurningSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds), 0);
                BehaviorStuff(AIComp, transformComp);
            }
            animComp.CurrentTimeValue += TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds);
        }

        /// <summary>
        /// Check whether the AI chould change behavior
        /// If it should then the method changes the behavior
        /// </summary>
        /// <param name="AIComp"> The AI component of the AI </param>
        /// <param name="transComp"> The transorm component of the AI </param>
        private void BehaviorStuff(AIComponent AIComp, TransformComponent transComp)
        {
            var worldTemp = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<WorldComponent>();
            var worldComp = (WorldComponent)worldTemp.Values.First();

            if (AIComp.AttackingDistance > AIComp.CurrentBehaivior.DistanceToClosestEnemy)
            {
                AIComp.ChangeBehavior("Attacking", transComp.Rotation);
            }
            else if (DistanceToClosestEnemy > AIComp.ChaseDistance + AIComp.Hysteria)
            {
                if (AIComp.HaveBehavior("Patroll"))
                {
                    AIComp.ChangeBehavior("Patroll", transComp.Rotation);
                }
                else
                {
                    AIComp.ChangeBehavior("Wander", transComp.Rotation);
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
            return "Chase";
        }
    }
}
