using System;
using System.Linq;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Managers;
using GameEngine.Source.Components;
using GameEngine.Source.Utils;
using DizGame.Source.Systems;

namespace DizGame.Source.AI_Behaviors
{
    /// <summary>
    /// A behavior for the AI that makes it wander around the map
    /// </summary>
    public class WanderBehavior : AiBehavior
    {
        private float currentTimeForDir;
        private float desiredRotation;

        /// <summary>
        /// Constructor
        /// </summary>
        public WanderBehavior()
        {
            currentTimeForDir = 0f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotation"> The current rotation of the AI </param>
        public override void OnEnter(Vector3 rotation)
        {
            currentTimeForDir = 0f;
            desiredRotation = rotation.Y;
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

            currentTimeForDir += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentTimeForDir > AIComp.DirectionDuration)
            {
                desiredRotation = (float)Util.GetRandomNumber(-AIComp.DirectionChangeRoation, AIComp.DirectionChangeRoation);
                desiredRotation = WrapAngle(desiredRotation);
                currentTimeForDir = 0f;
            }
            transformComp.Rotation = new Vector3(0, TurnToFace(desiredRotation, transformComp.Rotation.Y, AIComp.TurningSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds), 0);


            physComp.Velocity = transformComp.Forward * 10;
            physComp.Acceleration = new Vector3(physComp.Acceleration.X, 0, physComp.Acceleration.Z);

            var height = MovingSystem.GetHeight(transformComp.Position);

            transformComp.Position = new Vector3(transformComp.Position.X, height, transformComp.Position.Z);


            if (transformComp.Position.X >= AIComp.Bounds.Height)
            {
                transformComp.Position -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
            }
            else if (transformComp.Position.X <= 3)
            {
                transformComp.Position -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
            }
            else if (transformComp.Position.Z <= -AIComp.Bounds.Width)
            {
                transformComp.Position -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
            }
            else if (transformComp.Position.Z >= -3)
            {
                transformComp.Position -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
            }

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
            var worldTemp = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<WorldComponent>();
            var worldComp = (WorldComponent)worldTemp.Values.First();

            if (AIComp.EvadeDistance - AIComp.Hysteria > AIComp.CurrentBehaivior.DistanceToClosestEnemy && !((worldComp.Day % worldComp.ModulusValue == 0 && worldComp.Day != 0)))
            {
                AIComp.ChangeBehavior("Evade", transcomp.Rotation);
            }
            else if (worldComp.Day % worldComp.ModulusValue == 0 && worldComp.Day != 0 && DistanceToClosestEnemy < AIComp.ChaseDistance - AIComp.Hysteria)
            {
                AIComp.ChangeBehavior("Chase", transcomp.Rotation);
            }
        }

        /// <summary>
        /// Override of object.ToString
        /// Returns the name of the behavior
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Wander";
        }
    }
}
