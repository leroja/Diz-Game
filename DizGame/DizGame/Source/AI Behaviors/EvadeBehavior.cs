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
        /// <param name="rotation"> The current rotation of the AI </param>
        public override void OnEnter(Vector3 rotation)
        {
        }

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

            var rotation = GetRotationTo(AIComp, ComponentManager.Instance.GetEntityComponent<TransformComponent>(ClosestEnemy).Position);

            transformComp.Rotation = rotation + new Vector3(0, -MathHelper.Pi, 0); // -MathHelper.Pi gör så att AI:n får en motsatt rotaion till närmsta fienden


            physComp.Velocity = transformComp.Forward * 10;
            physComp.Acceleration = new Vector3(physComp.Acceleration.X, 0, physComp.Acceleration.Z);

            var height = MovingSystem.GetHeight(transformComp.Position);

            transformComp.Position = new Vector3(transformComp.Position.X, height, transformComp.Position.Z);


            if (transformComp.Position.X >= AIComp.Bounds.Height)
            {
                transformComp.Position -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
                AIComp.ChangeBehavior("Wander", transformComp.Rotation);
            }
            else if (transformComp.Position.X <= 3)
            {
                transformComp.Position -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
                AIComp.ChangeBehavior("Wander", transformComp.Rotation);
            }
            else if (transformComp.Position.Z <= -AIComp.Bounds.Width)
            {
                transformComp.Position -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
                AIComp.ChangeBehavior("Wander", transformComp.Rotation);
            }
            else if (transformComp.Position.Z >= -3)
            {
                transformComp.Position -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
                AIComp.ChangeBehavior("Wander", transformComp.Rotation);
            }

            animComp.CurrentTimeValue += TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds);

            BehaviorStuff(AIComp, transformComp);
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
