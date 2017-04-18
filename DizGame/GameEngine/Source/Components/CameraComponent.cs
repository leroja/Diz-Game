using GameEngine.Source.Components.Interface;
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
        public const float DEFAULT_ARATION = 1.33f;
        #endregion Public Constants

        #region Public Properties
        public float Fov { get; set; }
        public float AspectRatio { get; set; }
        public float NearPlane { get; set; }
        public float FarPlane { get; set; }
        
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        public Vector3 LookAt { get; set; }

       

        #endregion Public Propterties
    }
}
