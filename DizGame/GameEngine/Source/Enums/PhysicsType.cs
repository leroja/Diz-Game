
namespace GameEngine.Source.Enums
{
    /// <summary>
    /// PhysicType is used to define the objects physic abilities
    /// </summary>
    public enum PhysicsType : int
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// "Immovable entity"
        /// </summary>
        Static,
        /// <summary>
        /// Single rigid body
        /// </summary>
        Rigid,
        /// <summary>
        /// Represents a small lightweight rigid body
        /// </summary>
        Particle,
        /// <summary>
        /// Used for a ragdoll like body with joints
        /// </summary>
        Ragdoll,
        /// <summary>
        /// Bullets, balls etc
        /// </summary>
        Projectiles,
        /// <summary>
        /// Used for cloth, jelly etc
        /// </summary>
        Soft
    }

    /// <summary>
    /// Gravitytype is used to define which gravity the object should use.
    /// </summary>
    public enum GravityType
    {
        /// <summary>
        /// Apply none gravity.
        /// </summary>
        None,
        /// <summary>
        /// Apply the worlds gravity.
        /// </summary>
        World,
        /// <summary>
        /// Apply the objects own gravity.
        /// </summary>
        Self
    }
}
