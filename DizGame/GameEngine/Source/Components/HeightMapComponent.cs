using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Components
{
    class HeightMapComponent : IComponent
    {


        public int TerrainWidth { get; set; }
        public int TerrainHeight { get; set; }

        public VertexPositionNormalTexture[] Vertices { get; set; }
        public VertexBuffer VertexBuffer { get; set; }

        public IndexBuffer IndexBuffer { get; set; }
        public int[] Indices { get; set; }

    }
}