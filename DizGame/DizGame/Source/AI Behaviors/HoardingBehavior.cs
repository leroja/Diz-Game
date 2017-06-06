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
    /// <summary>
    /// An AI behavior for collecting resources
    /// </summary>
    public class HoardingBehavior : AiBehavior
    {
        private float currentTimeForRot;
        private float desiredRotation;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotation"></param>
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

         
            if (currentTimeForRot > AIComp.UpdateFrequency)
            {
                desiredRotation = GetRotationTo(AIComp, ComponentManager.Instance.GetEntityComponent<TransformComponent>(ClosestResource).Position).Y;
                currentTimeForRot = 0f;
            }
            transformComp.Rotation = new Vector3(0, TurnToFace(desiredRotation, transformComp.Rotation.Y, AIComp.TurningSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds), 0);
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

            
        }

    }
}
