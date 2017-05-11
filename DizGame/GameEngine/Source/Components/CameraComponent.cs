using GameEngine.Source.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public const float DEFAULT_ZFAR = 1000.0f;
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
        public readonly static Vector3 DEFAULT_POV = new Vector3(0, 2, 0);
        /// <summary>
        /// Default static cam offset Vector3(0,0,0)
        /// </summary>
        public readonly static Vector3 DEFAULT_STATIC = Vector3.Zero;
        #endregion CameraOffsets

        #region Public Properties
        /// <summary>
        /// 
        /// </summary>
        public float FieldOfView { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float AspectRatio { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float NearPlane { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float FarPlane { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Matrix View { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Matrix Projection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Vector3 LookAt { get; set; }
        /// <summary>
        /// Set which type of offset that wants to be used
        /// </summary>
        public Vector3 Offset { get; set; }
        /// <summary>
        /// Is an BoundingFrustrum which is View * Projection
        /// </summary>
        public BoundingFrustum CameraFrustrum { get; set; }
        /// <summary>
        /// Set which type of camera is used eg.(Pov, static, chase)
        /// </summary>
        public CameraType CameraType { get; set; }

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
            this.CameraType = type;

            LookAt = Vector3.Zero;
            if (type == CameraType.Chase)
                Offset = DEFAULT_CHASE;
            else if (type == CameraType.Pov)
                Offset = DEFAULT_POV;
            else
            {
                Offset = DEFAULT_STATIC;
            }
            CameraFrustrum = new BoundingFrustum(View * Projection);
        }
    }
}
