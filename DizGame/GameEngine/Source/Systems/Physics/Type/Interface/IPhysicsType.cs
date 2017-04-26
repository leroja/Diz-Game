using GameEngine.Source.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems.Interface
{
    public abstract class IPhysicsType : ISystem
    {
        public abstract void Update(PhysicsComponent physic, float dt);
    }
}
