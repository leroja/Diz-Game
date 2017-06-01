using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
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
