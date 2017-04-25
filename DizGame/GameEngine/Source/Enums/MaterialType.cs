using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Enums
{
    /// <summary>
    /// Enum for different types of material
    /// Which includes classes for the materials characteristics
    /// </summary>
    public enum MaterialType
    {
        None,
        Skin,
        Steel,
        Wood,
        Rubber,
        Plastic,
        CarTire,
        Ice,
        Leather,
        Nylon,
        Stone,
        Metal,
        Concrete,
        Brick,
        WetSnow,
        DrySnow
    }
    public static class MaterialFriction {
        /// <summary>
        /// Class for different types of materials types of Coefficients
        /// in format as float either as an ratio or constant. 
        /// Using float[]
        /// </summary>
        #region Friction
        #region Public Constants NonSlippery
        public static readonly float[] Friction_Steel_Steel = { 0.5f, 0.8f };
        public static readonly float[] Friction_Steel_Wood = { 0.5f, 0.6f };
        public static readonly float[] Friction_Wood_Wood = { 0.4f, 0.6f };
        public static readonly float[] Friction_Rubber_Metal = { 1f };
        public static readonly float[] Friction_Plastic_Metal = { 0.25f, 0.4f };
        public static readonly float[] Friction_Plastic_Plastic = { 0.3f, 0.4f };
        public static readonly float[] Friction_CarTire_Asphalt = { 0.72f };
        public static readonly float[] Friction_CarTire_Grass = { 0.35f };
        public static readonly float[] Friction_Ice_Ice = { 0.02f, 0.09f };
        public static readonly float[] Friction_Ice_Wood = { 0.05f };
        public static readonly float[] Friction_Ice_Steel = { 0.03f };
        public static readonly float[] Friction_Leather_Metal = { 0.6f };
        public static readonly float[] Friction_Leather_Wood = { 0.3f, 0.4f };
        public static readonly float[] Friction_Nylon_Nylon = { 0.15f, 0.25f };
        public static readonly float[] Friction_Iron_Iron = { 1.0f };
        public static readonly float[] Friction_Rubber_Rubber = { 1.16f };
        public static readonly float[] Friction_Rubber_Cardboard = { 0.5f, 0.8f };
        public static readonly float[] Friction_Rubber_DryAsphalt = { 0.5f, 0.8f };
        public static readonly float[] Friction_Rubber_WetAsphalt = { 0.25f, 0.75f };
        public static readonly float[] Friction_Rubber_DryConcrete = { 0.6f, 0.85f };
        public static readonly float[] Friction_Rubber_WetConcrete = { 0.45f, 0.75f };
        public static readonly float[] Friction_Skin_Metal = { 0.8f, 1.0f };
        public static readonly float[] Friction_Wood_Stone = { 0.2f, 0.4f };
        public static readonly float[] Friction_Wood_Concrete = { 0.62f };
        public static readonly float[] Friction_Wood_Brick = { 0.6f };
        #endregion Public Constants NonSlippery

        #region Public Constants Slippery
        public static readonly float[] Friction_Slippery_Steel_Steel = { 0.16f };
        public static readonly float[] Friction_Slippery_Steel_Wood = { 0.1f };
        public static readonly float[] Friction_Slippery_Wood_Wood = { 0.15f, 0.2f };
        public static readonly float[] Friction_Slippery_Leather_Metal = { 0.2f };
        public static readonly float[] Friction_Slippery_Iron_Iron = { 0.15f, 0.2f };
        public static readonly float[] Friction_Slippery_Wood_WetSnow = { 0.14f };
        public static readonly float[] Friction_Wood_DrySnow = { 0.04f };
        #endregion Public Constants Slippery
        #endregion Friction
    }
}
