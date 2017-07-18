using GameEngine.Source.RandomStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    public class ParticleEmitterComponent : IComponent
    {
        /// <summary>
        /// ParticleType for controlling updating
        /// </summary>
        public string ParticleType { get; set; }
        /// <summary>
        /// LifeTime Of emitter
        /// </summary>
        public float LifeTime { get; set; }

        /// <summary>
        /// the used particle effect for this emitter
        /// </summary>
        public Effect ParticleEffect { get; set; }

        /// <summary>
        /// an array of particles
        /// </summary>
        public ParticleVertex[] Particles { get; set; }

        /// <summary>
        ///  A vertex buffer holding our particles.
        /// </summary>
        public DynamicVertexBuffer VertexBuffer { get; set; }

        /// <summary>
        /// Index buffer turns sets of four vertices into particle quads (pairs of triangles).
        /// </summary>
        public IndexBuffer IndexBuffer { get; set; }

        /// <summary>
        /// First active particle in particles
        /// </summary>
        public int FirstActiveParticle { get; set; }
        /// <summary>
        /// first new particle in particles
        /// </summary>
        public int FirstNewParticle { get; set; }
        /// <summary>
        /// First free particle in particles
        /// </summary>
        public int FirstFreeParticle { get; set; }
        /// <summary>
        /// first retired particle
        /// </summary>
        public int FirstRetiredParticle { get; set; }
        /// <summary>
        ///  Store the current time, in seconds.
        /// </summary>
        public float CurrentTime { get; set; }


        /// <summary>
        /// Counts how many times draw has been called. used for updating
        /// </summary>
        public int DrawCounter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="maxParticles"> Max number of particles </param>
        public ParticleEmitterComponent(GraphicsDevice device, int maxParticles)
        {
            Particles = new ParticleVertex[maxParticles * 4];

            for (int i = 0; i < maxParticles; i++)
            {
                Particles[i * 4 + 0].Corner = new Vector2(-1, -1);
                Particles[i * 4 + 1].Corner = new Vector2(1, -1);
                Particles[i * 4 + 2].Corner = new Vector2(1, 1);
                Particles[i * 4 + 3].Corner = new Vector2(-1, 1);
            }
            VertexBuffer = new DynamicVertexBuffer(device, ParticleVertex.VertexDeclaration,
                                                  maxParticles * 4, BufferUsage.WriteOnly);

            ushort[] indices = new ushort[maxParticles * 6];

            for (int i = 0; i < maxParticles; i++)
            {
                indices[i * 6 + 0] = (ushort)(i * 4 + 0);
                indices[i * 6 + 1] = (ushort)(i * 4 + 1);
                indices[i * 6 + 2] = (ushort)(i * 4 + 2);

                indices[i * 6 + 3] = (ushort)(i * 4 + 0);
                indices[i * 6 + 4] = (ushort)(i * 4 + 2);
                indices[i * 6 + 5] = (ushort)(i * 4 + 3);
            }

            IndexBuffer = new IndexBuffer(device, typeof(ushort), indices.Length, BufferUsage.WriteOnly);

            IndexBuffer.SetData(indices);

        }
    }
}