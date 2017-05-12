using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework.Input;

namespace DizGame.Source.AI_States
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
            var pos = transformComp.Position;
            
            var height = GetCurrentHeight(pos);
            transformComp.Position = new Vector3(transformComp.Position.X, height, transformComp.Position.Z) + transformComp.Forward * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentTimeForRot > AIComp.UpdateFrequency)
            {
                desiredRotation = GetRotationToClosestEnenmy(AIComp).Y;
                currentTimeForRot = 0f;
            }

            transformComp.Rotation = new Vector3(0, TurnToFace(desiredRotation, transformComp.Rotation.Y, AIComp.TurningSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds), 0);

            BehaviorStuff(AIComp, transformComp);
        }

        private void BehaviorStuff(AIComponent AIComp, TransformComponent transcomp)
        {
            var worldTemp = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<WorldComponent>();
            var worldComp = (WorldComponent)worldTemp.Values.First();

            if (AIComp.AttackingDistance + AIComp.Hysteria > AIComp.CurrentBehaivior.DistanceToClosestEnemy)
            {
                AIComp.ChangeBehavior("Attacking", transcomp.Rotation);
            }
        }

    }
}
