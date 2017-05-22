using GameEngine.Source.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// Transform system responsible for handling all entities with transform components 
    /// and the calculations for which the transformations require within the update sequence of a game.
    /// </summary>
    public class TransformSystem : IUpdate
    {

        /// <summary>
        /// the system for this component should handle the objects world matrix calculation
        /// i.e objectWorld = Matrix.CreateScale(scale) * rotation * Matrix.CreateTranslation(position);
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            Dictionary<int, IComponent> mc = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<TransformComponent>();


            foreach (var entity in mc)
            {
                //TransformComponent tfc = ComponentManager.GetEntityComponent<TransformComponent>(entity.Key);
                TransformComponent tfc = (TransformComponent)entity.Value;
                var rotationQuaternion = Quaternion.CreateFromYawPitchRoll(tfc.Rotation.Y, tfc.Rotation.X, tfc.Rotation.Z);
                
                tfc.QuaternionRotation = rotationQuaternion;
                tfc.Forward = Vector3.Transform(Vector3.Forward, tfc.QuaternionRotation);
                tfc.Up = Vector3.Transform(Vector3.Up, tfc.QuaternionRotation);
                tfc.Right = Vector3.Transform(Vector3.Right, tfc.QuaternionRotation);

                tfc.ObjectMatrix = Matrix.CreateScale(tfc.Scale) * tfc.RotationMatrix 
                    * Matrix.CreateFromQuaternion(tfc.QuaternionRotation) 
                    * Matrix.CreateTranslation(tfc.Position);

                tfc.Orientation *= Quaternion.CreateFromRotationMatrix(tfc.ObjectMatrix); // TODO: vill få fram heading vet ej om detta är korrekt
            }

        }
    }
}
