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
    /// holds the data of a Chunk of a heightmap
    /// </summary>
    public class HeightMapChunk
    {
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public BasicEffect Effect { get; set; }
        public BoundingBox BoundingBox { get; set; }
        public Vector3 OffsetPosition { get; set; }
        /// <summary>
        /// hOw many indices there are in the index buffer divided by three
        /// </summary>
        public int IndicesDiv3 { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HeightMapChunk()
        {

        }

    }
}
