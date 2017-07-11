using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Managers;
using GameEngine.Source.Components;
using DizGame.Source.Systems;

namespace DizGame.Source.AI_Behaviors
{
    // TODO decide when to break from the hoarding behavior, I don't want them be in this behavior for to long but at the same time I want them to at least collect some ammo and/or health
    /// <summary>
    /// An AI behavior for collecting resources
    /// </summary>
    public class HoardingBehavior : AiBehavior
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

         
            if (CurrentTimeForRotation > AIComp.UpdateFrequency)
            {
                var healthComp = ComponentManager.Instance.GetEntityComponent<HealthComponent>(AIComp.ID);
                var ammoComp = ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(AIComp.ID);
                if (healthComp.Health == healthComp.MaxHealth)
                {
                    DesiredRotation = GetRotationTo(AIComp, ComponentManager.Instance.GetEntityComponent<TransformComponent>(ClosestAmmo).Position).Y;
                }
                else if(ammoComp.AmmountOfActiveMagazines >= 2 && healthComp.Health != healthComp.MaxHealth)
                {
                    DesiredRotation = GetRotationTo(AIComp, ComponentManager.Instance.GetEntityComponent<TransformComponent>(ClosestHealth).Position).Y;
                }
                else
                {
                    DesiredRotation = GetRotationTo(AIComp, ComponentManager.Instance.GetEntityComponent<TransformComponent>(ClosestResource).Position).Y;
                }
                CurrentTimeForRotation = 0f;
            }
            transformComp.Rotation = new Vector3(0, TurnToFace(DesiredRotation, transformComp.Rotation.Y, AIComp.TurningSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds), 0);
            BehaviorStuff(AIComp, transformComp);
            
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
            var healthComp = ComponentManager.Instance.GetEntityComponent<HealthComponent>(AIComp.ID);
            var ammoComp = ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(AIComp.ID);
            
            if (healthComp.Health == healthComp.MaxHealth && ammoComp.AmmountOfActiveMagazines >= 2)
            {
                AIComp.ChangeBehavior("Wander", transComp.Rotation);
            }
            else if (true)
            {
                AIComp.ChangeBehavior("Wander", transComp.Rotation);
            }else if (false)
            {
                AIComp.ChangeBehavior("Wander", transComp.Rotation);
            }
        }

    }
}