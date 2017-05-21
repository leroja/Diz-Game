using System;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Managers;
using GameEngine.Source.Components;

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
            var pos = transformComp.Position;

            var height = GetCurrentHeight(pos);
            var t = new Vector3(transformComp.Position.X, height, transformComp.Position.Z);

            var rotation = GetRotationToClosestEnenmy(AIComp);

            transformComp.Rotation = rotation + new Vector3(0, -MathHelper.Pi, 0);

            if (t.X >= AIComp.Bounds.Height)
            {
                t -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
                AIComp.ChangeBehavior("Wander", transformComp.Rotation);
            }
            else if (t.X <= 3)
            {
                t -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
                AIComp.ChangeBehavior("Wander", transformComp.Rotation);
            }
            else if (t.Z <= -AIComp.Bounds.Width)
            {
                t -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
                AIComp.ChangeBehavior("Wander", transformComp.Rotation);
            }
            else if (t.Z >= -3)
            {
                t -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
                AIComp.ChangeBehavior("Wander", transformComp.Rotation);
            }
            else
            {
                t += transformComp.Forward * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            transformComp.Position = t;
            animComp.CurrentTimeValue += TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds);

            BehaviorStuff(AIComp, transformComp);
        }

        /// <summary>
        /// Check whether the AI chould change behavior
        /// If it should then the method changes the behavior
        /// </summary>
        /// <param name="AIComp"> The AI component of the AI </param>
        /// <param name="transcomp"> The transorm component of the AI </param>
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
