using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.RandomStuff;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// A component for particles
    /// </summary>
    public class ParticleEmiterComponent : IComponent
    {
        /// <summary>
        /// Name of particle. Used for deciding settings
        /// </summary>
        public string ParticleName { get; set; }

        /// <summary>
        /// Number of particles on emiter
        /// </summary>
        public int NumberOfParticles { get; set; }

        /// <summary>
        /// Particle lifetime 
        /// </summary>
        public float LifeTime { get; set; }

        /// <summary>
        /// Emiter lifetime
        /// </summary>
        public float EmiterLife { get; set; }
        /// <summary>
        /// Particle texture  
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Setting for Fading textures
        /// </summary>
        public float FadeInTime { get; set; }

        /// <summary>
        /// Direkcion of Particles from emiter
        /// </summary>
        public Vector3 Direction { get; set; }

        /// <summary>
        /// Used for drawing Emitter Particles
        /// </summary>
        public Effect Effect { get; set; }

        /// <summary>
        /// start index for queue 
        /// </summary>
        public int StartIndex { get; set; }
        /// <summary>
        /// Nuber of active particles in queue
        /// </summary>
        public int NumberOfActiveParticles { get; set; }
        /// <summary>
        /// Speed of particle
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// A place to store Emiterns current particles
        /// </summary>
        public ParticleVertex[] Particles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int[] Indices { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Name of particle</param>
        /// <param name="numberOfParticles">Number of particles on emiter</param>
        /// <param name="lifeTime"> Particle lifetime </param>
        /// <param name="texture"> Particle texture  </param>
        /// <param name="FadeTime"> Setting for Fading textures </param>
        /// <param name="Direction"> Direction of Particles from emiter </param>
        public ParticleEmiterComponent(string name, int numberOfParticles, float lifeTime, Texture2D texture, float FadeTime, Vector3 Direction)
        {
            this.ParticleName = name;
            this.NumberOfParticles = numberOfParticles;
            this.LifeTime = lifeTime;
            this.Texture = texture;
            this.FadeInTime = FadeTime;
            this.Direction = Direction;
        }
    }
}
