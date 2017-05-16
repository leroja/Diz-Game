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
    public class ParticleEmiterComponent : IComponent
    {
        /// <summary>
        /// Name of particle. used for deciding settings
        /// </summary>
        public string particaleName { get; set; }

        /// <summary>
        /// Number of particles on emiter
        /// </summary>
        public int nParticles { get; set; }

        /// <summary>
        /// Particel lifetime 
        /// </summary>
        public float lifeTime { get; set; }

        /// <summary>
        /// Emiter lifetime
        /// </summary>
        public float EmiterLife { get; set; }
        /// <summary>
        /// Partiecle texture  
        /// </summary>
        public Texture2D texture { get; set; }

        /// <summary>
        /// Seting for Fading textures
        /// </summary>
        public float FadeInTime { get; set; }

        /// <summary>
        /// direktion of Particels from emiter
        /// </summary>
        public Vector3 Direction { get; set; }

        /// <summary>
        /// Used for drawing Emitter Particles
        /// </summary>
        public Effect effect { get; set; }

        /// <summary>
        /// start index for queue 
        /// </summary>
        public int StartIndex { get; set; }
        /// <summary>
        /// Nuber of active particles in queue
        /// </summary>
        public int numberOfActiveParticles { get; set; }
        /// <summary>
        /// Speed of particle
        /// </summary>
        public int speed { get; set; }

        /// <summary>
        /// A place to store Emiterns curetn particles
        /// </summary>
        public ParticleVertex[] particle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int[] indices { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Name of particle</param>
        /// <param name="numberOfParticles">Number of particles on emiter</param>
        /// <param name="lifeTime">Particel lifetime </param>
        /// <param name="texture">Partiecle texture  </param>
        /// <param name="FadeTime">Seting for Fading textures</param>
        /// <param name="Direction">direktion of Particels from emiter</param>
        public ParticleEmiterComponent(string name, int numberOfParticles, float lifeTime, Texture2D texture, float FadeTime, Vector3 Direction)
        {
            this.particaleName = name;
            this.nParticles = numberOfParticles;
            this.lifeTime = lifeTime;
            this.texture = texture;
            this.FadeInTime = FadeTime;
            this.Direction = Direction;


        }
    }
}
