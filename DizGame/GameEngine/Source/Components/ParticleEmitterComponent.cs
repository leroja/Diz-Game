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
        /// ParticleType for controlling uppdating
        /// </summary>
        public string ParticleType { get; set; } 
        /// <summary>
        /// LifeTime Of emiter
        /// </summary>
        public float LifeTime { get; set; }

        /// <summary>
        /// the used partical effect for this emiter
        /// </summary>
        public Effect particleEffect { get; set; }

        /// <summary>
        /// an array of particles
        /// </summary>
        public ParticleVertex[] particles { get; set; }

        /// <summary>
        ///  A vertex buffer holding our particles.
        /// </summary>
        public DynamicVertexBuffer vertexBuffer { get; set; }

        /// <summary>
        /// Index buffer turns sets of four vertices into particle quads (pairs of triangles).
        /// </summary>
        public IndexBuffer indexBuffer { get; set; }

        /// <summary>
        /// First active particle in particles
        /// </summary>
        public int firstActiveParticle { get; set; }
        /// <summary>
        /// first new particle in particles
        /// </summary>
        public int firstNewParticle { get; set; }
        /// <summary>
        /// First free particle in particles
        /// </summary>
        public int firstFreeParticle { get; set; }
        /// <summary>
        /// first retired particle
        /// </summary>
        public int firstRetiredParticle { get; set; }
        /// <summary>
        ///  Store the current time, in seconds.
        /// </summary>
        public float currentTime { get; set; }


        /// <summary>
        /// Counts howe many times draw has been caled. used for uppdating
        /// </summary>
        public int drawCounter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="maxParticles">Max number of particels</param>
        public ParticleEmitterComponent(GraphicsDevice device,int maxParticles)
        {
            particles = new ParticleVertex[maxParticles * 4];

            for (int i = 0; i < maxParticles; i++)
            {
                particles[i * 4 + 0].Corner = new Vector2(-1, -1);
                particles[i * 4 + 1].Corner = new Vector2(1, -1);
                particles[i * 4 + 2].Corner = new Vector2(1, 1);
                particles[i * 4 + 3].Corner = new Vector2(-1, 1);
            }
            vertexBuffer = new DynamicVertexBuffer(device, ParticleVertex.VertexDeclaration,
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

            indexBuffer = new IndexBuffer(device, typeof(ushort), indices.Length, BufferUsage.WriteOnly);

            indexBuffer.SetData(indices);

        }
    }
}
