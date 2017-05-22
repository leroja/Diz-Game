using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Components
{
    class SkyBoxComponent : IComponent
    {
        public Model SkyboxModel { get; set; }
        public Effect SkyboxEffect { get; set; }
        public float Size { get; set; } = 295;
        public TextureCube SkyboxTextureCube { get; set; }
    }
}
