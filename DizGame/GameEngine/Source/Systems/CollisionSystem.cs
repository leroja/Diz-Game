using AnimationContentClasses;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems
{
    class CollisionSystem : IUpdate, IObservable<List<Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>>>
    {
        //Holds all the observers for this class
        List<IObserver<List<Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>>>> observers;

        partial class Unsubscriber : IDisposable
        {
            private List<IObserver<List<Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>>>> _observers;
            private IObserver<List<Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>>> _observer;

            public Unsubscriber(List<IObserver<List<Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>>>> observers,
                IObserver<List<Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>>> observer)
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

        //Constructor
        public CollisionSystem()
        {
            observers = new List<IObserver<List<Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>>>>();
        }

        /// <summary>
        /// Checks all entities with boundingspherecomponent if they collide and sets those components'
        /// hasCollided variables to true if they collide.
        /// </summary>
        public void CollisionDetection()
        {
            List<int> sphereEntities = ComponentManager.Instance.GetAllEntitiesWithComponentType<ModelComponent>();
            List<Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>> collidedVolumes = new List<Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>>();

            for (int i = 0; i < sphereEntities.Count - 1; i++)
            {
                ModelComponent modComp1 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(sphereEntities[i]);

                for (int j = i + 1; j < sphereEntities.Count; i++)
                {
                    ModelComponent modComp2 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(sphereEntities[j]);
                    if (modComp1.BoundingVolume.Bounding.Intersects(modComp2.BoundingVolume.Bounding))
                    {
                        if (ComponentManager.Instance.CheckIfEntityHasComponent<ModelComponent>(sphereEntities[i]) && ComponentManager.Instance.CheckIfEntityHasComponent<ModelComponent>(sphereEntities[j]))
                        {

                            if (FindFirstHit(new KeyValuePair<int, List<BoundingVolume>>(modComp1.ID, modComp1.BoundingVolume.Volume), new KeyValuePair<int, List<BoundingVolume>>(modComp2.ID, modComp2.BoundingVolume.Volume), out Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>> tuple))
                            {
                                if (tuple != null)
                                    collidedVolumes.Add(tuple);
                            }
                        }
                    }
                }
            }
            foreach (IObserver<List<Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>>> observer in observers)
            {
                observer.OnNext(collidedVolumes);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<List<Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>>> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        /// <summary>
        /// Goes through every boundingsphere in 
        /// </summary>
        /// <param name="spheres1"></param>
        /// <param name="spheres2"></param>
        /// <param name="tuple"></param>
        /// <returns>true if any of spheres1's and spheres2's spheres collide. Otherwise false</returns>
        private bool FindFirstHit(KeyValuePair<int, List<BoundingVolume>> volume1, KeyValuePair<int, List<BoundingVolume>> volume2, out Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>> tuple)
        {
            BoundingVolume s1 = volume1.Value.FirstOrDefault();
            BoundingVolume s2 = volume2.Value.FirstOrDefault();
            if (s1 != null || s2 != null)
            {
                if (s1.Bounding.Intersects(s2.Bounding))
                {
                    tuple = new Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>(new KeyValuePair<int, IBounding3D>(volume1.Key, s1.Bounding), new KeyValuePair<int, IBounding3D>(volume2.Key, s2.Bounding));
                    return true;
                }
                else if (volume1.Value.Count > volume2.Value.Count)
                {
                    volume1.Value.Remove(s1);
                    FindFirstHit(volume1, volume2, out tuple);
                }
                else if (volume1.Value.Count < volume2.Value.Count)
                {
                    volume2.Value.Remove(s2);
                    FindFirstHit(volume1, volume2, out tuple);
                }
            }
            tuple = null;
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            CollisionDetection();
        }
    }
}
