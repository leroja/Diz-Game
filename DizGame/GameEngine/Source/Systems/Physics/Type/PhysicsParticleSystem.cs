using GameEngine.Source.Systems.Interfaces;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using GameEngine.Source.Systems.AbstractClasses;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// Class for updating the particles physic
    /// </summary>
    public class PhysicsParticleSystem : IPhysicsTypeSystem
    {
        /// <summary>
        /// Constructor which sets the PhysicType
        /// </summary>
        /// <param name="physicsSystem"></param>
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
            UpdatePosition(physic, dt);
            //TODO: ParticleSystem
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
