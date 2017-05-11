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


        public Vector3 TestRot(TransformComponent AiTransComp)
        {
            var transComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(ClosestEnemy);

            var lookPos = AiTransComp.Position - transComp.Position;
            lookPos.Y = 0;

            ////var rot  = Quaternion
            //var rotation = Quaternion.LookRotation(lookPos);

            return Vector3.One;
        }

        // Todo, temp kanske ta bort

        // O is your object's position
        // P is the position of the object to face
        // U is the nominal "up" vector (typically Vector3.Y)
        // Note: this does not work when O is straight below or straight above P
        Matrix RotateToFace(Vector3 O, Vector3 P, Vector3 U)
        {
            Vector3 D = (O - P);
            Vector3 Right = Vector3.Cross(U, D);
            Vector3.Normalize(ref Right, out Right);
            Vector3 Backwards = Vector3.Cross(Right, U);
            Vector3.Normalize(ref Backwards, out Backwards);
            Vector3 Up = Vector3.Cross(Backwards, Right);
            Matrix rot = new Matrix(Right.X, Right.Y, Right.Z, 0, Up.X, Up.Y, Up.Z, 0, Backwards.X, Backwards.Y, Backwards.Z, 0, 0, 0, 0, 1);
            return rot;
        }

        public Quaternion GetRotation(Vector3 entityPos)
        {
            Vector3 targetPos = ComponentManager.Instance.GetEntityComponent<TransformComponent>(ClosestEnemy).Position;

            // the new forward vector, so the avatar faces the target
            Vector3 newForward = Vector3.Normalize(entityPos - targetPos);

            // calc the rotation so the avatar faces the target
            var src = Vector3.Forward;

            var dest = newForward;

            src.Normalize();
            dest.Normalize();

            float d = Vector3.Dot(src, dest);

            if (d >= 1f)
            {
                return Quaternion.Identity;
            }
            if (d < (1e-6f - 1.0f))
            {
                Vector3 axis = Vector3.Cross(Vector3.UnitX, src);

                if (axis.LengthSquared() == 0)
                {
                    axis = Vector3.Cross(Vector3.UnitY, src);
                }

                axis.Normalize();
                return Quaternion.CreateFromAxisAngle(axis, MathHelper.Pi);
            }


            float s = (float)Math.Sqrt((1 + d) * 2);
            float invS = 1 / s;

            Vector3 c = Vector3.Cross(src, dest);
            Quaternion q = new Quaternion(invS * c, 0.5f * s);
            q.Normalize();

            return q;
        }

    }
}
