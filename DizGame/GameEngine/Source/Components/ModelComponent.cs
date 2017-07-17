using AnimationContentClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// Component that stores an model.
    /// </summary>
    public class ModelComponent : IComponent
    {
        #region Public Properties
        /// <summary>
        /// The model
        /// </summary>
        public Model Model { get; set; }
        /// <summary>
        /// Used to store the models bone transforms.
        /// </summary>
        public Matrix[] MeshWorldMatrices { get; set; }
        /// <summary>
        /// The bounding sphere the encapsulates the model
        /// </summary>
        public BoundingVolume BoundingVolume { get; set; }
        /// <summary>
        /// A Bool that says whether the model is static or not
        /// </summary>
        public bool IsStatic { get; set; }
        /// <summary>
        /// A Bool that says whether the model is visible or not
        /// </summary>
        public bool IsVisible { get; set; }

        #endregion Public Properties

        /// <summary>
        /// Constructor which takes model as parameter
        /// </summary>
        /// <param name="model"></param>
        public ModelComponent(Model model)
        {
            Model = model;
            IsVisible = true;
        }
    }
}