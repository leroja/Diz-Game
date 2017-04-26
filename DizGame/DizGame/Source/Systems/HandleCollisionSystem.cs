using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DizGame.Source.Systems
{
    class HandleCollisionSystem : IObserver<Tuple<BoundingSphere, BoundingSphere>>
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
            throw new NotImplementedException();
        }

        public void OnNext(Tuple<BoundingSphere, BoundingSphere> value)
        {
            
        }
    }
}
