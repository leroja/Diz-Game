using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using AnimationContentClasses;
using GameEngine.Source.Managers;
using GameEngine.Source.Components;
using DizGame.Source.Components;

namespace DizGame.Source.Systems
{
    class HandleCollisionSystem : IObserver<Tuple<int, int>>
    {
        public HandleCollisionSystem()
        {

        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        /// <summary>
        /// This method is called from the Observable class. It handles the collisions
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(Tuple<int, int> value)
        {
            ModelComponent mComp1 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(value.Item1);
            ModelComponent mComp2 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(value.Item2);

            if ((ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(value.Item1) ||
                ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(value.Item1)) &&
                (ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(value.Item2) ||
                ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(value.Item2)))
            {

                PhysicsComponent pC1 = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(value.Item1);
                PhysicsComponent pC2 = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(value.Item2);
                TransformComponent tComp1 = ComponentManager.Instance.GetEntityComponent<TransformComponent>(value.Item1);
                TransformComponent tComp2 = ComponentManager.Instance.GetEntityComponent<TransformComponent>(value.Item1);
                tComp1.Position -= pC1.Velocity;
                tComp2.Position -= pC2.Velocity;
            }
            //else if (ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(value.Item1.Key) ||
            //    ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(value.Item1.Key) &&
            //    !ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(value.Item2.Key) ||
            //    !ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(value.Item2.Key))
            //{

            //}

            //else if (!ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(value.Item1.Key) ||
            //    !ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(value.Item1.Key) &&
            //    ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(value.Item2.Key) ||
            //    ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(value.Item2.Key))
            //{

            //}

            if (ComponentManager.Instance.CheckIfEntityHasComponent<BulletComponent>(value.Item1))
            {
                if (ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(value.Item2) ||
                ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(value.Item2))
                {
                    HealthComponent health = ComponentManager.Instance.GetEntityComponent<HealthComponent>(value.Item2);
                    BulletComponent bullet = ComponentManager.Instance.GetEntityComponent<BulletComponent>(value.Item1);
                    ComponentManager.Instance.RemoveEntity(value.Item1);
                    ComponentManager.Instance.RecycleID(value.Item1);
                    health.Health -= bullet.Damage;
                    
                }
            }
            else if (ComponentManager.Instance.CheckIfEntityHasComponent<BulletComponent>(value.Item2))
            {
                if (ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(value.Item1) ||
                ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(value.Item1))
                {
                    HealthComponent health = ComponentManager.Instance.GetEntityComponent<HealthComponent>(value.Item1);
                    BulletComponent bullet = ComponentManager.Instance.GetEntityComponent<BulletComponent>(value.Item2);
                    ComponentManager.Instance.RemoveEntity(value.Item2);
                    ComponentManager.Instance.RecycleID(value.Item2);
                    health.Health -= bullet.Damage;
                }
            }
        }
    }
}
