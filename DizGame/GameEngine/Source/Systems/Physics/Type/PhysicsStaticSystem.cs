using GameEngine.Source.Systems.Interfaces;
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
        /// Updates Acceleration, mass, gravity, force, velocity, position and deceleration
        /// Using non Euler for acceleration
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        public override void Update(PhysicsComponent physic, float dt)
        {
            //TODO: StaticSystem
            PhysicsSystem.UpdateMass(physic);
            
            // Code here:

            //

        }


    }
}
