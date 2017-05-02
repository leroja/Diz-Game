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
        public BoundingSphere BoundingSphere { get; set; }
        public bool IsStatic { get; set; }
        #endregion Public Properties

        public ModelComponent(Model model)
        {
            Model = model;
            
        }
    }
}
