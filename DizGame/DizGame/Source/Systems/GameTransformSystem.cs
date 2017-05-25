using GameEngine.Source.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;

namespace DizGame.Source.Systems
{
    public class GameTransformSystem : IUpdate
    {
        /// <summary>
        /// the system for this component should handle the objects world matrix calculation
        /// i.e objectWorld = Matrix.CreateScale(scale) * rotation * Matrix.CreateTranslation(position);
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            var mc = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<TransformComponent>();


            foreach (var entity in mc)
            {
                TransformComponent tfc = ComponentManager.GetEntityComponent<TransformComponent>(entity.Key);
                var rotationQuaternion = Quaternion.CreateFromYawPitchRoll(tfc.Rotation.Y, tfc.Rotation.X, tfc.Rotation.Z);

                tfc.QuaternionRotation = rotationQuaternion;
                tfc.Forward = Vector3.Transform(Vector3.Forward, tfc.QuaternionRotation);
                tfc.Up = Vector3.Transform(Vector3.Up, tfc.QuaternionRotation);
                tfc.Right = Vector3.Transform(Vector3.Right, tfc.QuaternionRotation);

                tfc.ModelMatrix = Matrix.CreateScale(tfc.Scale)
                    * Matrix.CreateFromQuaternion(tfc.QuaternionRotation)
                    * Matrix.CreateTranslation(tfc.Position);

                // TODO: THIS SHIT
                rotationQuaternion = Quaternion.CreateFromYawPitchRoll(tfc.Rotation.Y, 0,0);

                tfc.QuaternionRotation = rotationQuaternion;
                tfc.Forward = Vector3.Transform(Vector3.Forward, tfc.QuaternionRotation);
                tfc.Up = Vector3.Transform(Vector3.Up, tfc.QuaternionRotation);
                tfc.Right = Vector3.Transform(Vector3.Right, tfc.QuaternionRotation);

                tfc.ObjectMatrix = Matrix.CreateScale(tfc.Scale)
                    * Matrix.CreateFromQuaternion(tfc.QuaternionRotation)
                    * Matrix.CreateTranslation(tfc.Position);
            }
        }
    }
}
