using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using System;

namespace DizGame.Source.AI_Behaviors
{
    // todo fixa så att AI:n letar efter ammo och hälsa om de har lågt värde
    // Todo gör att AI:n går ur attacking när den inte här några kulor kvar och/eller lite hälsa 

    /// <summary>
    /// The abstract base class for the AI behaviors
    /// </summary>
    public abstract class AiBehavior
    {
        /// <summary>
        /// The ID of the closest enemy
        /// </summary>
        public int ClosestEnemy { get; set; }
        /// <summary>
        /// Distance to the closest enemy
        /// </summary>
        public float DistanceToClosestEnemy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AIComp"></param>
        /// <param name="gameTime"></param>
        public abstract void Update(AIComponent AIComp, GameTime gameTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotation"> The current rotation of the AI </param>
        public abstract void OnEnter(Vector3 rotation);

        /// <summary>
        /// Finds the closest enemy entity
        /// </summary>
        public void FindClosestEnemy(AIComponent AIComp)
        {
            DistanceToClosestEnemy = float.MaxValue;

            var PlayerIds = ComponentManager.Instance.GetAllEntitiesWithComponentType<PlayerComponent>();
            var AiIds = ComponentManager.Instance.GetAllEntitiesWithComponentType<AIComponent>();

            var transformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);

            // Find wich Enemy Entity is the closest one
            foreach (var entityId in PlayerIds)
            {
                var transComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entityId);

                var dist = Vector3.Distance(transformComp.Position, transComp.Position);
                if (dist < DistanceToClosestEnemy)
                {
                    ClosestEnemy = entityId;
                    DistanceToClosestEnemy = dist;
                }
            }
            foreach (var EntityId in AiIds)
            {
                var transComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(EntityId);

                if (AIComp.ID == EntityId)
                    continue;

                var dist = Vector3.Distance(transformComp.Position, transComp.Position);
                if (dist < DistanceToClosestEnemy)
                {
                    ClosestEnemy = EntityId;
                    DistanceToClosestEnemy = dist;
                }
            }
        }

        /// <summary>
        /// Calculates the rotation to the closest enemy
        /// </summary>
        /// <param name="AIComp"> The current AI </param>
        /// <returns> A new rotation Vector </returns>
        public Vector3 GetRotationToClosestEnenmy(AIComponent AIComp)
        {
            var ClosestEnemyTransFormComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(ClosestEnemy);
            var AITransformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);

            float x = ClosestEnemyTransFormComp.Position.X - AITransformComp.Position.X;
            float z = ClosestEnemyTransFormComp.Position.Z - AITransformComp.Position.Z;
            float desiredAngle = (float)Math.Atan2(x, z) + MathHelper.Pi;

            return new Vector3(0, WrapAngle(desiredAngle), 0);
        }

        /// <summary>
        /// Calculates the angle that an object should face, given 
        /// its desiredAngle, its current angle, and its maximum turning speed.
        /// </summary>
        public float TurnToFace(float desiredAngle, float currentAngle, float turnSpeed)
        {
            // first, figure out how much we want to turn, using WrapAngle to get our
            // result from -Pi to Pi ( -180 degrees to 180 degrees )
            float difference = WrapAngle(desiredAngle - currentAngle);

            // clamp that between -turnSpeed and turnSpeed.
            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

            // so, the closest we can get to our target is currentAngle + difference.
            // return that, using WrapAngle again.
            return WrapAngle(currentAngle + difference);
        }

        /// <summary>
        /// A function for keeping the rotation to be between -2PI and +2PI
        /// </summary>
        /// <param name="rotation"> The current rotation </param>
        /// <returns> A rotation between -2PI and +2PI </returns>
        public float WrapAngle(float rotation)
        {
            while (rotation < -MathHelper.Pi)
            {
                rotation += MathHelper.TwoPi;
            }
            while (rotation > MathHelper.Pi)
            {
                rotation -= MathHelper.TwoPi;
            }
            return rotation;
        }

        /// <summary>
        /// Gets the rotation needed for shooting at an enemy on different height level
        /// </summary>
        /// <param name="AIComp"> The current AI </param>
        /// <returns></returns>
        public float GetRotationForAimingAtEnemy(AIComponent AIComp)
        {
            var ClosestEnemyTransFormComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(ClosestEnemy);
            var AITransformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);

            var dist = Vector2.Distance(new Vector2(AITransformComp.Position.X, AITransformComp.Position.Z), new Vector2(ClosestEnemyTransFormComp.Position.X, ClosestEnemyTransFormComp.Position.Z));

            float y = ClosestEnemyTransFormComp.Position.Y - AITransformComp.Position.Y;

            float desiredAngle = (float)Math.Atan2(y, dist);

            return WrapAngle(desiredAngle);
        }
    }
}
