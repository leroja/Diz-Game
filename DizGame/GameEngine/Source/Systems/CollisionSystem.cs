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
            List<int> VolEntities = ComponentManager.Instance.GetAllEntitiesWithComponentType<ModelComponent>();
            List<Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>> collidedVolumes = new List<Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>>();

            for (int i = 0; i < VolEntities.Count - 1; i++)
            {
                ModelComponent modelComp1 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(VolEntities[i]);

                for (int j = 1; j < VolEntities.Count; i++)
                {
                    ModelComponent modelComp2 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(VolEntities[j]);
                    if (modelComp1.BoundingVolume.Bounding.Intersects(modelComp2.BoundingVolume.Bounding))
                    {
                        if (ComponentManager.Instance.CheckIfEntityHasComponent<ModelComponent>(VolEntities[i]) && ComponentManager.Instance.CheckIfEntityHasComponent<ModelComponent>(VolEntities[j]))
                        {
                            List<BoundingVolume> spheres1 = modelComp1.BoundingVolume.Volume;
                            List<BoundingVolume> spheres2 = modelComp2.BoundingVolume.Volume;
                            if (FindFirstHit(modelComp1.ID, modelComp2.ID, spheres1, spheres2, out Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>> tuple))
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
        /// <returns> true if any of spheres1's and spheres2's spheres collide. Otherwise false</returns>
        private bool FindFirstHit(int mod1ID, int mod2ID, List<BoundingVolume> spheres1, List<BoundingVolume> spheres2, out Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>> tuple)
        {
            IBounding3D s1 = spheres1[0].Bounding;
            IBounding3D s2 = spheres2[0].Bounding;
            if (s1 != null || s2 != null)
            {
                if (s1.Intersects(s2))
                {
                    tuple = new Tuple<KeyValuePair<int, IBounding3D>, KeyValuePair<int, IBounding3D>>(new KeyValuePair<int, IBounding3D>(mod1ID, s1), new KeyValuePair<int, IBounding3D>(mod2ID, s2));
                    return true;
                }
                else if (spheres1.Count > spheres2.Count)
                {
                    BoundingVolume[] temp1 = new BoundingVolume[spheres1.Count];
                    spheres1.CopyTo(temp1);
                    
                    temp1.Skip(1);
                    FindFirstHit(mod1ID, mod2ID, temp1.ToList(), spheres2, out tuple);
                }
                else if (spheres1.Count < spheres2.Count)
                {
                    BoundingVolume[] temp2 = new BoundingVolume[spheres2.Count];
                    spheres2.CopyTo(temp2);
                    temp2.Skip(1);
                    FindFirstHit(mod1ID, mod2ID, spheres1, temp2.ToList(), out tuple);
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
