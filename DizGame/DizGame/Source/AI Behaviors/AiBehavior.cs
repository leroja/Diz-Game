using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DizGame.Source.AI_Behaviors
{
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
        /// Returns the current height based on the AI current position
        /// </summary>
        /// <param name="position">
        /// The current Position of the AI
        /// </param>
        /// <returns></returns>
        public float GetCurrentHeight(Vector3 position)
        {
            List<int> temp = ComponentManager.Instance.GetAllEntitiesWithComponentType<HeightmapComponentTexture>();
            if (temp.Count != 0)
            {
                HeightmapComponentTexture hmap = ComponentManager.Instance.GetEntityComponent<HeightmapComponentTexture>(temp.First());

                int roundX = (int)Math.Round(position.X); int roundY = (int)Math.Round(position.Z);
                if (roundX >= hmap.Width - 1 || roundY >= hmap.Height - 1)
                {
                    return 0;
                }
                if (roundY <= 0 && roundX >= 0)
                    return hmap.HeightMapData[roundX, -roundY];
            }
            return 0;
        }

        /// <summary>
        /// Function to get the height of the current position
        /// using BarryCentric for an exact height in the current 
        /// triangle
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public float GetHeight(Vector3 position)
        {
            List<int> temp = ComponentManager.Instance.GetAllEntitiesWithComponentType<HeightmapComponentTexture>();
            if (temp.Count != 0)
            {
                HeightmapComponentTexture hmap = ComponentManager.Instance.GetEntityComponent<HeightmapComponentTexture>(temp.First());
                float gridSquareSize = (hmap.Width * hmap.Height) / ((float)hmap.HeightMapData.Length - 1);
                int gridX = (int)Math.Floor(position.X / gridSquareSize);
                int gridZ = -(int)Math.Floor(position.Z / gridSquareSize);

                if (gridX >= hmap.HeightMapData.Length - 1 || gridZ >= hmap.HeightMapData.Length - 1 || gridX < 0 || gridZ < 0)
                {
                    return 0;
                }
                float xCoord = (position.X % gridSquareSize) / gridSquareSize;
                float zCoord = (position.Z % gridSquareSize) / gridSquareSize;
                float answer = 0;

                if (xCoord <= (1 - zCoord))
                {
                    answer = BarryCentric(
                        new Vector3(0, hmap.HeightMapData[gridX, gridZ], 0),
                        new Vector3(1, hmap.HeightMapData[gridX + 1, gridZ], 0),
                        new Vector3(0, hmap.HeightMapData[gridX, gridZ + 1], 1),
                        new Vector2(xCoord, zCoord));
                }
                else
                {
                    answer = BarryCentric(
                        new Vector3(1, hmap.HeightMapData[gridX + 1, gridZ], 0),
                        new Vector3(1, hmap.HeightMapData[gridX + 1, gridZ + 1], 1),
                        new Vector3(0, hmap.HeightMapData[gridX, gridZ + 1], 1),
                        new Vector2(xCoord, zCoord));
                }
                return answer;
            }
            return 0;
        }
        private float BarryCentric(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 pos)
        {
            float det = (p2.Z - p3.Z) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Z - p3.Z);
            float l1 = ((p2.Z - p3.Z) * (pos.X - p3.X) + (p3.X - p2.X) * (pos.Y - p3.Z)) / det;
            float l2 = ((p3.Z - p1.Z) * (pos.X - p3.X) + (p1.X - p3.X) * (pos.Y - p3.Z)) / det;
            float l3 = 1.0f - l1 - l2;
            return l1 * p1.Y + l2 * p2.Y + l3 * p3.Y;
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
