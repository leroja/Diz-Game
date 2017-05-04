using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems.Interface
{
    public interface IPhysics
    {
        PhysicsType PhysicsType { get; set; }
        void Update(PhysicsComponent physic, float dt);
        void UpdateAcceleration(PhysicsComponent physic);
        void UpdateVelocity(PhysicsComponent physic, float dt);
        void UpdateDeceleration(PhysicsComponent physic);
        void UpdateWeight(PhysicsComponent physic, float gravity);
        void UpdateGravity(PhysicsComponent physic, float dt);
        void UpdateMass(PhysicsComponent physic);
        void UpdateForce(PhysicsComponent physic);
        void UpdatePhysicComponentByType(PhysicsComponent physic, float dt);
    }
}
