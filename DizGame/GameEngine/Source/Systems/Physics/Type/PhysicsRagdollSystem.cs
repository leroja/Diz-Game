using GameEngine.Source.Systems.Interfaces;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using GameEngine.Source.Systems.AbstractClasses;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// Class for updating the ragdoll physic
    /// </summary>
    public class PhysicsRagdollSystem : IPhysicsTypeSystem
    {
        /// <summary>
        /// Constructor which sets the PhysicType
        /// </summary>
        /// <param name="physicsSystem"></param>
        public PhysicsRagdollSystem(IPhysics physicsSystem) : base(physicsSystem)
        {
            PhysicsType = PhysicsType.Ragdoll;
        }

        /// <summary>
        /// Updates Acceleration, mass, gravity, force, velocity, position and deceleration
        /// Using non Euler for acceleration
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        public override void Update(PhysicsComponent physic, float dt)
        {
            //TODO: RagdollSystem
            PhysicsSystem.UpdateMass(physic);
            PhysicsSystem.UpdateGravity(physic, dt);
            PhysicsSystem.UpdateVelocity(physic, dt);
            UpdatePosition(physic, dt);
            //TODO: RagdollSystem
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
