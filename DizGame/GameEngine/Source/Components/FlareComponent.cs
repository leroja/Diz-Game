using GameEngine.Source.RandomStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// Component to represents flares
    /// </summary>
    public class FlareComponent : IComponent
    {
        #region Constants
        /// <summary>
        /// How big is the circular glow effect
        /// </summary>
        public const float GlowSize = 400;
        /// <summary>
        /// How big a rectangle should we examine when issuing our occlusion queries?
        /// Increasing this makes the flares fade out more gradually when the sun goes
        /// behind scenery, while smaller query areas cause sudden on/off transitions.
        /// </summary>
        public const float QuerySize = 100;
        #endregion
        #region Fields
        /// <summary>
        /// Lights direction
        /// </summary>
        public Vector3 LightDirection { get; set; }
        /// <summary>
        /// Lights position
        /// </summary>
        public Vector2 LightPosition { get; set; }
        /// <summary>
        /// Light behindcamera
        /// </summary>
        public bool LightBehindCamera { get; set; }
        /// <summary>
        /// Glow texture
        /// </summary>
        public Texture2D GlowSprite { get; set; }
        /// <summary>
        /// Vertices
        /// </summary>
        public VertexPositionColor[] QueryVertices { get; set; }
        /// <summary>
        /// If flare is active
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Custom blend state so the occlusion query polygons do not show up on screen.
        /// </summary>
        public static readonly BlendState ColorWriteDisable = new BlendState
        {
            ColorWriteChannels = ColorWriteChannels.None
        };
        /// <summary>
        /// An occlusion query is used to detect when the sun is hidden behind scenery.
        /// </summary>
        public OcclusionQuery OcclusionQuery { get; set; }
        /// <summary>
        /// if( occlusion is active.
        /// </summary>
        public bool OcclusionQueryActive { get; set; }
        /// <summary>
        /// Occulusion alpha
        /// </summary>
        public float OcclusionAlpha { get; set; }
        /// <summary>
        /// Array of flares
        /// </summary>
        public Flare[] Flares { get; set; }
        #endregion

        /// <summary>
        /// Basic constructor which sets parameters to default values.
        /// </summary>
        public FlareComponent()
        {
            LightDirection = Vector3.Normalize(new Vector3(-1, -0.1f, 0.3f));
            SetupDefaultQueryVertices();
        }

        /// <summary>
        /// Constructor which takes in lightDirection to set it's direction
        /// </summary>
        /// <param name="lightDirection"></param>
        public FlareComponent(Vector3 lightDirection)
        {
            LightDirection = lightDirection;
            SetupDefaultQueryVertices();
        }

        private void SetupDefaultQueryVertices()
        {
            QueryVertices = new VertexPositionColor[4];

            QueryVertices[0].Position = new Vector3(-QuerySize / 2, -QuerySize / 2, -1);
            QueryVertices[1].Position = new Vector3(QuerySize / 2, -QuerySize / 2, -1);
            QueryVertices[2].Position = new Vector3(-QuerySize / 2, QuerySize / 2, -1);
            QueryVertices[3].Position = new Vector3(QuerySize / 2, QuerySize / 2, -1);
        }
    }
}
