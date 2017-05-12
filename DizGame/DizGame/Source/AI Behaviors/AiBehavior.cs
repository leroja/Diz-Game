using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.AI_States
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AiBehavior
    {
        /// <summary>
        /// The ID of the closest enemy
        /// </summary>
        public int ClosestEnemy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float DistanceToClosestEnemy { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="AIComp"></param>
        /// <param name="gameTime"></param>
        public abstract void Update(AIComponent AIComp, GameTime gameTime);
        public abstract void OnEnter(Vector3 rotation);


        /// <summary>
        /// 
        /// </summary>
        public void FindClosestEnemy(AIComponent AIComp)
        {
            DistanceToClosestEnemy = float.MaxValue;

            var PlayerIds = ComponentManager.Instance.GetAllEntitiesWithComponentType<PlayerComponent>();
            var AiIds = ComponentManager.Instance.GetAllEntitiesWithComponentType<AIComponent>();

            var transformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);
            //float distance = float.MaxValue;

            // Find wich Enemy Entity is the closest one
            //int closestEntity = AIComp.ID;
            foreach (var entityId in PlayerIds)
            {
                //var playComp = ComponentManager.Instance.GetEntityComponent<PlayerComponent>(entityId);
                var transComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entityId);

                var dist = Vector3.Distance(transformComp.Position, transComp.Position);
                if (dist < DistanceToClosestEnemy)
                {
                    //closestEntity = entityId;
                    ClosestEnemy = entityId;
                    DistanceToClosestEnemy = dist;
                }
            }

            // todo ska vi kolla mot andra AI:s eller inte?
            foreach (var EntityId in AiIds)
            {
                var transComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(EntityId);

                if (AIComp.ID == EntityId)
                    continue;
                
                var dist = Vector3.Distance(transformComp.Position, transComp.Position);
                if (dist < DistanceToClosestEnemy)
                {
                    //closestEntity = EntityId;
                    ClosestEnemy = EntityId;
                    DistanceToClosestEnemy = dist;
                }
            }
        }


        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public Vector3 GetRotationToClosestEnenmy(AIComponent AIComp)
        {
            var ClosestEnemyTransFormComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(ClosestEnemy);
            var AITransformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);

            float x = ClosestEnemyTransFormComp.Position.X - AITransformComp.Position.X;
            float z = ClosestEnemyTransFormComp.Position.Z - AITransformComp.Position.Z;
            //float x1 = AITransformComp.Position.X - ClosestEnemyTransFormComp.Position.X;
            //float z1 = AITransformComp.Position.Z - ClosestEnemyTransFormComp.Position.Z;
            //float desiredAngle = (float)Math.Atan2(z, x);
            float desiredAngle = (float)Math.Atan2(x, z) + MathHelper.Pi; // + PI = fulhack
            //float desiredAngle1 = (float)Math.Atan2(z1, x1);

            return WrapAngle(new Vector3(0, desiredAngle, 0));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public Vector3 WrapAngle(Vector3 rotation)
        {
            while (rotation.Y < -MathHelper.Pi)
            {
                rotation.Y += MathHelper.TwoPi;
            }
            while (rotation.Y > MathHelper.Pi)
            {
                rotation.Y -= MathHelper.TwoPi;
            }
            return rotation;
        }



        /// <summary>
        /// Calculates the angle that an object should face, given its position, its
        /// target's position, its current angle, and its maximum turning speed.
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

        public float WrapAngle(float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }
    }
}
