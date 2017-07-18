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
        /// <summary>
        /// Constructor
        /// </summary>
        public WanderBehavior()
        {
            CurrentTimeForRotation = 0f;
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

            CurrentTimeForRotation += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (CurrentTimeForRotation > AIComp.DirectionDuration)
            {
                DesiredRotation = (float)Util.GetRandomNumber(-AIComp.DirectionChangeRoation, AIComp.DirectionChangeRoation);
                DesiredRotation = WrapAngle(DesiredRotation);
                CurrentTimeForRotation = 0f;
            }
            transformComp.Rotation = new Vector3(0, TurnToFace(DesiredRotation, transformComp.Rotation.Y, AIComp.TurningSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds), 0);


            physComp.Velocity = transformComp.Forward * 10;
            physComp.Acceleration = new Vector3(physComp.Acceleration.X, 0, physComp.Acceleration.Z);

            var height = MovingSystem.GetHeight(transformComp.Position);

            transformComp.Position = new Vector3(transformComp.Position.X, height, transformComp.Position.Z);


            if (transformComp.Position.X >= AIComp.Bounds.HighX)
            {
                Border(transformComp, gameTime);
            }
            else if (transformComp.Position.X <= AIComp.Bounds.LowX)
            {
                Border(transformComp, gameTime);
            }
            else if (transformComp.Position.Z <= AIComp.Bounds.HighZ)
            {
                Border(transformComp, gameTime);
            }
            else if (transformComp.Position.Z >= AIComp.Bounds.LowZ)
            {
                Border(transformComp, gameTime);
            }

            animComp.CurrentTimeValue += TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds);

            BehaviorStuff(AIComp, transformComp);
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
            var ammoComp = ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(AIComp.ID);
            var healthComp = ComponentManager.Instance.GetEntityComponent<HealthComponent>(AIComp.ID);

            if (AIComp.EvadeDistance - AIComp.Hysteria > AIComp.CurrentBehaivior.DistanceToClosestEnemy && !((worldComp.Day % worldComp.ModulusValue == 0 && worldComp.Day != 0)))
            {
                AIComp.ChangeBehavior("Evade", transComp.Rotation);
            }
            else if (ammoComp.AmmountOfActiveMagazines < 2 || (healthComp.Health / healthComp.MaxHealth) <= 0.6)
            {
                AIComp.ChangeBehavior("Hoarding", transComp.Rotation);
            }
            else if (worldComp.Day % worldComp.ModulusValue == 0 && worldComp.Day != 0 && DistanceToClosestEnemy < AIComp.ChaseDistance - AIComp.Hysteria)
            {
                AIComp.ChangeBehavior("Chase", transComp.Rotation);
            }
        }

        /// <summary>
        /// Code that executes when the AI collides with its defined border
        /// </summary>
        /// <param name="transformComp"> The AI:s transform component </param>
        /// <param name="gameTime"> the current GameTime </param>
        private void Border(TransformComponent transformComp, GameTime gameTime)
        {
            transformComp.Position -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds; // move the AI away from the border
            transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0); // rotate the AI 180 degrees
            DesiredRotation += MathHelper.Pi;
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