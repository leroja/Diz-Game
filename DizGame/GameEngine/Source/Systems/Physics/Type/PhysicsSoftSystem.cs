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
    public class PhysicsSoftSystem : IPhysicsTypeSystem
    {
        public PhysicsSoftSystem(IPhysics physicsSystem) : base(physicsSystem)
        {
            PhysicsType = PhysicsType.Soft;
        }
        public override void Update(PhysicsComponent physic, float dt)
        {
            //TODO: SoftSystem
            PhysicsSystem.UpdateAcceleration(physic);
            PhysicsSystem.UpdateMass(physic);
            PhysicsSystem.UpdateGravity(physic, dt);
            PhysicsSystem.UpdateForce(physic);
            PhysicsSystem.UpdateVelocity(physic, dt);
            // Code here:

            //
            PhysicsSystem.UpdateDeceleration(physic);
        }
    }
}
