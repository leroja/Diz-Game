using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    public class PhysicsParticleComponent : IComponent
    {
        #region Public Configuration
        public float DampingCoefficient { get; set; }
        public float Age { get; set; }
        public bool IsAlive { get; set; }
        #endregion Public Configuration
    }
}
