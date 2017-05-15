using AnimationContentClasses;
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
    public class ModelComponent : IComponent
    {
        #region Public Properties
        public Model Model { get; set; }
        public Matrix[] MeshWorldMatrices { get; set; }
        public BoundingVolume BoundingVolume { get; set; }
        public bool IsStatic { get; set; }
        #endregion Public Properties

        public ModelComponent(Model model, BoundingVolume volume)
        {
            Model = model;
            BoundingVolume = volume;
        }
    }
}
