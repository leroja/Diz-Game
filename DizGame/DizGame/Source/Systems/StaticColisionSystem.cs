using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using System;

namespace DizGame.Source.Systems
{
    class StaticColisionSystem : IObserver<Tuple<object, object>>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(Tuple<object, object> value)
        {
            BulletStaticColision(value);
            StaticPlayerKolision(value);

        }

        private void StaticPlayerKolision(Tuple<object, object> value)
        {
            int id1 = (int)value.Item1;
            int id2 = (int)value.Item2;
            if (((ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(id1) || ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(id1)) 
                && ComponentManager.Instance.CheckIfEntityHasComponent<PhysicsComponent>(id1)) 
                && (ComponentManager.Instance.CheckIfEntityHasComponent<PhysicsComponent>(id2) && ComponentManager.Instance.CheckIfEntityHasComponent<ModelComponent>(id2)))
            {
                if (ComponentManager.Instance.GetEntityComponent<ModelComponent>(id2).IsStatic)
                {
                    PhysicsComponent physicsComp = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(id1);
                    TransformComponent trans = ComponentManager.Instance.GetEntityComponent<TransformComponent>(id1);
                    trans.Position = trans.PreviousPosition;
                }
            }
            else if (((ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(id2) || ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(id2)) 
                && ComponentManager.Instance.CheckIfEntityHasComponent<PhysicsComponent>(id2)) 
                && (ComponentManager.Instance.CheckIfEntityHasComponent<PhysicsComponent>(id1) && ComponentManager.Instance.CheckIfEntityHasComponent<ModelComponent>(id1)))
            {
                
                if (ComponentManager.Instance.GetEntityComponent<ModelComponent>(id1).IsStatic)
                {
                    PhysicsComponent physicsComp1 = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(id2);
                    PhysicsComponent physicsComp2 = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(id1);
                    TransformComponent trans = ComponentManager.Instance.GetEntityComponent<TransformComponent>(id2);
                    //Vector3 dir = physicsComp.Velocity;
                    //dir.Normalize();
                    //Ray ray = new Ray(trans.Position, dir);
                    //if ()
                    //if (trans.Position == trans.PreviousPosition)
                    //{
                    //    physicsComp.Velocity -= physicsComp.Velocity;
                    //}
                    trans.Position = trans.PreviousPosition;
                }
            }

        }

        private void BulletStaticColision(Tuple<object, object> value)
        {
            int id1 = (int)value.Item1;
            int id2 = (int)value.Item2;
            if (ComponentManager.Instance.CheckIfEntityHasComponent<BulletComponent>(id1) && ComponentManager.Instance.CheckIfEntityHasComponent<ModelComponent>(id2))
            {
                if (ComponentManager.Instance.GetEntityComponent<ModelComponent>(id2).IsStatic)
                {
                    ComponentManager.Instance.RemoveEntity(id1);
                    ComponentManager.Instance.RecycleID(id1);
                }
            }
            else if (ComponentManager.Instance.CheckIfEntityHasComponent<BulletComponent>(id2) && ComponentManager.Instance.CheckIfEntityHasComponent<ModelComponent>(id1))
            {
                if (ComponentManager.Instance.GetEntityComponent<ModelComponent>(id1).IsStatic)
                {
                    ComponentManager.Instance.RemoveEntity(id2);
                    ComponentManager.Instance.RecycleID(id2);
                }
            }
        }
    }
}
