using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Enums
{
    public static class MaterialType
    {
        #region Public Constants NonSlippery
        public static readonly float[] Steel_Steel = { 0.5f, 0.8f };
        public static readonly float[] Steel_Wood = { 0.5f, 0.6f };
        public static readonly float[] Wood_Wood = { 0.4f, 0.6f };
        public static readonly float[] Rubber_Metal = { 1f };
        public static readonly float[] Plastic_Metal = { 0.25f, 0.4f };
        public static readonly float[] Plastic_Plastic = { 0.3f, 0.4f };
        public static readonly float[] CarTire_Asphalt = { 0.72f };
        public static readonly float[] CarTire_Grass = { 0.35f };
        public static readonly float[] Ice_Ice = { 0.02f, 0.09f };
        public static readonly float[] Ice_Wood = { 0.05f };
        public static readonly float[] Ice_Steel = { 0.03f };
        public static readonly float[] Leather_Metal = { 0.6f };
        public static readonly float[] Leather_Wood = { 0.3f, 0.4f };
        public static readonly float[] Nylon_Nylon = { 0.15f, 0.25f };
        public static readonly float[] Iron_Iron = { 1.0f };
        public static readonly float[] Rubber_Rubber = { 1.16f };
        public static readonly float[] Rubber_Cardboard = { 0.5f, 0.8f };
        public static readonly float[] Rubber_DryAsphalt = { 0.5f, 0.8f };
        public static readonly float[] Rubber_WetAsphalt = { 0.25f, 0.75f };
        public static readonly float[] Rubber_DryConcrete = { 0.6f, 0.85f };
        public static readonly float[] Rubber_WetConcrete = { 0.45f, 0.75f };
        public static readonly float[] Skin_Metal = { 0.8f, 1.0f };
        public static readonly float[] Wood_Stone = { 0.2f, 0.4f };
        public static readonly float[] Wood_Concrete = { 0.62f };
        public static readonly float[] Wood_Brick = { 0.6f };
        #endregion Public Constants NonSlippery

        #region Public Constants Slippery
        public static readonly float[] Slippery_Steel_Steel = { 0.16f };
        public static readonly float[] Slippery_Steel_Wood = { 0.1f };
        public static readonly float[] Slippery_Wood_Wood = { 0.15f, 0.2f };
        public static readonly float[] Slippery_Leather_Metal = { 0.2f };
        public static readonly float[] Slippery_Iron_Iron = { 0.15f, 0.2f };
        public static readonly float[] Slippery_Wood_WetSnow = { 0.14f};
        public static readonly float[] Wood_DrySnow = { 0.04f };
        #endregion Public Constants Slippery
    }
}
