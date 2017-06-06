using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.RandomStuff
{
    /// <summary>
    /// Vertex Declaration for particle vertex needed for rendering and Effect
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
        public Vector3 StartPosition
        {
            get { return vertexPosition; }
            set { vertexPosition = value; }
        }
        /// <summary>
        /// Coordinate, Used in shader for texture
        /// </summary>
        public Vector2 UV
        {
            get { return vertexUV; }
            set { vertexUV = value; }
        }
        /// <summary>
        /// Movement direction
        /// </summary>
        public Vector3 Direction
        {
            get { return vertexDirection; }
            set { vertexDirection = value; }
        }
        /// <summary>
        /// Movement speed
        /// </summary>
        public float Speed
        {
            get { return vertsxSpeed; }
            set { vertsxSpeed = value; }
        }
        /// <summary>
        /// Creation time for particle
        /// </summary>
        public float StartTime
        {
            get { return vertexStartTime; }
            set { vertexStartTime = value; }
        }
        /// <summary>
        /// Vertex Declaration for vertexes 
        /// </summary>
        VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }

        /// <summary>
        /// Declaration of vertex
        /// </summary>
        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0), new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0), new VertexElement(20, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 1), new VertexElement(32, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 2), new VertexElement(36, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 3));

        /// <summary>
        /// Constructor for ParticleVertex
        /// </summary>
        /// <param name="startPosition">Starting Position for Particle</param>
        /// <param name="UV">Coordinate, Used in shader for texture</param>
        /// <param name="Direction">Movement direction</param>
        /// <param name="speed">Movement speed</param>
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
