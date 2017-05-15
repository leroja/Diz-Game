using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// Recalculates the models bounding Spheres if it is necessary.
    /// If the model is static it is not needed to recalculte the bounding sphere
    /// </summary>
    public class ModelBoundingSphereSystem : IUpdate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var ids = ComponentManager.GetAllEntitiesWithComponentType<ModelComponent>();
            foreach (var modelEnt in ids)
            {
                var modelComp = ComponentManager.GetEntityComponent<ModelComponent>(modelEnt);
                if (!modelComp.IsStatic || modelComp.BoundingSphere.Radius == 0)
                {
                    var sphere = GetModelBoundingSphere(modelComp, modelEnt);
                    modelComp.BoundingSphere = sphere;
                }
            }
        }

        private BoundingSphere GetModelBoundingSphere(ModelComponent modelComp, int entityId)
        {
            var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(entityId);
            var sphere = new BoundingSphere(transformComp.Position, 0);

            var boneTransforms = new Matrix[modelComp.Model.Bones.Count];
            modelComp.Model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (var mesh in modelComp.Model.Meshes)
            {
                var meshTransform = boneTransforms[mesh.ParentBone.Index] * transformComp.ObjectMatrix;

                var s = mesh.BoundingSphere;
                s = s.Transform(meshTransform);
                sphere = BoundingSphere.CreateMerged(sphere, s);
            }
            return sphere;
        }
    }
}
