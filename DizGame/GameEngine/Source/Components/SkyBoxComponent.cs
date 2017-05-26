﻿using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// A component for the skybox
    /// </summary>
    public class SkyBoxComponent : IComponent
    {
        /// <summary>
        /// 
        /// </summary>
        public Model SkyboxModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Effect SkyboxEffect { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float Size { get; set; } = 295;
        /// <summary>
        /// 
        /// </summary>
        public Texture2D SkyboxTextureCube { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skyboxModel"></param>
        public SkyBoxComponent(Model skyboxModel)
        {
            SkyboxModel = skyboxModel;
            //SkyboxTextureCube = texture;
            //this.SkyboxEffect = SkyboxEffect;
        }
    }
}