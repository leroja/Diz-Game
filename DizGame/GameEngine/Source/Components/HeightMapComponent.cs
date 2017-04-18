using GameEngine.Source.Components.Interface;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Components
{
    class HeightMapComponent : IComponent
    {


        public int terrainWidth { get; set; }
        public int terrainHeight { get; set; }

        public VertexPositionNormalTexture[] vertices { get; set; }
        public VertexBuffer vertexBuffer { get; set; }

        public IndexBuffer indexBuffer { get; set; }
        public int[] indices { get; set; }

    }
}