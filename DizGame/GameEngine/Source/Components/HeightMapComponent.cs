using GameEngine.Source.Components;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Components
{
    public class HeightMapComponent : IComponent
    {

        public string terrainMapName { get; set; }

        public int terrainWidth { get; set; }
        public int terrainHeight { get; set; }

        public float scaleFactor { get; set; }

        public VertexPositionNormalTexture[] vertices { get; set; }
        public VertexBuffer vertexBuffer { get; set; }

        public IndexBuffer indexBuffer { get; set; }
        public int[] indices { get; set; }

        //to be set for other systems to get height data from map.
        public float[,] heightData { get; set; }

        public HeightMapComponent()
        {

        }

    }
}