using GameEngine.Source.Components;

namespace GameEngine.Source.Systems.Interfaces
{
    /// <summary>
    /// Public physic interface
    /// </summary>
    public interface IPhysics
    {
        /// <summary>
        /// Used to update regular acceleration e.g.(A = F/M)
        /// </summary>
        /// <param name="physic"></param>
        void UpdateAcceleration(PhysicsComponent physic);
        /// <summary>
        /// Used to update Euler acceleration
        /// </summary>
        /// <param name="physic"></param>
        void UpdateEulerAcceleration(PhysicsComponent physic);
        /// <summary>
        /// Used to update velocity
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        void UpdateVelocity(PhysicsComponent physic, float dt);
        /// <summary>
        /// Used to update deceleration
        /// </summary>
        /// <param name="physic"></param>
        void UpdateDeceleration(PhysicsComponent physic);
        /// <summary>
        /// Used to apply gravity 
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        void UpdateGravity(PhysicsComponent physic, float dt);
        /// <summary>
        /// Used to update the mass (M = density * volume)
        /// </summary>
        /// <param name="physic"></param>
        void UpdateMass(PhysicsComponent physic);
        /// <summary>
        /// Used to update the force (F = M * A)
        /// </summary>
        /// <param name="physic"></param>
        void UpdateForce(PhysicsComponent physic);
    }
}
