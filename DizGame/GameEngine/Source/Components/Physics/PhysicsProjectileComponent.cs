using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    public class PhysicsProjectileComponent : IComponent
    {
        #region Public Configuration
        /// <summary>
        /// The projectiles lifespan
        /// </summary>
        public float TotalTimePassed { get; set; }
        public float Damping { get; set; }
        #endregion Public COnfiguration

        public PhysicsProjectileComponent()
        {
            TotalTimePassed = 0;
            Damping = 0.99f;
        }
    }
}
