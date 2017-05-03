using GameEngine.Source.Systems.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;

namespace GameEngine.Source.Systems
{
    public class PhysicsSoftSystem : PhysicsSystem
    {
        public override PhysicsType PhysicsType { get; set; }
        public PhysicsSoftSystem()
        {
            PhysicsType = PhysicsType.Soft;
        }
        public override void Update(PhysicsComponent physic, float dt)
        {
            //TODO: SoftSystem
            throw new NotImplementedException();
        }
    }
}
