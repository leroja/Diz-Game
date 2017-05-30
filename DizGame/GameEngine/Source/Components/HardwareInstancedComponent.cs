using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Components
{

    public class HardwareInstancedComponent : IComponent
    {

        public GraphicsDevice GraphicsDevice { get; set; }

        public Effect Effect { get; set; }

        public RasterizerState RasterizerState { get; set; }

        public Texture2D Texture { get; set; }

        public Matrix[] ObjectWorldMatrices { get; set; }

        public int InstanceCount { get; set; }
        
        public VertexBufferBinding[] Bindings { get; set; }

        public VertexBuffer MatriceVB { get; set; }

        public VertexDeclaration MatriceVD { get; set; }

        public VertexBuffer  VertexBuffer { get; set; }

        public IndexBuffer  IndexBuffer { get; set; }

        public int IndicesPerPrimitive { get; set; }
    }
}
