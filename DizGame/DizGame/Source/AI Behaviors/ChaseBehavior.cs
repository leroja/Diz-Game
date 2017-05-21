﻿using System;
using System.Linq;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;

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
            var pos = transformComp.Position;

            var height = GetCurrentHeight(pos);

            var t = new Vector3(transformComp.Position.X, height, transformComp.Position.Z);

            if (t.X >= AIComp.Bounds.Height)
            {
                t -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            else if (t.X <= 3)
            {
                t -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            else if (t.Z <= -AIComp.Bounds.Width)
            {
                t -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            else if (t.Z >= -3)
            {
                t -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
                t += transformComp.Forward * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentTimeForRot > AIComp.UpdateFrequency)
                {
                    desiredRotation = GetRotationToClosestEnenmy(AIComp).Y;
                    currentTimeForRot = 0f;
                }
                transformComp.Rotation = new Vector3(0, TurnToFace(desiredRotation, transformComp.Rotation.Y, AIComp.TurningSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds), 0);
                BehaviorStuff(AIComp, transformComp);
            }
            transformComp.Position = t;
            animComp.CurrentTimeValue += TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds);

            //transformComp.Position = new Vector3(transformComp.Position.X, height, transformComp.Position.Z) + transformComp.Forward * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //if (currentTimeForRot > AIComp.UpdateFrequency)
            //{
            //    desiredRotation = GetRotationToClosestEnenmy(AIComp).Y;
            //    currentTimeForRot = 0f;
            //}

            //transformComp.Rotation = new Vector3(0, TurnToFace(desiredRotation, transformComp.Rotation.Y, AIComp.TurningSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds), 0);
            //animComp.CurrentTimeValue += TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds);

            //BehaviorStuff(AIComp, transformComp);
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
