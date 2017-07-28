using GameEngine.Source.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
            var mc = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<TransformComponent>();

            Parallel.ForEach(mc, entity =>
            {
                TransformComponent tfc = (TransformComponent)entity.Value;
                tfc.QuaternionRotation = Quaternion.CreateFromYawPitchRoll(tfc.Rotation.Y, tfc.Rotation.X, tfc.Rotation.Z);
                
                tfc.Forward = Vector3.Transform(Vector3.Forward, tfc.QuaternionRotation);
                tfc.Up = Vector3.Transform(Vector3.Up, tfc.QuaternionRotation);
                tfc.Right = Vector3.Transform(Vector3.Right, tfc.QuaternionRotation);

                tfc.ObjectMatrix = Matrix.CreateScale(tfc.Scale)
                    * Matrix.CreateFromQuaternion(tfc.QuaternionRotation)
                    * Matrix.CreateTranslation(tfc.Position);
            });
        }
    }
}