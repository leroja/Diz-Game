using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using ContentProject;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// System to update the CameraComponents
    /// derived from IUpdate
    /// </summary>
    public class CameraSystem : IUpdate
    {
        /// <summary>
        /// Updates all the cameras View, Projection using corresponding transform component.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Parallel.ForEach(ComponentManager.GetAllEntitiesWithComponentType<CameraComponent>(), entityID =>
            {
                CameraComponent camera = ComponentManager.GetEntityComponent<CameraComponent>(entityID);
                TransformComponent transform = ComponentManager.GetEntityComponent<TransformComponent>(entityID);

                UpdateCameraAfterType(camera, transform);
                camera.Projection = Matrix.CreatePerspectiveFieldOfView(camera.FieldOfView, camera.AspectRatio, camera.NearPlane, camera.FarPlane);
                camera.CameraFrustrum = new BoundingFrustum3D(new BoundingFrustum(camera.View * camera.Projection));
            });
        }

        /// <summary>
        /// Updates the cameras LookAt and View based by CameraType
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="transform"></param>
        private void UpdateCameraAfterType(CameraComponent camera, TransformComponent transform)
        {
            switch (camera.CameraType)
            {
                case Enums.CameraType.Pov:
                    var rotTest = Matrix.CreateFromQuaternion(transform.QuaternionRotation);
                    Vector3 transformedOffsetTest = Vector3.Transform(camera.Offset, rotTest);
                    Vector3 cameraPos = transform.Position + transformedOffsetTest;
                    Vector3 upTest = Vector3.Transform(Vector3.Up, rotTest);

                    Vector3 lookAtOffset = Vector3.Transform(Vector3.UnitZ, Matrix.CreateRotationX(transform.Rotation.X) * Matrix.CreateRotationY(transform.Rotation.Y));
                    camera.LookAt = cameraPos - lookAtOffset;
                    camera.View = Matrix.CreateLookAt(cameraPos, camera.LookAt, upTest);
                    break;
                case Enums.CameraType.StaticCam:
                    camera.View = Matrix.CreateLookAt(transform.Position, camera.LookAt, Vector3.Up);
                    break;
                case Enums.CameraType.Chase:
                    var rot = Matrix.CreateFromQuaternion(transform.QuaternionRotation);
                    Vector3 transformedOffset = Vector3.Transform(camera.Offset, rot);
                    Vector3 camPos = transform.Position + transformedOffset;
                    Vector3 up = Vector3.Transform(Vector3.Up, rot);
                    camera.View = Matrix.CreateLookAt(camPos, transform.Position, up);
                    break;
                default:
                    camera.LookAt = transform.Position;
                    camera.View = Matrix.CreateLookAt(transform.Position, camera.LookAt, Vector3.Up);
                    break;
            }
        }
    }
}