
namespace GameEngine.Source.Enums
{
    /// <summary>
    /// Enum for different types of material
    /// Which includes classes for the materials characteristics
    /// </summary>
    public enum MaterialType
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// Skin
        /// </summary>
        Skin,
        /// <summary>
        /// Steel
        /// </summary>
        Steel,
        /// <summary>
        /// Wood
        /// </summary>
        Wood,
        /// <summary>
        /// Rubber
        /// </summary>
        Rubber,
        /// <summary>
        /// Plastic
        /// </summary>
        Plastic,
        /// <summary>
        /// Car tire
        /// </summary>
        CarTire,
        /// <summary>
        /// Ice
        /// </summary>
        Ice,
        /// <summary>
        /// Leather
        /// </summary>
        Leather,
        /// <summary>
        /// Nylon
        /// </summary>
        Nylon,
        /// <summary>
        /// Stone
        /// </summary>
        Stone,
        /// <summary>
        /// Metal
        /// </summary>
        Metal,
        /// <summary>
        /// Concrete
        /// </summary>
        Concrete,
        /// <summary>
        /// Brick
        /// </summary>
        Brick,
        /// <summary>
        /// Wetsnow
        /// </summary>
        WetSnow,
        /// <summary>
        /// Drysnow
        /// </summary>
        DrySnow
    }

    /// <summary>
    /// Static class that returns friction coefficents ratio or singular between different materials.
    /// </summary>
    public static class MaterialFriction
    {
        #region Friction
        #region Public Constants NonSlippery
        /// <summary>
        /// Returns 0.5f - 0.8f
        /// </summary>
        public static readonly float[] Friction_Steel_Steel = { 0.5f, 0.8f };
        /// <summary>
        /// Returns 0.5f - 0.6f
        /// </summary>
        public static readonly float[] Friction_Steel_Wood = { 0.5f, 0.6f };
        /// <summary>
        ///  Returns 0.4f - 0.6f 
        /// </summary>
        public static readonly float[] Friction_Wood_Wood = { 0.4f, 0.6f };
        /// <summary>
        ///  Returns 1.0f
        /// </summary>
        public static readonly float[] Friction_Rubber_Metal = { 1f };
        /// <summary>
        ///  Returns 0.25f - 0.4f
        /// </summary>
        public static readonly float[] Friction_Plastic_Metal = { 0.25f, 0.4f };
        /// <summary>
        ///  Returns 0.3 - 0.4
        /// </summary>
        public static readonly float[] Friction_Plastic_Plastic = { 0.3f, 0.4f };
        /// <summary>
        ///  Returns 0.72f
        /// </summary>
        public static readonly float[] Friction_CarTire_Asphalt = { 0.72f };
        /// <summary>
        ///  Returns 0.35f
        /// </summary>
        public static readonly float[] Friction_CarTire_Grass = { 0.35f };
        /// <summary>
        ///  Returns 0.02f - 0.09f
        /// </summary>
        public static readonly float[] Friction_Ice_Ice = { 0.02f, 0.09f };
        /// <summary>
        ///  Returns 0.05f
        /// </summary>
        public static readonly float[] Friction_Ice_Wood = { 0.05f };
        /// <summary>
        ///  Returns 0.03f
        /// </summary>
        public static readonly float[] Friction_Ice_Steel = { 0.03f };
        /// <summary>
        ///  Returns 0.6f
        /// </summary>
        public static readonly float[] Friction_Leather_Metal = { 0.6f };
        /// <summary>
        ///  Returns 0.3f - 0.4f
        /// </summary>
        public static readonly float[] Friction_Leather_Wood = { 0.3f, 0.4f };
        /// <summary>
        ///  Returns 0.15f - 0.25f
        /// </summary>
        public static readonly float[] Friction_Nylon_Nylon = { 0.15f, 0.25f };
        /// <summary>
        ///  Returns 1.0f
        /// </summary>
        public static readonly float[] Friction_Iron_Iron = { 1.0f };
        /// <summary>
        ///  Returns 1.16f
        /// </summary>
        public static readonly float[] Friction_Rubber_Rubber = { 1.16f };
        /// <summary>
        ///  Returns 0.5f - 0.8f
        /// </summary>
        public static readonly float[] Friction_Rubber_Cardboard = { 0.5f, 0.8f };
        /// <summary>
        ///  Returns 0.5f - 0.8f
        /// </summary>
        public static readonly float[] Friction_Rubber_DryAsphalt = { 0.5f, 0.8f };
        /// <summary>
        ///  Returns 0.25f - 0.75f
        /// </summary>
        public static readonly float[] Friction_Rubber_WetAsphalt = { 0.25f, 0.75f };
        /// <summary>
        ///  Returns 0.6f - 0.85f
        /// </summary>
        public static readonly float[] Friction_Rubber_DryConcrete = { 0.6f, 0.85f };
        /// <summary>
        ///  Returns 0.45f - 0.75f
        /// </summary>
        public static readonly float[] Friction_Rubber_WetConcrete = { 0.45f, 0.75f };
        /// <summary>
        ///  Returns 0.8f - 1.0f
        /// </summary>
        public static readonly float[] Friction_Skin_Metal = { 0.8f, 1.0f };
        /// <summary>
        ///  Returns 0.2f - 0.4f
        /// </summary>
        public static readonly float[] Friction_Wood_Stone = { 0.2f, 0.4f };
        /// <summary>
        ///  Returns 0.62f
        /// </summary>
        public static readonly float[] Friction_Wood_Concrete = { 0.62f };
        /// <summary>
        ///  Returns 0.6f 
        /// </summary>
        public static readonly float[] Friction_Wood_Brick = { 0.6f };
        #endregion Public Constants NonSlippery

        #region Public Constants Slippery
        /// <summary>
        ///  Returns 0.16f
        /// </summary>
        public static readonly float[] Friction_Slippery_Steel_Steel = { 0.16f };
        /// <summary>
        ///  Returns 0.1f
        /// </summary>
        public static readonly float[] Friction_Slippery_Steel_Wood = { 0.1f };
        /// <summary>
        ///  Returns 0.15f - 0.2f
        /// </summary>
        public static readonly float[] Friction_Slippery_Wood_Wood = { 0.15f, 0.2f };
        /// <summary>
        ///  Returns 0.2f
        /// </summary>
        public static readonly float[] Friction_Slippery_Leather_Metal = { 0.2f };
        /// <summary>
        ///  Returns 0.15f - 0.2f
        /// </summary>
        public static readonly float[] Friction_Slippery_Iron_Iron = { 0.15f, 0.2f };
        /// <summary>
        ///  Returns 0.14f
        /// </summary>
        public static readonly float[] Friction_Slippery_Wood_WetSnow = { 0.14f };
        /// <summary>
        ///  Returns 0.04f
        /// </summary>
        public static readonly float[] Friction_Wood_DrySnow = { 0.04f };
        #endregion Public Constants Slippery
        #endregion Friction
    }
}
