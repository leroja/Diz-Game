using GameEngine.Source.Components;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Components
{
    public class HeightMapComponent : IComponent
    {

        public string TerrainMapName { get; set; }

        public int TerrainWidth { get; set; }
        public int TerrainHeight { get; set; }

        public float ScaleFactor { get; set; }

        public VertexPositionNormalTexture[] Vertices { get; set; }
        public VertexBuffer VertexBuffer { get; set; }

        public IndexBuffer IndexBuffer { get; set; }
        public int[] Indices { get; set; }

        //to be set for other systems to get height data from map.
        public float[,] HeightData { get; set; }

        public HeightMapComponent()
        {

        }

    }
}