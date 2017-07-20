using ContentProject;
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
    /// <summary>
    /// 
    /// </summary>
    public class CollisionSystem : IUpdate, IObservable<Tuple<object, object>>
    {
        //Holds all the observers for this class
        List<IObserver<Tuple<object, object>>> observers;

        partial class Unsubscriber : IDisposable
        {
            private List<IObserver<Tuple<object, object>>> _observers;
            private IObserver<Tuple<object, object>> _observer;

            public Unsubscriber(List<IObserver<Tuple<object, object>>> observers,
                IObserver<Tuple<object, object>> observer)
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
        /// Constructor
        /// </summary>
        public CollisionSystem()
        {
            observers = new List<IObserver<Tuple<object, object>>>();
        }

        /// <summary>
        /// Checks all model entities with their BoundingVolume if it intersects 
        /// with another boundingVolume. Then notifies all observers and sends those model
        /// entity ids to the observers. 
        /// 
        /// </summary>
        public void CollisionDetection()
        {
            List<int> boundingEntities = ComponentManager.Instance.GetAllEntitiesWithComponentType<ModelComponent>();

            for (int i = 0; i < boundingEntities.Count - 1; i++)
            {
                ModelComponent modComp1 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(boundingEntities[i]);

                for (int j = i + 1; j < boundingEntities.Count; j++)
                {
                    ModelComponent modComp2 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(boundingEntities[j]);
                    if (modComp1 != null && modComp2 != null && modComp1.BoundingVolume != null && modComp2.BoundingVolume != null)
                    {
                        if (modComp1.BoundingVolume.Bounding.Intersects(modComp2.BoundingVolume.Bounding))
                        {
                            foreach (IObserver<Tuple<object, object>> observer in observers)
                            {
                                observer.OnNext(new Tuple<object, object>(boundingEntities[i], boundingEntities[j]));
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
        public IDisposable Subscribe(IObserver<Tuple<object, object>> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            CollisionDetection();
        }
    }
}