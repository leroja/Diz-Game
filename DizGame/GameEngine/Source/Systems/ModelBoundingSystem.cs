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
            var dict = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<ModelComponent>();
            foreach (var modelEnt in dict)
            {
                //var modelComp = ComponentManager.GetEntityComponent<ModelComponent>(modelEnt);
                //var tcs = ComponentManager.GetEntityComponent<TransformComponent>(modelEnt.Key);
                GetModelBoundingVolume((ModelComponent)modelEnt.Value, modelEnt.Key);
            }
        }

        private void GetModelBoundingVolume(ModelComponent modComp, int entityId)
        {
            var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(entityId);
            //var box = new BoundingBox();
            if (modComp.BoundingVolume != null)
            {
                if (modComp.BoundingVolume.Bounding is BoundingSphere3D)
                {
                    BoundingSphere sphere = ((BoundingSphere3D)modComp.BoundingVolume.Bounding).Sphere;
                    ((BoundingSphere3D)modComp.BoundingVolume.Bounding).Sphere.Center = transformComp.Position;
                    ((BoundingSphere3D)modComp.BoundingVolume.Bounding).Sphere.Center.Y = transformComp.Position.Y + sphere.Radius;

                    //ModelComponent mComp = ComponentManager.GetEntityComponent<ModelComponent>(entityId);
                    //var sphere = new BoundingSphere(transformComp.Position + 
                    //    new Vector3(0, ((BoundingSphere3D)modComp.BoundingVolume.Bounding).Sphere.Radius, 0), 
                    //    ((BoundingSphere3D)modComp.BoundingVolume.Bounding).Sphere.Radius);
                    //modComp.BoundingVolume.Bounding = new BoundingSphere3D(sphere);
                }
                //if (modComp.BoundingVolume.Bounding is BoundingBox3D)
                //{
                //    BoundingBox box = new BoundingBox(((BoundingBox3D)modComp.BoundingVolume.Bounding).Box.Min + transformComp.Position, ((BoundingBox3D)modComp.BoundingVolume.Bounding).Box.Max + transformComp.Position);
                //    modComp.BoundingVolume.Bounding = new BoundingBox3D(box);
                //}
            }
        }
    }
}
