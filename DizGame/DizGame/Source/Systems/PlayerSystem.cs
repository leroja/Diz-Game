using GameEngine.Source.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using Microsoft.Xna.Framework.Input;

namespace DizGame.Source.Systems
{
    public class PlayerSystem : IUpdate
    {
        private EntityFactory entFactory;

        private static Vector3 WORLD_X_AXIS = new Vector3(1.0f, 0.0f, 0.0f);
        private static Vector3 WORLD_Y_AXIS = new Vector3(0.0f, 1.0f, 0.0f);
        private static Vector3 WORLD_Z_AXIS = new Vector3(0.0f, 0.0f, 1.0f);

        public PlayerSystem(EntityFactory entFac)
        {
            this.entFactory = entFac;
        }
        public override void Update(GameTime gameTime)
        {
            var PlayerEntityIds = ComponentManager.GetAllEntitiesWithComponentType<PlayerComponent>();

            foreach (var playerId in PlayerEntityIds)
            {
                var playerComp = ComponentManager.GetEntityComponent<PlayerComponent>(playerId);
                var mouseComp = ComponentManager.GetEntityComponent<MouseComponent>(playerId);
                var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(playerId);
                var testComp = ComponentManager.GetEntityComponent<TestComponent>(playerId);

                //UpdateInput(ref mouseComp, ref testComp);
                //RotateSmoothly(testComp.SmoothedMouseMovement.X, testComp.SmoothedMouseMovement.Y, ref testComp, ref transformComp);

                var rot = transformComp.Rotation;
                //rot.X = 0;
                //rot.Y = 0;
                //rot.Z = 0;

                //if (mouseComp.MouseDeltaPosition.X > 0)
                //{
                //    rot.Y -= 0.01f;
                //}
                //if (mouseComp.MouseDeltaPosition.X < 0)
                //{
                //    rot.Y += 0.01f;
                //}

                //if (mouseComp.MouseDeltaPosition.Y > 0)
                //{
                //    rot.Z += 0.05f;
                //}
                //if (mouseComp.MouseDeltaPosition.Y < 0)
                //{
                //    rot.Z -= 0.05f;
                //}
                transformComp.Rotation = rot;





                if (mouseComp.GetState("Fire") == ButtonStates.Pressed)
                {
                    entFactory.CreateBullet("Bullet", transformComp.Position, transformComp.QuaternionRotation, new Vector3(.1f, .1f, .1f), 100);
                }
            }
        }

        private void UpdateInput(ref MouseComponent mouseComp, ref TestComponent testComp)
        {
            Rectangle clientBounds = GameOne.Instance.Window.ClientBounds;

            int centerX = clientBounds.Width / 2;
            int centerY = clientBounds.Height / 2;
            float deltaX = centerX - mouseComp.X;
            float deltaY = centerY - mouseComp.Y;

            Mouse.SetPosition(centerX, centerY);

            //Console.WriteLine(deltaX.ToString(), deltaY.ToString());

            testComp.SmoothedMouseMovement.X = deltaX;
            testComp.SmoothedMouseMovement.Y = deltaY;
        }

        private void RotateSmoothly(float headingDegrees, float pitchDegrees, ref TestComponent testComp, ref TransformComponent transComp)
        {
            headingDegrees *= testComp.RotationSpeed;
            pitchDegrees *= testComp.RotationSpeed;

            Rotate(headingDegrees, pitchDegrees, ref testComp, ref transComp);
        }
        public void Rotate(float headingDegrees, float pitchDegrees, ref TestComponent testComp, ref TransformComponent transComp)
        {
            //headingDegrees = -headingDegrees;
            pitchDegrees = -pitchDegrees;

            testComp.AccumPitchDegrees += pitchDegrees;
            Console.WriteLine("Pitch " + testComp.AccumPitchDegrees);

            if (testComp.AccumPitchDegrees > 90.0f)
            {
                pitchDegrees = 90.0f - (testComp.AccumPitchDegrees - pitchDegrees);
                testComp.AccumPitchDegrees = 90.0f;
            }

            if (testComp.AccumPitchDegrees < -90.0f)
            {
                pitchDegrees = -90.0f - (testComp.AccumPitchDegrees - pitchDegrees);
                testComp.AccumPitchDegrees = -90.0f;
            }

            testComp.AccumHeadingDegrees += headingDegrees;
            Console.WriteLine("Heading " + testComp.AccumHeadingDegrees);

            if (testComp.AccumHeadingDegrees > 360.0f)
                testComp.AccumHeadingDegrees -= 360.0f;

            if (testComp.AccumHeadingDegrees < -360.0f)
                testComp.AccumHeadingDegrees += 360.0f;

            float heading = MathHelper.ToRadians(headingDegrees);
            float pitch = MathHelper.ToRadians(pitchDegrees);
            Quaternion rotation = Quaternion.Identity;

            // Rotate the camera about the world Y axis.
            if (heading != 0.0f)
            {
                Quaternion.CreateFromAxisAngle(ref WORLD_Y_AXIS, heading, out rotation);
                //Quaternion.Concatenate(ref rotation, ref orientation, out orientation);
                Quaternion.Concatenate(ref rotation, ref transComp.QuaternionRotation, out transComp.QuaternionRotation);
            }

            // Rotate the camera about its local X axis.
            if (pitch != 0.0f)
            {
                Quaternion.CreateFromAxisAngle(ref WORLD_X_AXIS, pitch, out rotation);
                //Quaternion.Concatenate(ref orientation, ref rotation, out orientation);
                Quaternion.Concatenate(ref transComp.QuaternionRotation, ref rotation, out transComp.QuaternionRotation);
            }

            //UpdateViewMatrix();
        }
    }
}
