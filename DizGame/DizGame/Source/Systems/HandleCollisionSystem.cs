﻿using System;
using GameEngine.Source.Managers;
using GameEngine.Source.Components;
using DizGame.Source.Components;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// 
    /// </summary>
    public class HandleCollisionSystem : IObserver<Tuple<object, object>>
    {
        /// <summary>
        /// 
        /// </summary>
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public void OnError(Exception error)
        {
            throw error;
        }

        /// <summary>
        /// This method is called from the Observable class. It handles the collisions with 
        /// players and AI and other moving objects
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(Tuple<object, object> value)
        {
            int id1 = (int)value.Item1;
            int id2 = (int)value.Item2;
            PhysicsComponent phys1 = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(id1);
            PhysicsComponent phys2 = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(id2);
            ModelComponent mComp1 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(id1);
            ModelComponent mComp2 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(id2);
            if ((ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(id1) ||
                ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(id1)) &&
                (ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(id2) ||
                ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(id2)))
            {
                TransformComponent tComp1 = ComponentManager.Instance.GetEntityComponent<TransformComponent>(id1);
                TransformComponent tComp2 = ComponentManager.Instance.GetEntityComponent<TransformComponent>(id2);
                //Vector3 dir1 = phys1.Velocity;
                //Vector3 dir2 = phys2.Velocity;
                //dir1.Normalize();
                //dir2.Normalize();
                //Ray ray1 = new Ray(tComp1.Position, dir1);
                //Ray ray2 = new Ray(tComp2.Position, dir2);

                //if ((ray1.Intersects(((BoundingSphere3D)mComp2.BoundingVolume.Bounding).Sphere) != null) && 
                //    ray2.Intersects(((BoundingSphere3D)mComp2.BoundingVolume.Bounding).Sphere) != null)
                //{
                //    tComp1.Position = tComp1.PreviousPosition;
                //    tComp2.Position = tComp2.PreviousPosition;
                //}
                //else if ((ray1.Intersects(((BoundingSphere3D)mComp2.BoundingVolume.Bounding).Sphere) != null) &&
                //    ray2.Intersects(((BoundingSphere3D)mComp2.BoundingVolume.Bounding).Sphere) == null)
                //{
                //    tComp1.Position = tComp1.PreviousPosition;
                //}
                //else if ((ray1.Intersects(((BoundingSphere3D)mComp2.BoundingVolume.Bounding).Sphere) == null) &&
                //    ray2.Intersects(((BoundingSphere3D)mComp2.BoundingVolume.Bounding).Sphere) != null)
                //{
                //    tComp2.Position = tComp2.PreviousPosition;
                //}
                tComp1.Position = tComp1.PreviousPosition;
                tComp2.Position = tComp2.PreviousPosition;
            }
        }
    }
}