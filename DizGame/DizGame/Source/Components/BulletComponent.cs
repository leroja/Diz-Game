using GameEngine.Source.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Components
{
    public class BulletComponent : IComponent
    {
        /// <summary>
        /// How far the bullet will reach
        /// </summary>
        public float MaxRange { get; set; }
        /// <summary>
        /// Where the Bullet where created/shooted from
        /// </summary>
        public Vector3 StartPos { get; set; }
        /// <summary>
        /// How much damage the bullet does to enties
        /// </summary>
        public float Damage { get; set; }
        /// <summary>
        /// ID of the Owner/shooter Entity
        /// </summary>
        public int Owner { get; set; }
    }
}
