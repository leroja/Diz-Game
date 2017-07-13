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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AIComp"></param>
        /// <param name="gameTime"></param>
        public override void Update(AIComponent AIComp, GameTime gameTime)
        {
            CurrentTimeForRotation += (float)gameTime.ElapsedGameTime.TotalSeconds;

            var transformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);
            var animComp = ComponentManager.Instance.GetEntityComponent<AnimationComponent>(AIComp.ID);
            var physComp = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(AIComp.ID);

            physComp.Velocity = transformComp.Forward * 10;
            physComp.Acceleration = new Vector3(physComp.Acceleration.X, 0, physComp.Acceleration.Z);

            var height = MovingSystem.GetHeight(transformComp.Position);

            transformComp.Position = new Vector3(transformComp.Position.X, height, transformComp.Position.Z);

            if (transformComp.Position.X >= AIComp.Bounds.Height)
            {
                Border(AIComp, transformComp, gameTime);
            }
            else if (transformComp.Position.X <= 3)
            {
                Border(AIComp, transformComp, gameTime);
            }
            else if (transformComp.Position.Z <= -AIComp.Bounds.Width)
            {
                Border(AIComp, transformComp, gameTime);
            }
            else if (transformComp.Position.Z >= -3)
            {
                Border(AIComp, transformComp, gameTime);
            }
            else
            {
                if (CurrentTimeForRotation > AIComp.UpdateFrequency)
                {
                    DesiredRotation = GetRotationTo(AIComp, ComponentManager.Instance.GetEntityComponent<TransformComponent>(ClosestEnemy).Position).Y;
                    CurrentTimeForRotation = 0f;
                }
                transformComp.Rotation = new Vector3(0, TurnToFace(DesiredRotation, transformComp.Rotation.Y, AIComp.TurningSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds), 0);
                BehaviorStuff(AIComp, transformComp);
            }
            animComp.CurrentTimeValue += TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds);
        }

        /// <summary>
        /// Check whether the AI should change behavior
        /// If it should then the method changes the behavior
        /// </summary>
        /// <param name="AIComp"> The AI component of the AI </param>
        /// <param name="transComp"> The transform component of the AI </param>
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
                if (AIComp.HaveBehavior("Patrol"))
                {
                    AIComp.ChangeBehavior("Patrol", transComp.Rotation);
                }
                else
                {
                    AIComp.ChangeBehavior("Wander", transComp.Rotation);
                }
            }
        }

        /// <summary>
        /// Code that executes when the AI collides with its defined border
        /// </summary>
        /// <param name="AIComp"> The AI component of the AI </param>
        /// <param name="transformComp"> The AI:s transform component </param>
        /// <param name="gameTime"> the current GameTime </param>
        private void Border(AIComponent AIComp, TransformComponent transformComp, GameTime gameTime)
        {
            transformComp.Position -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
            if (AIComp.HaveBehavior("Patrol"))
            {
                AIComp.ChangeBehavior("Patrol", transformComp.Rotation);
            }
            else
            {
                AIComp.ChangeBehavior("Wander", transformComp.Rotation);
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