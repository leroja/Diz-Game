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
        public ModelBoundingSystem()
        {
            var ids = ComponentManager.GetAllEntitiesWithComponentType<ModelComponent>();
            foreach (var modelEnt in ids)
            {
                var modelComp = ComponentManager.GetEntityComponent<ModelComponent>(modelEnt);
            }
        }
        public override void Update(GameTime gameTime)
        {
            var ids = ComponentManager.GetAllEntitiesWithComponentType<ModelComponent>();
            foreach (var modelEnt in ids)
            {
                var modelComp = ComponentManager.GetEntityComponent<ModelComponent>(modelEnt);
                BoundingVolume volume = modelComp.BoundingVolume;
                if (volume != null)
                {
                    var sphere = GetModelBoundingSphere(volume, modelEnt);
                }
            }
        }

        private BoundingVolume GetModelBoundingSphere(BoundingVolume volume, int entityId)
        {
            var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(entityId);
            var sphere = new BoundingSphere(transformComp.Position, 0);
            var box = new BoundingBox();
            foreach (var bVolume in volume.Volume)
            {
                var s = bVolume.Bounding;
                if (s is BoundingSphere3D)
                    sphere = BoundingSphere.CreateMerged(sphere, ((BoundingSphere3D)s).Sphere);
                if (s is BoundingBox3D)
                    box = BoundingBox.CreateMerged(box, ((BoundingBox3D)s).Box);
            }
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
                return volume;
        }
    }
}
