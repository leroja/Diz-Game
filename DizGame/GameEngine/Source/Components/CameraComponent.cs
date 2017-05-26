using AnimationContentClasses;
using GameEngine.Source.Enums;
using Microsoft.Xna.Framework;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// Camera component used to define cameras
    /// </summary>
    public class CameraComponent : IComponent
    {
        #region Public Constants
        /// <summary>
        /// Default field of view (PiOver4  which is pi/4)
        /// </summary>
        public const float DEFAULT_FOV = MathHelper.PiOver4;
        /// <summary>
        /// Default far plane (1000.0f)
        /// </summary>
        public const float DEFAULT_ZFAR = 1500.0f;
        /// <summary>
        /// Default near plane (1.0f)
        /// </summary>
        public const float DEFAULT_ZNEAR = 1.0f;
        /// <summary>
        /// Default aspect ratio (1.33f)
        /// </summary>
        public const float DEFAULT_ARATIO = 1.33f;
        #endregion Public Constants

        #region CameraOffsets
        /// <summary>
        /// Default chase cam offset Vector3(0,25,20)
        /// </summary>
        public readonly static Vector3 DEFAULT_CHASE = new Vector3(0, 25, 20);
        /// <summary>
        /// Default personal view offset Vector3(0,2,0)
        /// </summary>
        public readonly static Vector3 DEFAULT_POV = new Vector3(0, 7, 0);
        /// <summary>
        /// Default static cam offset Vector3(0,0,0)
        /// </summary>
        public readonly static Vector3 DEFAULT_STATIC = Vector3.Zero;
        #endregion CameraOffsets

        #region Public Properties
        /// <summary>
        /// Field if view
        /// </summary>
        public float FieldOfView { get; set; }
        /// <summary>
        /// Aspect ratio (Screen Height / Screen Width)
        /// </summary>
        public float AspectRatio { get; set; }
        /// <summary>
        /// Near plane
        /// </summary>
        public float NearPlane { get; set; }
        /// <summary>
        /// Far plane
        /// </summary>
        public float FarPlane { get; set; }
        /// <summary>
        /// Camera view
        /// </summary>
        public Matrix View { get; set; }
        /// <summary>
        /// Camera Projection
        /// </summary>
        public Matrix Projection { get; set; }
        /// <summary>
        /// What the camera looks at
        /// </summary>
        public Vector3 LookAt { get; set; }
        /// <summary>
        /// Set which type of offset that wants to be used
        /// </summary>
        public Vector3 Offset { get; set; }
        /// <summary>
        /// Is an BoundingFrustrum which is View * Projection
        /// </summary>
        public BoundingFrustum3D CameraFrustrum { get; set; }
        /// <summary>
        /// Set which type of camera is used eg.(Pov, static, chase)
        /// </summary>
        public CameraType CameraType { get; set; }
        /// <summary>
        /// Sets if camera is flareable eg. reflect the light flares.
        /// </summary>
        public bool IsFlareable { get; set; }
        #endregion Public Propterties

        /// <summary>
        /// Basic constructor which sets default values
        /// to the attributes
        /// </summary>
        public CameraComponent(CameraType type)
        {
            FieldOfView = DEFAULT_FOV;
            AspectRatio = DEFAULT_ARATIO;
            NearPlane = DEFAULT_ZNEAR;
            FarPlane = DEFAULT_ZFAR;

            View = Matrix.Identity;
            Projection = Matrix.Identity;
            CameraType = type;

            LookAt = Vector3.Zero;
            if (type == CameraType.Chase)
                Offset = DEFAULT_CHASE;
            else if (type == CameraType.Pov)
                Offset = DEFAULT_POV;
            else
            {
                Offset = DEFAULT_STATIC;
            }
            CameraFrustrum = new BoundingFrustum3D(new BoundingFrustum(View * Projection));

            IsFlareable = false;
        }
    }
}
