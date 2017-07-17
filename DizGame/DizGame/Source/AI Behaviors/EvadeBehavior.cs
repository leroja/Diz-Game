using System;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Managers;
using GameEngine.Source.Components;
using DizGame.Source.Systems;

namespace DizGame.Source.AI_Behaviors
{
    /// <summary>
    /// A behavior that makes the AI evade all enemies that are within the specified distance
    /// </summary>
    public class EvadeBehavior : AiBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AIComp"></param>
        /// <param name="gameTime"></param>
        public override void Update(AIComponent AIComp, GameTime gameTime)
        {
            var transformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);
            var animComp = ComponentManager.Instance.GetEntityComponent<AnimationComponent>(AIComp.ID);
            var physComp = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(AIComp.ID);
            var pos = transformComp.Position;

            DesiredRotation = GetRotationTo(AIComp, ComponentManager.Instance.GetEntityComponent<TransformComponent>(ClosestEnemy).Position).Y - MathHelper.Pi;// -MathHelper.Pi gör så att AI:n får en motsatt rotaion till närmsta fienden;

            transformComp.Rotation = new Vector3(0, TurnToFace(DesiredRotation, transformComp.Rotation.Y, AIComp.TurningSpeed * 5 * (float)gameTime.ElapsedGameTime.TotalSeconds), 0);


            physComp.Velocity = transformComp.Forward * 30;
            physComp.Acceleration = new Vector3(physComp.Acceleration.X, 0, physComp.Acceleration.Z);

            var height = MovingSystem.GetHeight(transformComp.Position);

            transformComp.Position = new Vector3(transformComp.Position.X, height, transformComp.Position.Z);


            if (transformComp.Position.X >= AIComp.Bounds.HighX)
            {
                Border(transformComp, AIComp, gameTime);
            }
            else if (transformComp.Position.X <= AIComp.Bounds.LowX)
            {
                Border(transformComp, AIComp, gameTime);
            }
            else if (transformComp.Position.Z <= AIComp.Bounds.HighZ)
            {
                Border(transformComp, AIComp, gameTime);
            }
            else if (transformComp.Position.Z >= AIComp.Bounds.LowZ)
            {
                Border(transformComp, AIComp, gameTime);
            }

            animComp.CurrentTimeValue += TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds * 3);

            BehaviorStuff(AIComp, transformComp);
        }

        /// <summary>
        /// Code that executes when the AI collides with its defined border
        /// </summary>
        /// <param name="transformComp"> The AI:s transform component </param>
        /// <param name="AIComp"> The AI component of the AI </param>
        /// <param name="gameTime"> the current GameTime </param>
        private void Border(TransformComponent transformComp, AIComponent AIComp, GameTime gameTime)
        {
            transformComp.Position -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
            AIComp.ChangeBehavior("Wander", transformComp.Rotation);
        }

        /// <summary>
        /// Check whether the AI should change behavior
        /// If it should then the method changes the behavior
        /// </summary>
        /// <param name="AIComp"> The AI component of the AI </param>
        /// <param name="transcomp"> The transform component of the AI </param>
        private void BehaviorStuff(AIComponent AIComp, TransformComponent transcomp)
        {
            if (AIComp.EvadeDistance + AIComp.Hysteria < AIComp.CurrentBehaivior.DistanceToClosestEnemy)
            {
                AIComp.ChangeBehavior("Wander", transcomp.Rotation);
            }
        }

        /// <summary>
        /// Override of object.ToString
        /// Returns the name of the behavior
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Evade";
        }
    }
}