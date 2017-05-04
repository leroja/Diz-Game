using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems.Interface
{
    public abstract class IPhysicsTypeSystem : ISystem
    {
        public IPhysics PhysicsSystem { get; set; }
        public PhysicsType PhysicsType { get; set; }
        public bool Euler { get; set; }
        public IPhysicsTypeSystem(IPhysics physicsSystem)
        {
            this.PhysicsSystem = physicsSystem;
        }
        public abstract void Update(PhysicsComponent physic, float dt);
    }
}
