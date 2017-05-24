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
using GameEngine.Source.Enums;

namespace DizGame.Source.Systems
{
    class HandleCollisionSystem : IObserver<Tuple<object, object>>
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
        public void OnNext(Tuple<object, object> value)
        {
            int id1 = (int)value.Item1;
            int id2 = (int)value.Item2;
            ModelComponent mComp1 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(id1);
            ModelComponent mComp2 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(id2);
            PhysicsComponent phys1 = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(id1);
            PhysicsComponent phys2 = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(id2);
            //if ((ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(id1) ||
            //    ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(id1)) &&
            //    (ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(id2) ||
            //    ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(id2)))
            //{

            //    PhysicsComponent pC1 = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(id1);
            //    PhysicsComponent pC2 = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(id2);
            //    TransformComponent tComp1 = ComponentManager.Instance.GetEntityComponent<TransformComponent>(id1);
            //    TransformComponent tComp2 = ComponentManager.Instance.GetEntityComponent<TransformComponent>(id2);
            //    tComp1.Position -= pC1.Velocity;
            //    tComp2.Position -= pC2.Velocity;
            //}
            //if (phys1.PhysicsType == PhysicsType.Rigid )
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

        }
    }
}
