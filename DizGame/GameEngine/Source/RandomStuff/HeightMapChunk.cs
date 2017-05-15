﻿using Microsoft.Xna.Framework;
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
        /// <summary>
        /// The vertexBuffer of the Chunk
        /// </summary>
        public VertexBuffer VertexBuffer { get; set; }
        /// <summary>
        /// The IndexBuffer of the chunk
        /// </summary>
        public IndexBuffer IndexBuffer { get; set; }
        /// <summary>
        /// The Basic Effect for the Chunk
        /// </summary>
        public BasicEffect Effect { get; set; }
        /// <summary>
        /// The bounding box that encapsulates the Chunk
        /// </summary>
        public BoundingBox BoundingBox { get; set; }
        /// <summary>
        /// The position offset from the startingpoint of the heightmap
        /// </summary>
        public Vector3 OffsetPosition { get; set; }
        /// <summary>
        /// How many indices there are in the index buffer divided by three
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
