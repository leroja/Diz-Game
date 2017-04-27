using GameEngine.Source.Systems.Interface;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Managers;

namespace GameEngine.Source.Systems
{
    public class PhysicsRigidBodySystem : IPhysicsType
    {
        public override void Update(PhysicsComponent physic, float dt)
        {
            // Creates and send this throught instead of creating globas that takes memory
            PhysicsRigidbodyComponent rigidBody = ComponentManager.GetEntityComponent<PhysicsRigidbodyComponent>(physic.ID);
            TransformComponent transform = ComponentManager.GetEntityComponent<TransformComponent>(physic.ID);

            UpdateLinearPosition(physic, dt);

            if (rigidBody == null)
                ComponentManager.AddComponentToEntity(physic.ID, new PhysicsRigidbodyComponent());
            rigidBody = ComponentManager.GetEntityComponent<PhysicsRigidbodyComponent>(physic.ID);

            rigidBody.BodyToLocal = Matrix.CreateFromQuaternion(transform.Orientation);
            rigidBody.LocalToBody = Matrix.Invert(rigidBody.BodyToLocal);
            IntegrateStateVariables(transform, rigidBody, dt);
        }
        /// <summary>
        /// Updates the object position using its velocity * dt
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="dt"></param>
        private void UpdateLinearPosition(PhysicsComponent physic, float dt)
        {
            //ComponentManager.GetEntityComponent<TransformComponent>(physic.ID).Position
            //    += ComponentManager.GetEntityComponent<PhysicsComponent>(physic.ID).Velocity * dt;

            //ComponentManager.GetEntityComponent<TransformComponent>(physic.ID).Position
            //    += ComponentManager.GetEntityComponent<PhysicsComponent>(physic.ID).Velocity * dt + (0.5f * physic.Acceleration * dt * dt);
            ComponentManager.GetEntityComponent<TransformComponent>(physic.ID).Position
                += physic.Distance;
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
