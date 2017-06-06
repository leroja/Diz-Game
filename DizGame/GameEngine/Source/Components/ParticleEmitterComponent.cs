using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Source.RandomStuff;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// A component for particles
    /// </summary>
    public class ParticleEmitterComponent : IComponent
    {
        /// <summary>
        /// Name of particle. Used for deciding settings
        /// </summary>
        public string ParticleName { get; set; }

        /// <summary>
        /// Number of particles on emitter
        /// </summary>
        public int NumberOfParticles { get; set; }

        /// <summary>
        /// Particle lifetime 
        /// </summary>
        public float LifeTime { get; set; }

        /// <summary>
        /// Emitter lifetime
        /// </summary>
        public float EmitterLife { get; set; }
        /// <summary>
        /// Particle texture  
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Setting for Fading textures
        /// </summary>
        public float FadeInTime { get; set; }

        /// <summary>
        /// Direction of Particles from emitter
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
        /// Number of active particles in queue
        /// </summary>
        public int NumberOfActiveParticles { get; set; }
        /// <summary>
        /// Speed of particle
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// A place to store the emitters current particles
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
        /// <param name="numberOfParticles">Number of particles on emitter</param>
        /// <param name="lifeTime"> Particle lifetime </param>
        /// <param name="texture"> Particle texture  </param>
        /// <param name="FadeTime"> Setting for Fading textures </param>
        /// <param name="Direction"> Direction of Particles from emitter </param>
        public ParticleEmitterComponent(string name, int numberOfParticles, float lifeTime, Texture2D texture, float FadeTime, Vector3 Direction)
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