using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using GameEngine.Source.Systems.Interfaces;

namespace GameEngine.Source.Systems.AbstractClasses
{
    /// <summary>
    /// Abstract class, used for the Physic"Type"Systems e.g. (Particle, Rigid, soft etc.)
    /// </summary>
    public abstract class IPhysicsTypeSystem : ISystem
    {
        /// <summary>
        /// Base class which is set in the constructor
        /// </summary>
        public IPhysics PhysicsSystem { get; set; }
        /// <summary>
        /// Returns an enum of the classtype
        /// </summary>
        public PhysicsType PhysicsType { get; set; }

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="physicsSystem"></param>
        public IPhysicsTypeSystem(IPhysics physicsSystem)
        {
            this.PhysicsSystem = physicsSystem;
        }

        /// <summary>
        /// Abstract update function
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        public abstract void Update(PhysicsComponent physic, float dt);
    }
}
