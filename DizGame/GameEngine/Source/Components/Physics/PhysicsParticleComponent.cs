using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// ParticleComponent used in the physicSystem
    /// </summary>
    public class PhysicsParticleComponent : IComponent
    {
        #region Public Configuration
        /// <summary>
        /// Coefficient which is used to set the particles dampening
        /// </summary>
        public float DampingCoefficient { get; set; }
        /// <summary>
        /// The particles age set in time
        /// </summary>
        public float Age { get; set; }
        /// <summary>
        /// If the particle is alive
        /// </summary>
        public bool IsAlive { get; set; }
        public Texture2D Tetxure { get; set; }
        public float LifeTime { get; set; }
        #endregion Public Configuration
        /// <summary>
        /// Basic constructor which sets default values
        /// to the attributes
        /// </summary>
        public PhysicsParticleComponent()
        {
            DampingCoefficient = 0.99f;
            Age = 0f;
            IsAlive = true;
        }
    }
}
