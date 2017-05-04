﻿using GameEngine.Source.Systems.Interface;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Managers;
using System;
using GameEngine.Source.Enums;

namespace GameEngine.Source.Systems
{
    public class PhysicsRigidBodySystem : IPhysicsTypeSystem
    {
        public PhysicsRigidBodySystem(IPhysics physicsSystem) : base(physicsSystem)
        {
            PhysicsType = PhysicsType.Rigid;
        }
        public override void Update(PhysicsComponent physic, float dt)
        {
            PhysicsSystem.UpdateAcceleration(physic);
            PhysicsSystem.UpdateMass(physic);
            PhysicsSystem.UpdateGravity(physic, dt);
            PhysicsSystem.UpdateForce(physic);
            PhysicsSystem.UpdateVelocity(physic, dt);

            // Creates and send this throught instead of creating globas that takes memory
            PhysicsRigidbodyComponent rigidBody = ComponentManager.GetEntityComponent<PhysicsRigidbodyComponent>(physic.ID);
            TransformComponent transform = ComponentManager.GetEntityComponent<TransformComponent>(physic.ID);

            UpdatePosition(physic, dt);

            if (rigidBody == null)
                ComponentManager.AddComponentToEntity(physic.ID, new PhysicsRigidbodyComponent());
            rigidBody = ComponentManager.GetEntityComponent<PhysicsRigidbodyComponent>(physic.ID);

            rigidBody.BodyToLocal = Matrix.CreateFromQuaternion(transform.Orientation);
            rigidBody.LocalToBody = Matrix.Invert(rigidBody.BodyToLocal);
            IntegrateStateVariables(transform, rigidBody, dt);

            PhysicsSystem.UpdateDeceleration(physic);
        }
        /// <summary>
        /// Updates the object position using its velocity * dt
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="dt"></param>
        private void UpdatePosition(PhysicsComponent physic, float dt)
        {
            //ComponentManager.GetEntityComponent<TransformComponent>(physic.ID).Position
            //        += physic.Velocity * dt + physic.Acceleration * dt * dt * 0.5f;

            ComponentManager.GetEntityComponent<TransformComponent>(physic.ID).Position
                    += physic.Velocity * dt;

            Vector3 rotation = ComponentManager.GetEntityComponent<TransformComponent>(physic.ID).Rotation;
            if (rotation.Length() != 0)
                ComponentManager.GetEntityComponent<TransformComponent>(physic.ID).Orientation
                    *= Quaternion.CreateFromAxisAngle(
                        ComponentManager.GetEntityComponent<TransformComponent>(physic.ID).Rotation * (1 / rotation.Length()), rotation.Length() * dt);
        }
        private void IntegrateStateVariables(TransformComponent transform, PhysicsRigidbodyComponent rigidbody, float dt)
        {
            // Linear position, velocity and acceleration is computed in the Pointmass base class.
            // Remains to do here:
            // Orientation
            Matrix A = Matrix.CreateFromQuaternion(transform.Orientation);
            A += Matrix.Multiply(Tilde(rigidbody.AngularVelocity), dt) * A;
            transform.Orientation = Quaternion.CreateFromRotationMatrix(A);
            transform.Orientation.Normalize();
            // Angular Momentum
            rigidbody.AngularMomentum += dt * rigidbody.TotalBodyTorque;
            // Update the world frame inverted inertia tensor
            CalculateWorldFrameInvertedInertiaTensor(transform.Orientation, rigidbody);
            // Update the angular velocity
            CalculateAngularVelocity(rigidbody);
        }
        private void CalculateWorldFrameInvertedInertiaTensor(Quaternion worldFrameOrientation, PhysicsRigidbodyComponent rigidbody)
        {
            Matrix A = Matrix.CreateFromQuaternion(worldFrameOrientation);
            rigidbody.WorldFrameInvertedInertiaTensor = A * rigidbody.BodyFrameInvertedInertiaTensor * Matrix.Transpose(A);
        }
        private void CalculateAngularVelocity(PhysicsRigidbodyComponent rigidbody)
        {
            rigidbody.AngularVelocity = Vector3.Transform(rigidbody.AngularMomentum, rigidbody.WorldFrameInvertedInertiaTensor);
        }
        private Matrix Tilde(Vector3 v)
        {
            return new Matrix(0, -v.Z, v.Y, 0,
            v.Z, 0, -v.X, 0,
            -v.Y, v.X, 0, 0,
            0, 0, 0, 1);
        }

    }
}
