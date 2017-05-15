using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.RandomStuff
{
    public class HeightMapChunk
    {
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public BasicEffect Effect { get; set; }
        public BoundingBox BoundingBox { get; set; }
        public BoundingSphere BoundingSphere { get; set; }

        public Vector3 OffsetPosition { get; set; }
        
        public int indicesDiv3;
        public int Width { get; set; }
        public int Height { get; set; }
        public float[,] heightInfo;
        public VertexPositionNormalTexture[] Vertices { get; set; }
        public int[] Indices { get; set; }

        public Rectangle Rectangle { get; set; }
        public Texture2D Texture { get; set; }

        public HeightMapChunk()
        {

        }

    }
}
