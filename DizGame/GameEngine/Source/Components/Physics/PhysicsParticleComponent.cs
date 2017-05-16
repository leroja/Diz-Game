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
    public class PhysicsParticleComponent : IComponent
    {
        #region Public Configuration
        public float DampingCoefficient { get; set; }
        public float Age { get; set; }
        public bool IsAlive { get; set; }
        public Texture2D Tetxure { get; set; }
        public float LifeTime { get; set; }
        #endregion Public Configuration
    }
}
