using Microsoft.Xna.Framework;

namespace GameEngine.Source.Random_Stuff
{
    // TODO refine fog effect parameters
    /// <summary>
    /// 
    /// </summary>
    public static class GlobalFogSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public static float FogStart { get; } = 10;
        /// <summary>
        /// 
        /// </summary>
        public static float FogEnd { get; } = 410;
        /// <summary>
        /// 
        /// </summary>
        public static bool FogEnabled { get; } = true;
        /// <summary>
        /// 
        /// </summary>
        public static Vector3 FogColour { get; } = Color.LightGray.ToVector3();
    }
}
