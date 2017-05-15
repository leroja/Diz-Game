using GameEngine.Source.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using GameEngine.Source.Systems.AbstractClasses;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// Class for updating the static physic
    /// </summary>
    public class PhysicsStaticSystem : IPhysicsTypeSystem
    {
        /// <summary>
        /// Constructor which sets the PhysicType
        /// </summary>
        /// <param name="physicsSystem"></param>
        public PhysicsStaticSystem(IPhysics physicsSystem) : base(physicsSystem)
        {
            PhysicsType = PhysicsType.Static;
        }
        /// <summary>
        /// Updates Acceleration, mass, gravity, force, velocity, position and decaeleration
        /// Using non euler for acceleration
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        public override void Update(PhysicsComponent physic, float dt)
        {
            //TODO: StaticSystem
            PhysicsSystem.UpdateAcceleration(physic);
            PhysicsSystem.UpdateMass(physic);
            PhysicsSystem.UpdateGravity(physic, dt);
            PhysicsSystem.UpdateForce(physic);
            PhysicsSystem.UpdateVelocity(physic, dt);
            UpdatePosition(physic, dt);
            //TODO: StaticSystem
            // Code here:

            //
            PhysicsSystem.UpdateVelocity(physic, dt);
            PhysicsSystem.UpdateDeceleration(physic);
        }
        /// <summary>
        /// Updates the object position using its velocity * dt
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        private void UpdatePosition(PhysicsComponent physic, float dt)
        {
            ComponentManager.GetEntityComponent<TransformComponent>(physic.ID).Position
                    += physic.Velocity * dt;
        }
    }
}
