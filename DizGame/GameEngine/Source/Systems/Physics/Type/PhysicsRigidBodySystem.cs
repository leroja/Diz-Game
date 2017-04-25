using GameEngine.Source.Systems.Interface;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Managers;

namespace GameEngine.Source.Systems
{
    public class PhysicsRigidBodySystem : IPhysicsType
    {
        public override void Update(int entityID, float dt)
        {
            // Creates and send this throught instead of creating globas that takes memory
            PhysicsComponent physics = ComponentManager.GetEntityComponent<PhysicsComponent>(entityID);
            PhysicsRigidbodyComponent rigidBody = ComponentManager.GetEntityComponent<PhysicsRigidbodyComponent>(entityID);
            TransformComponent transform = ComponentManager.GetEntityComponent<TransformComponent>(entityID);

            if (rigidBody == null)
                ComponentManager.AddComponentToEntity(entityID, new PhysicsRigidbodyComponent());
            rigidBody = ComponentManager.GetEntityComponent<PhysicsRigidbodyComponent>(entityID);

            rigidBody.BodyToLocal = Matrix.CreateFromQuaternion(transform.Orientation);
            rigidBody.LocalToBody = Matrix.Invert(rigidBody.BodyToLocal);
            IntegrateStateVariables(transform, rigidBody, dt);
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
