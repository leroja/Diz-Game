using System;
using System.Collections.Generic;
using System.Linq;
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
            var animComp = ComponentManager.Instance.GetEntityComponent<AnimationComponent>(AIComp.ID);

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

            animComp.CurrentTimeValue += TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds);

            BehaviorStuff(AIComp, transformComp);
        }

        /// <summary>
        /// Check whether the AI chould change behavior
        /// If it should then the method changes the behavior
        /// </summary>
        /// <param name="AIComp"> The AI component of the AI </param>
        /// <param name="transcomp"> The transorm component of the AI </param>
        private void BehaviorStuff(AIComponent AIComp, TransformComponent transcomp)
        {
            var worldTemp = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<WorldComponent>();
            var worldComp = (WorldComponent)worldTemp.Values.First();


            if (worldComp.Day % worldComp.ModulusValue == 0 && worldComp.Day != 0 && DistanceToClosestEnemy < AIComp.ChaseDistance)
            {
                AIComp.ChangeBehavior("Chase", transcomp.Rotation);
            }
        }

        /// <summary>
        /// Gets the rotaion to next waypoint for the AI
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private Vector3 GetRotationToNextWayPoint(int ID)
        {
            var nextWaypoint = waypoints.Peek();
            var AITransformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(ID);

            float x = nextWaypoint.X - AITransformComp.Position.X;
            float z = nextWaypoint.Y - AITransformComp.Position.Z;
            float desiredAngle = (float)Math.Atan2(x, z) + MathHelper.Pi;

            return new Vector3(0, WrapAngle(desiredAngle), 0);
        }

        /// <summary>
        /// Override of object.ToString
        /// Returns the name of the behavior
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Patroll";
        }
    }
}
