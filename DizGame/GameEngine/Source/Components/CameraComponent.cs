using GameEngine.Source.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    public class CameraComponent : IComponent
    {
        #region Public Constants
        public const float DEFAULT_FOV = MathHelper.PiOver4;
        public const float DEFAULT_ZFAR = 1000.0f;
        public const float DEFAULT_ZNEAR = 1.0f;
        public const float DEFAULT_ARATIO = 1.33f;
        #endregion Public Constants

        #region CameraOffsets
        public readonly static Vector3 DEFAULT_CHASE = new Vector3(0, 4, 8);
        public readonly static Vector3 DEFAULT_POV = new Vector3(0, 2, 0);
        public readonly static Vector3 DEFAULT_STATIC = Vector3.Zero;
        #endregion CameraOffsets

        #region Public Properties
        public float FieldOfView { get; set; }
        public float AspectRatio { get; set; }
        public float NearPlane { get; set; }
        public float FarPlane { get; set; }
        
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        public Vector3 LookAt { get; set; }
        public Vector3 Offset { get; set; }

        public BoundingFrustum CameraFrustrum { get; set; }

       public CameraType CameraType { get; set; }

        #endregion Public Propterties
        public CameraComponent()
        {
            FieldOfView = DEFAULT_FOV;
            AspectRatio = DEFAULT_ARATIO;
            NearPlane = DEFAULT_ZNEAR;
            FarPlane = DEFAULT_ZFAR;

            View = Matrix.Identity;
            Projection = Matrix.Identity;

            LookAt = Vector3.Zero;
            Offset = DEFAULT_STATIC;
        }
    }
}
