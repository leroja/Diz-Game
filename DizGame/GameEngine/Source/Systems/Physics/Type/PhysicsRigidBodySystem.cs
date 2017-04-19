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
            PhysicsComponent physics = ComponentManager.GetEntityComponent<PhysicsComponent>(entityID);
            RigidbodyComponent rigidBody = ComponentManager.GetEntityComponent<RigidbodyComponent>(entityID);
            if (rigidBody == null)
                ComponentManager.AddComponentToEntity(entityID, new RigidbodyComponent());
            rigidBody = ComponentManager.GetEntityComponent<RigidbodyComponent>(entityID);

            rigidBody.BodyToLocal = Matrix.CreateFromQuaternion(physics.Orientation);
            rigidBody.LocalToBody = Matrix.Invert(rigidBody.BodyToLocal);
            IntegrateStateVariables(physics, rigidBody, dt);
        }
        private void IntegrateStateVariables(PhysicsComponent physics, RigidbodyComponent rigidbody, float dt)
        {
            // Linear position, velocity and acceleration is computed in the Pointmass base class.
            // Remains to do here:
            // Orientation
            Matrix A = Matrix.CreateFromQuaternion(physics.Orientation);
            A += Matrix.Multiply(Tilde(rigidbody.AngularVelocity), dt) * A;
            physics.Orientation = Quaternion.CreateFromRotationMatrix(A);
            physics.Orientation.Normalize();
            // Angular Momentum
            rigidbody.AngularMomentum += dt * rigidbody.TotalBodyTorque;
            // Update the world frame inverted inertia tensor
            CalculateWorldFrameInvertedInertiaTensor(physics.Orientation, rigidbody);
            // Update the angular velocity
            CalculateAngularVelocity(rigidbody);
        }
        private void CalculateWorldFrameInvertedInertiaTensor(Quaternion worldFrameOrientation, RigidbodyComponent rigidbody)
        {
            Matrix A = Matrix.CreateFromQuaternion(worldFrameOrientation);
            rigidbody.WorldFrameInvertedInertiaTensor = A * rigidbody.BodyFrameInvertedInertiaTensor * Matrix.Transpose(A);
        }
        private void CalculateAngularVelocity(RigidbodyComponent rigidbody)
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
