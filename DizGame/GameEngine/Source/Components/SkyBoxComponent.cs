using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Components
{
    public class SkyBoxComponent : IComponent
    {
        public Model SkyboxModel { get; set; }
        public Effect SkyboxEffect { get; set; }
        public float Size { get; set; } = 295;
        public Texture2D SkyboxTextureCube { get; set; }
        public SkyBoxComponent(Model skyboxModel)
        {
            SkyboxModel = skyboxModel;
            //SkyboxTextureCube = texture;
            //this.SkyboxEffect = SkyboxEffect;
        }
    }
}
