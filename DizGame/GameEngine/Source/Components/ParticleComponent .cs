using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    // inte nära klar
    //TODO: Göra klart
    public class ParticleComponent : IComponent
    {
        public Texture2D Texture { get; set; }
        public Vector3 Position { get; set; }
        public float Timecreated { get; set; }
        public float Lifetime { get; set; }
        public int MaximumParticles { get; set; }
        public Vector3 Acceleration { get; set; }
        public Vector3 ParticleGravity { get; set; }

    }
}
