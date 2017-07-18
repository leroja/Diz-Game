using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using System;

namespace DizGame.Source.AI_Behaviors
{
    // Todo fix so that the AI looks for ammo and health when they are low
    /// <summary>
    /// The abstract base class for the AI behaviors
    /// </summary>
    public abstract class AiBehavior
    {
        #region properties
        /// <summary>
        /// The ID of the closest enemy
        /// </summary>
        public int ClosestEnemy { get; set; }
        /// <summary>
        /// Distance to the closest enemy
        /// </summary>
        public float DistanceToClosestEnemy { get; set; }
        /// <summary>
        /// Distance to the closest resource
        /// </summary>
        public float DistanceToClosestResource { get; set; }
        /// <summary>
        /// The ID of the Closest resource
        /// </summary>
        public int ClosestResource { get; set; }
        /// <summary>
        /// The ID of the closest Ammo resource
        /// </summary>
        public int ClosestAmmo { get; set; }
        /// <summary>
        /// The ID of the closest Health Resource
        /// </summary>
        public int ClosestHealth { get; set; }
        /// <summary>
        /// Distance to the closest Ammo resource
        /// </summary>
        public float DistanceToClosestAmmo { get; set; }
        /// <summary>
        /// Distance to the closest Health resource
        /// </summary>
        public float DistanceToClosestHealth { get; set; }

        /// <summary>
        /// The rotation the AI wants to have
        /// </summary>
        public float DesiredRotation { get; set; }
        /// <summary>
        /// How long since the AI last updated its desired rotation
        /// </summary>
        public float CurrentTimeForRotation { get; set; }
        /// <summary>
        /// Time left until the AI can shot again
        /// </summary>
        public float Time { get; set; }

        #endregion properties

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AIComp"></param>
        /// <param name="gameTime"></param>
        public abstract void Update(AIComponent AIComp, GameTime gameTime);

        /// <summary>
        /// Finds the closest enemy entity
        /// </summary>
        public void FindClosestEnemy(AIComponent AIComp)
        {
            DistanceToClosestEnemy = float.MaxValue;

            var PlayerIds = ComponentManager.Instance.GetAllEntitiesWithComponentType<PlayerComponent>();
            var AiIds = ComponentManager.Instance.GetAllEntitiesWithComponentType<AIComponent>();

            var transformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);

            // Find which Enemy Entity is the closest one
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
        /// Finds the closest resource entity
        /// </summary>
        public void FindClosestResource(AIComponent AIComp)
        {
            DistanceToClosestResource = float.MaxValue;
            DistanceToClosestAmmo = float.MaxValue;
            DistanceToClosestHealth = float.MaxValue;

            var ResourceIds = ComponentManager.Instance.GetAllEntitiesWithComponentType<ResourceComponent>();


            var transformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);

            // Find which Resource Entity is the closest one
            foreach (var entityId in ResourceIds)
            {
                var resourceComp = ComponentManager.Instance.GetEntityComponent<ResourceComponent>(entityId);
                var transComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entityId);

                var dist = Vector3.Distance(transformComp.Position, transComp.Position);
                if (dist < DistanceToClosestResource)
                {
                    ClosestResource = entityId;
                    DistanceToClosestResource = dist;
                }
                if (dist < DistanceToClosestAmmo && resourceComp.thisType == ResourceType.Ammo)
                {
                    ClosestAmmo = entityId;
                    DistanceToClosestAmmo = dist;
                }
                if (dist < DistanceToClosestHealth && resourceComp.thisType == ResourceType.Health)
                {
                    ClosestHealth = entityId;
                    DistanceToClosestHealth = dist;
                }
            }
        }

        /// <summary>
        /// Calculates the rotation to the position
        /// </summary>
        /// <param name="AIComp"> The current AI </param>
        /// <param name="OtherPosition"> Position to rotate to </param>
        /// <returns> A new rotation Vector </returns>
        public Vector3 GetRotationTo(AIComponent AIComp, Vector3 OtherPosition)
        {
            var AITransformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);

            float x = OtherPosition.X - AITransformComp.Position.X;
            float z = OtherPosition.Z - AITransformComp.Position.Z;
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