using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Enums
{
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
    public enum GravityType
    {
        None,
        World,
        Self
    }
}
