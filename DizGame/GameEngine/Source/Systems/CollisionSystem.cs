using GameEngine.Source.Components;
using GameEngine.Source.Components.Interface;
using GameEngine.Source.Managers;
using GameEngine.Source.Systems.Abstract_classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems
{
    class CollisionSystem : IUpdate, IObservable<List<Tuple<BoundingSphereComponent, BoundingSphereComponent>>>
    {
        List<IObserver<List<Tuple<BoundingSphereComponent, BoundingSphereComponent>>>> observers;
        /// <summary>
        /// Checks all entities with boundingspherecomponent if they collide and sets those components'
        /// hasCollided variables to true if they collide.
        /// </summary>
        /// 
        CollisionSystem()
        {
            observers = new List<IObserver<List<Tuple<BoundingSphereComponent, BoundingSphereComponent>>>>();
        }

        public void CollisionDetection()
        {
            List<int> sphereEntities = ComponentManager.Instance.GetAllEntitiesWithComponentType<BoundingSphereComponent>();

            for (int i = 0; i < sphereEntities.Count - 1; i++)
            {
                BoundingSphereComponent sphereComp1 = ComponentManager.Instance.GetEntityComponent<BoundingSphereComponent>(sphereEntities[i]);

                for (int j = 1; j < sphereEntities.Count; i++)
                {
                    BoundingSphereComponent sphereComp2 = ComponentManager.Instance.GetEntityComponent<BoundingSphereComponent>(sphereEntities[j]);
                    if (sphereComp1.sphere.Intersects(sphereComp2.sphere))
                    {
                        if (ComponentManager.Instance.CheckIfEntityHasComponent<ModelComponent>(sphereEntities[i]) && ComponentManager.Instance.CheckIfEntityHasComponent<ModelComponent>(sphereEntities[j]))
                        {
                            ModelComponent mComp1 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(sphereEntities[i]);
                            ModelComponent mComp2 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(sphereEntities[j]);
                            List<BoundingSphere> spheres1 = mComp1.Model.Meshes.Select(s => s.BoundingSphere).ToList();
                            List<BoundingSphere> spheres2 = mComp2.Model.Meshes.Select(s => s.BoundingSphere).ToList();

                            if (FindFirstHit(spheres1, spheres2))
                            {

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<List<Tuple<BoundingSphereComponent, BoundingSphereComponent>>> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        /// <summary>
        /// 
        /// </summary>
        private class Unsubscriber : IDisposable
        {
            private List<IObserver<List<Tuple<BoundingSphereComponent, BoundingSphereComponent>>>> _observers;
            private IObserver<List<Tuple<BoundingSphereComponent, BoundingSphereComponent>>> _observer;

            public Unsubscriber(List<IObserver<List<Tuple<BoundingSphereComponent, BoundingSphereComponent>>>> observers,
                IObserver<List<Tuple<BoundingSphereComponent, BoundingSphereComponent>>> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
            /// <summary>
            /// Goes through every boundingsphere in 
            /// </summary>
            /// <param name="spheres1"></param>
            /// <param name="spheres2"></param>
            /// <returns> true if any of spheres1's and spheres2's spheres collide. Otherwise false</returns>
        private bool FindFirstHit(List<BoundingSphere> spheres1, List<BoundingSphere> spheres2)
        {
            BoundingSphere s1 = spheres1.FirstOrDefault();
            BoundingSphere s2 = spheres2.FirstOrDefault();
            if (s1 != null || s2 != null)
            {
                if (s1.Intersects(s2)) return true;
                else if (spheres1.Count > spheres2.Count)
                {
                    spheres1.Remove(s1);
                    FindFirstHit(spheres1, spheres2);
                }
                else if (spheres1.Count < spheres2.Count)
                {
                    spheres2.Remove(s2);
                    FindFirstHit(spheres1, spheres2);
                }
            }
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
