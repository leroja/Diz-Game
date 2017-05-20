using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.RandomStuff;
using AnimationContentClasses;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// Recalculates the models bounding Spheres if it is necessary.
    /// If the model is static it is not needed to recalculte the bounding sphere
    /// </summary>
    public class ModelBoundingSystem : IUpdate
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ModelBoundingSystem()
        {
        }

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

                UpdateModelBoundingVolume(modelComp, modelEnt);
            }
        }

        /// <summary>
        /// Updates the BoundingVolume of the model to its position in the world
        /// </summary>
        /// <param name="modComp"></param>
        /// <param name="entityId"></param>
        private void UpdateModelBoundingVolume(ModelComponent modComp, int entityId)
        {
            if (modComp.BoundingVolume != null)
            {
                var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(entityId);

                if (modComp.BoundingVolume.Bounding is BoundingSphere3D)
                {
                    BoundingSphere3D bounding = modComp.BoundingVolume.Bounding as BoundingSphere3D;
                    bounding.Sphere = new BoundingSphere(transformComp.Position + modComp.MiddlePosition, bounding.Sphere.Radius);
                }
            }
            //else if (modComp.BoundingVolume.Bounding is BoundingBox3D)
            //{
            //    BoundingBox3D bounding = modComp.BoundingVolume.Bounding as BoundingBox3D;
            //    bounding.Box = new BoundingBox(bounding.Box.Min + transformComp.Position, bounding.Box.Max + transformComp.Position);
            //}
            //modComp.BoundingVolume = new BoundingVolume(0, new BoundingSphere3D(new BoundingSphere(new Vector3(transformComp.Position.X, transformComp.Position.Y, transformComp.Position.Z), 3)));
            //if (volume.Volume.FirstOrDefault().Bounding is BoundingSphere3D)
            //{
            //    volume.Bounding = new BoundingSphere3D(sphere);
            //    volume.BoundingID = -1;
            //}
            //if (volume.Volume.FirstOrDefault().Bounding is BoundingBox3D)
            //{
            //    volume.Bounding = new BoundingBox3D(box);
            //    volume.BoundingID = -1;
            //}
        }
    }
}
