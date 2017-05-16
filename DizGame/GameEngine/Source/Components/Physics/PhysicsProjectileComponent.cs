using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{    
    /// <summary>
     /// ProjectileComponent used in the physicSystem
     /// </summary>
    public class PhysicsProjectileComponent : IComponent
    {
        #region Public Configuration
        /// <summary>
        /// The projectiles lifespan
        /// </summary>
        public float TotalTimePassed { get; set; }
        /// <summary>
        /// Coefficient which is used to set the projectiles dampening
        /// </summary>
        public float DampingCoefficient { get; set; }
        #endregion Public Configuration
        /// <summary>
        /// Basic constructor which sets default values
        /// to the attributes
        /// </summary>
        public PhysicsProjectileComponent()
        {
            TotalTimePassed = 0;
            DampingCoefficient = 0.99f;
        }
    }
}
