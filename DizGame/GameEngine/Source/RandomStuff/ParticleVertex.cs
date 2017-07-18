using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.RandomStuff
{
    /// <summary>
    /// Vertex Declaration for particle vertex needed for rendering and Effect
    /// </summary>
    public struct ParticleVertex : IVertexType
    {
        /// Stores the starting position of the particle.
        public Vector3 Position;
        /// Stores which corner of the particle quad this vertex represents.
        public Vector2 Corner;
        /// Stores the starting velocity of the particle.
        public Vector3 Velocity;
        /// Four random values, used to make each particle look slightly different.
        public Color Random;
        /// The time (in seconds) at which this particle was created.
        public float Time;

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get
            {
                return VertexDeclaration;
            }
        }


        /// Describe the layout of this vertex structure.
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
          new VertexElement(0, VertexElementFormat.Vector3,
                                 VertexElementUsage.Position, 0),
          new VertexElement(12, VertexElementFormat.Vector2,
                                 VertexElementUsage.Normal, 0),
          new VertexElement(20, VertexElementFormat.Vector3,
                                 VertexElementUsage.Normal, 1),
          new VertexElement(32, VertexElementFormat.Color,
                                 VertexElementUsage.Color, 0),
          new VertexElement(36, VertexElementFormat.Single,
                                 VertexElementUsage.TextureCoordinate, 0)
        );

        public const int sizeInBytes = 40;
    }
}
