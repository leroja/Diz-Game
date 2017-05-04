using GameEngine.Source.Systems.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Enums;

namespace GameEngine.Source.Systems
{
    public class PhysicsParticleSystem : IPhysicsTypeSystem
    {
        public PhysicsParticleSystem(IPhysics physicsSystem) : base(physicsSystem)
        {
            PhysicsType = PhysicsType.Particle;
        }
        /// <summary>
        /// Updates the particle using Euler method because,
        /// particles using constant acceleration
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        public override void Update(PhysicsComponent physic, float dt)
        {
            PhysicsSystem.UpdateMass(physic);
            PhysicsSystem.UpdateGravity(physic, dt);
            PhysicsSystem.UpdateForce(physic);
            PhysicsSystem.UpdateEulerAcceleration(physic);
            //TODO: ParticleSystem
            // Code here:

            //
            PhysicsSystem.UpdateVelocity(physic, dt);
            PhysicsSystem.UpdateDeceleration(physic);
        }
    }
}
