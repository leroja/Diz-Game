using GameEngine.Source.Components;
using GameEngine.Source.Systems.Abstract_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems.Interface
{
    public abstract class IPhysicsType : ISystem
    {
        public abstract void Update(int entityID, float dt);
    }
}
