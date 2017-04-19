using GameEngine.Source.Physics.Type.Interface;
using GameEngine.Source.Components;
using GameEngine.Source.Systems.Abstract_classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Physics.Type
{
    public class RigidBody : IPhysicsType
    {
        //        /// Given in the rigid body frame in the unit kilograms per meter^2.
        //        public Matrix InertiaTensor
        //        {
        //            get
        //            {
        //                return bodyFrameInertiaTensor;
        //            }
        //            set
        //            {
        //                bodyFrameInertiaTensor = value;
        //                bodyFrameInvertedInertiaTensor = Matrix.Invert(bodyFrameInertiaTensor);
        //                // Update the world frame inverted inertia tensor
        //                IPositioned position = ComponentManager.Instance.GetPositioned(EntityID);
        //                CalculateWorldFrameInvertedInertiaTensor(position.Orientation);
        //                // Update the angular velocity
        //                CalculateAngularVelocity();
        //            }
        //        }
        //        /// Angular velocity pseudo vector in the body frame in radians per second.
        //        public Vector3 AngularVelocity
        //        {
        //            get
        //            {
        //                return angularVelocity;
        //            }
        //        }
        //    {
        //        /// Angular momentum in the body frame.
        //        public Vector3 AngularMomentum
        //        {
        //            get
        //            {
        //                return angularMomentum;
        //            }
        //            set
        //            {
        //                angularMomentum = value;
        //            }
        //        }
        //        /// Total Moments (Torque) in the body frame in Newton meters.
        //        public Vector3 TotalBodyTorque
        //        {
        //            get; set;
        //        }
        //        /// Transformation matrix from body frame to the local frame.
        //        /// The local frame is aligned with the world frame but with the
        //        /// origin at the body's center of gravity.
        //        public Matrix BodyToLocal
        //        {
        //            get; private set;
        //        }
        //        /// Transformation matrix from local frame to body frame.
        //        /// The local frame is aligned with the world frame but with the
        //        /// origin at the body's center of gravity.
        //        public Matrix LocalToBody
        //        {
        //            get; private set;
        //        }

        public override void Update(PhysicsComponent pc, float dt)
        {
            //            pc.Acceleration = pc.Forces / pc.Mass;
            //            pc.Velocity += pc.Acceleration * dt;

            //            IPositioned position = ComponentManager.Instance.GetPositioned(EntityID);
            //            BodyToLocal = Matrix.CreateFromQuaternion(position.Orientation);
            //            LocalToBody = Matrix.Invert(BodyToLocal);
            //            IntegrateStateVariables(position, dt);
        }
        //        private void IntegrateStateVariables(TransformComponent position, float dt)
        //        {
        //            // Linear position, velocity and acceleration is computed in the Pointmass base class.
        //            // Remains to do here:
        //            // Orientation
        //            Matrix A = Matrix.CreateFromQuaternion(position.Orientation);
        //            A += dt * Tilde(AngularVelocity) * A;
        //            position.Orientation = Quaternion.CreateFromRotationMatrix(A).Normalize();
        //            // Angular Momentum
        //            angularMomentum += dt * TotalBodyTorque;
        //            // Update the world frame inverted inertia tensor
        //            CalculateWorldFrameInvertedInertiaTensor(position.Orientation);
        //            // Update the angular velocity
        //            CalculateAngularVelocity();
        //        }
        //        private void CalculateWorldFrameInvertedInertiaTensor(Quaternion worldFrameOrientation)
        //        {
        //            Matrix A = Matrix.CreateFromQuaternion(worldFrameOrientation);
        //            worldFrameInvertedInertiaTensor = A * bodyFrameInvertedInertiaTensor * Matrix.Transpose(A);
        //        }
        //        private void CalculateAngularVelocity()
        //        {
        //            Vector3.Transform(ref angularMomentum, ref worldFrameInvertedInertiaTensor,
        //            out angularVelocity);
        //        }
        //        private Matrix Tilde(Vector3 v)
        //        {
        //            return new Matrix(0, -v.Z, v.Y, 0,
        //            v.Z, 0, -v.X, 0,
        //            -v.Y, v.X, 0, 0,
        //            0, 0, 0, 1);
        //        }
        //        private Vector3 angularVelocity;
        //        private Matrix angularVelocityTilde;
        //        private Vector3 angularMomentum;
        //        private Matrix bodyFrameInertiaTensor;
        //        private Matrix bodyFrameInvertedInertiaTensor;
        //        private Matrix worldFrameInvertedInertiaTensor;
        //    }
    }
}
