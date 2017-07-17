using GameEngine.Source.Components;
using Microsoft.Xna.Framework;

namespace DizGame.Source.Components
{
    /// <summary>
    /// A component for bullets that are being fired in the game
    /// </summary>
    public class BulletComponent : IComponent
    {
        /// <summary>
        /// How far the bullet will reach
        /// </summary>
        public float MaxRange { get; set; }
        /// <summary>
        /// Where the Bullet where created/shot from
        /// </summary>
        public Vector3 StartPos { get; set; }
        /// <summary>
        /// How much damage the bullet does to entities
        /// </summary>
        public float Damage { get; set; }
        /// <summary>
        /// ID of the Owner/shooter Entity
        /// </summary>
        public int Owner { get; set; }
    }
}