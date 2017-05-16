using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.RandomStuff
{

    /// <summary>
    /// Vertex Declaration for particle vertex nedded for rendering and Effect
    /// </summary>
    public struct ParticleVertex : IVertexType
    {
        Vector3 vertexPosition;
        Vector2 vertexUV;
        Vector3 vertexDirection;
        float vertsxSpeed;
        float vertexStartTime;


        /// <summary>
        /// Starting Position for Particle
        /// </summary>
        public Vector3 startPosition
        {
            get { return vertexPosition; }
            set { vertexPosition = value; }
        }
        /// <summary>
        /// Cordinate, Used in shader for texture
        /// </summary>
        public Vector2 uv
        {
            get { return vertexUV; }
            set { vertexUV = value; }
        }
        /// <summary>
        /// Movment direction
        /// </summary>
        public Vector3 direction
        {
            get { return vertexDirection; }
            set { vertexDirection = value; }
        }
        /// <summary>
        /// Movment speed
        /// </summary>
        public float speed
        {
            get { return vertsxSpeed; }
            set { vertsxSpeed = value; }
        }
        /// <summary>
        /// Creation time for particle
        /// </summary>
        public float startTime
        {
            get { return vertexStartTime; }
            set { vertexStartTime = value; }
        }
        /// <summary>
        /// Vertex Declaration for vertexes 
        /// </summary>
        VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }

        /// <summary>
        /// Decllration of vertex
        /// </summary>
        public readonly static VertexDeclaration VertexDeclaration =  new VertexDeclaration(new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0), new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0), new VertexElement(20, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 1), new VertexElement(32, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 2), new VertexElement(36, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 3));

        /// <summary>
        /// Constructor for ParticleVertex
        /// </summary>
        /// <param name="startPosition">Starting Position for Particle</param>
        /// <param name="UV">Cordinate, Used in shader for texture</param>
        /// <param name="Direction">Movment direction</param>
        /// <param name="speed">Movment speed</param>
        /// <param name="startingTime">Creation time for particle</param>
        public ParticleVertex(Vector3 startPosition, Vector2 UV, Vector3 Direction, float speed, float startingTime)
        {
            this.vertexPosition = startPosition;
            this.vertexUV = UV;
            this.vertexDirection = Direction;
            this.vertsxSpeed = speed;
            this.vertexStartTime = startingTime;
        }

        
        
        
    }
}
