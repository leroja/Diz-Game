using GameEngine.Source.Components;
using GameEngine.Source.Systems.Abstract_classes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems
{
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
                TransformComponent tfc = ComponentManager.GetEntityComponent<TransformComponent>(entity.Key);
                CalculateTransformComponent(tfc);
            }
            Dictionary<int, IComponent> animationEntities = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<AnimationComponent>();

            foreach (var ent in animationEntities)
            {
                AnimationComponent ac = ComponentManager.GetEntityComponent<AnimationComponent>(ent.Key);
                foreach (var tfc in ac.AnimationTransforms)
                {
                    CalculateTransformComponent(tfc.Value);
                }
            }
        }
        private void CalculateTransformComponent(TransformComponent tfc)
        {
            
            var rotationQuaternion = Quaternion.CreateFromYawPitchRoll(tfc.Rotation.Y, tfc.Rotation.X, tfc.Rotation.Z);

            tfc.QuaternionRotation *= rotationQuaternion;
            tfc.Forward = Vector3.Transform(Vector3.Forward, tfc.QuaternionRotation);
            var up = Vector3.Transform(Vector3.Up, tfc.QuaternionRotation);
            var right = Vector3.Transform(Vector3.Right, tfc.QuaternionRotation);

            tfc.ObjectMatrix = Matrix.CreateScale(tfc.Scale) * Matrix.CreateFromQuaternion(tfc.QuaternionRotation) * Matrix.CreateTranslation(tfc.Position);
        }
    }
}
