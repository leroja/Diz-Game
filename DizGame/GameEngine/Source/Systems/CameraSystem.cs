using GameEngine.Source.Systems.Abstract_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;

namespace GameEngine.Source.Systems
{
    public class CameraSystem : IUpdate
    {
        public override void Update(GameTime gameTime)
        {
            foreach(int entityID in ComponentManager.GetAllEntitiesWithComponentType<CameraComponent>())
            {
                CameraComponent camera = ComponentManager.GetEntityComponent<CameraComponent>(entityID);
                TransformComponent transform = ComponentManager.GetEntityComponent<TransformComponent>(entityID);
               if(camera != null || transform != null)
                {
                    UpdateCameraAfterType(camera, transform);
                    camera.Projection = Matrix.CreatePerspectiveFieldOfView(camera.FieldOfView, camera.AspectRatio, camera.NearPlane, camera.FarPlane);
                }
            }
        }
        private void UpdateCameraAfterType(CameraComponent camera, TransformComponent transform)
        {
            switch(camera.CameraType)
            {
                case Enums.CameraType.Pov:
                    Vector3 lookAtOffset = Vector3.Transform(Vector3.UnitZ, Matrix.CreateRotationX(transform.Rotation.X) * Matrix.CreateRotationY(transform.Rotation.Y));
                    camera.LookAt = transform.Position + lookAtOffset;
                    camera.View = Matrix.CreateLookAt(transform.Position, camera.LookAt, Vector3.Up);
                    break;
                case Enums.CameraType.StaticCam:
                    camera.LookAt = transform.Position;
                    camera.View = Matrix.CreateLookAt(transform.Position, camera.LookAt, Vector3.Up);
                    break;
                case Enums.CameraType.Chase:
                    Vector3 transformedOffset = Vector3.Transform(camera.Offset, Matrix.CreateFromQuaternion(transform.QuaternionRotation));
                    Vector3 camPos = transform.Position + transformedOffset;
                    Vector3 up = Vector3.Transform(Vector3.Up, Matrix.CreateFromQuaternion(transform.QuaternionRotation));
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
