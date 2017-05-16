using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Managers;
using GameEngine.Source.Components;

namespace DizGame.Source.AI_Behaviors
{
    /// <summary>
    /// A behavior that makes the AI patroll between some specified waypoint
    /// </summary>
    public class PatrollingBehavior : AiBehavior
    {
        private Queue<Vector2> waypoints;
        private float atDestinationLimit = 5f;

        /// <summary>
        /// A Constructor
        /// </summary>
        /// <param name="waypointList"> A list of Vector2 waypoints </param>
        public PatrollingBehavior(List<Vector2> waypointList)
        {
            waypoints = new Queue<Vector2>();

            foreach (var waypoint in waypointList)
            {
                waypoints.Enqueue(waypoint);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotation"></param>
        public override void OnEnter(Vector3 rotation)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AIComp"></param>
        /// <param name="gameTime"></param>
        public override void Update(AIComponent AIComp, GameTime gameTime)
        {
            var transformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);

            transformComp.Rotation = GetRotationToNextWayPoint(AIComp.ID);

            var height = GetCurrentHeight(transformComp.Position);

            var t = new Vector3(transformComp.Position.X, height, transformComp.Position.Z);
            t += transformComp.Forward * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            transformComp.Position = t;
            
            var location = new Vector2(transformComp.Position.X, transformComp.Position.Z);
            var DistanceToDestination = Vector2.Distance(location, waypoints.Peek());
            if (DistanceToDestination < atDestinationLimit)
            {
                var waypoint = waypoints.Dequeue();
                waypoints.Enqueue(waypoint);
            }

            BehaviorStuff(AIComp, transformComp);
        }


        private void BehaviorStuff(AIComponent AIComp, TransformComponent transcomp)
        {
            var worldTemp = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<WorldComponent>();
            var worldComp = (WorldComponent)worldTemp.Values.First();


            if (worldComp.Day % 2 == 0 && worldComp.Day != 0 && DistanceToClosestEnemy < AIComp.ChaseDistance)
            {
                AIComp.ChangeBehavior("Chase", transcomp.Rotation);
            }
        }

        private Vector3 GetRotationToNextWayPoint(int ID)
        {
            var nextWaypoint = waypoints.Peek();
            var AITransformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(ID);

            float x = nextWaypoint.X - AITransformComp.Position.X;
            float z = nextWaypoint.Y - AITransformComp.Position.Z;
            float desiredAngle = (float)Math.Atan2(x, z) + MathHelper.Pi;
            
            return new Vector3(0, WrapAngle(desiredAngle), 0);
        }
    }
}
