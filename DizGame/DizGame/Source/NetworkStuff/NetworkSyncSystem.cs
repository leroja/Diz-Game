using GameEngine.Source.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using static GameEngine.Source.Systems.CollisionSystem;

namespace DizGame.Source.NetworkStuff
{
    /// <summary>
    /// 
    /// </summary>
    public class NetworkSyncSystem : IUpdate, IObservable<Tuple<object, object>>, IObserver<Tuple<object, object>> // TODO: not sure what the observer/observable shall send/receive
    {
        List<IObserver<Tuple<object, object>>> observers;

        private const double updateInterval = 0.05;
        private double remaingTime = 0;

        /// <summary>
        /// 
        /// </summary>
        public NetworkSyncSystem()
        {
            observers = new List<IObserver<Tuple<object, object>>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            remaingTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (remaingTime > updateInterval)
            {
                SyncObjects(gameTime);
                remaingTime = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        private void SyncObjects(GameTime gameTime)
        {
            foreach (var entiy in ComponentManager.GetAllEntitiesWithComponentType<SyncComponent>())
            {
                foreach (var observer in observers)
                {
                    //observer.OnNext();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddOrUpdateObjects()
        {

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
        /// <param name="value"></param>
        public void OnNext(Tuple<object, object> value)
        {
            // AddOrUpdateObject
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }
}