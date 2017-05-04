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
    public class PhysicsParticleSystem : PhysicsSystem
    {
        public override PhysicsType PhysicsType { get; set; }
        public PhysicsParticleSystem()
        {
            PhysicsType = PhysicsType.Particle;
        }
        public override void Update(PhysicsComponent physic, float dt)
        {
            //TODO: ParticleSystem
            throw new NotImplementedException();
        }

        public override void UpdateAcceleration(PhysicsComponent physic)
        {
            physic.LastAcceleration = physic.Acceleration;
            Vector3 new_acceleration = physic.Forces / physic.Mass;
            Vector3 avg_acceleration = (physic.LastAcceleration + new_acceleration) / 2;
            physic.Acceleration = avg_acceleration;
        }

        public override void UpdateVelocity(PhysicsComponent physic, float dt)
        {
            physic.Velocity += (physic.Acceleration + physic.Forces / physic.Mass) * dt;
        }
    }
}
