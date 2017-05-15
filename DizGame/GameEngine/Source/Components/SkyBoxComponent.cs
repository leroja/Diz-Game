using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
