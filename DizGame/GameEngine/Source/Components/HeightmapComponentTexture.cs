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
    public class HeightmapComponentTexture : IComponent
    {
        public BasicEffect Effect { get; set; }
        public int[] Indices { get; set; }
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }


        public float[,] HeightMapData { get; set; }
        public List<HeightMapChunk> HeightMapChunks { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public HeightmapComponentTexture()
        {
            HeightMapChunks = new List<HeightMapChunk>();
        }
    }
}
