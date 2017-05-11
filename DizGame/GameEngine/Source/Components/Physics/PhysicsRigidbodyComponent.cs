using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// RigidbodyComponent used in the physicSystem
    /// </summary>
    public class PhysicsRigidbodyComponent : IComponent
    {
        #region Public Configuration
        /// <summary>
        /// Angular velocity pseudo vector in the body frame in radians per second.
        /// </summary>
        public Vector3 AngularVelocity { get; set; }
        /// <summary>
        /// Angular momentum in the body frame.
        /// </summary>
        public Vector3 AngularMomentum { get; set; }
        /// <summary>
        /// Total Moments (Torque) in the body frame in Newton meters.
        /// </summary>
        public Vector3 TotalBodyTorque { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Matrix AngularVelocityTilde { get; set; }
        /// <summary>
        /// Transformation matrix from body frame to the local frame.
        /// The local frame is aligned with the world frame but with the
        /// origin at the body's center of gravity.
        /// </summary>
        public Matrix BodyToLocal { get; set; }
        /// <summary>
        /// Transformation matrix from local frame to body frame.
        /// The local frame is aligned with the world frame but with the
        /// origin at the body's center of gravity.
        /// </summary>
        public Matrix LocalToBody { get; set; }
        /// <summary>
        /// Given in the rigid body frame in the unit kilograms per meter^2.
        /// </summary>
        public Matrix BodyFrameInertiaTensor { get; set; }
        public Matrix BodyFrameInvertedInertiaTensor { get; set; }
        public Matrix WorldFrameInvertedInertiaTensor { get; set; }
        public bool Frozen { get; set; }
        public bool Moved { get; set; }
        #endregion Public Configuration

        /// <summary>
        /// Basic constructor which sets default values
        /// to the attributes
        /// </summary>
        public PhysicsRigidbodyComponent()
        {
            Frozen = false;
            Moved = false;

            AngularVelocity = Vector3.Zero;
            AngularMomentum = Vector3.Zero;
            TotalBodyTorque = Vector3.Zero;

            BodyToLocal = Matrix.Identity;
            LocalToBody = Matrix.Identity;
            AngularVelocityTilde = Matrix.Identity;
            BodyFrameInertiaTensor = Matrix.Identity;
            BodyFrameInvertedInertiaTensor = Matrix.Identity;
            WorldFrameInvertedInertiaTensor = Matrix.Identity;
        }
    }
}
