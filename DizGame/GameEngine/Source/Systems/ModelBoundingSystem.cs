using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using AnimationContentClasses;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// Recalculates the models bounding Spheres if it is necessary.
    /// If the model is static it is not needed to recalculate the bounding sphere
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
            var dict = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<ModelComponent>();
            Parallel.ForEach(dict, modelEnt =>
            {
                GetModelBoundingVolume((ModelComponent)modelEnt.Value, modelEnt.Key);
            });
        }

        /// <summary>
        /// Updates the BoundingVolume of the model to its position in the world
        /// </summary>
        /// <param name="modComp"></param>
        /// <param name="entityId"></param>
        private void GetModelBoundingVolume(ModelComponent modComp, int entityId)
        {
            var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(entityId);
            if (modComp.BoundingVolume != null)
            {
                if (modComp.BoundingVolume.Bounding is BoundingSphere3D)
                {
                    BoundingSphere sphere = ((BoundingSphere3D)modComp.BoundingVolume.Bounding).Sphere;
                    ((BoundingSphere3D)modComp.BoundingVolume.Bounding).Sphere.Center = transformComp.Position;
                    ((BoundingSphere3D)modComp.BoundingVolume.Bounding).Sphere.Center.Y = transformComp.Position.Y + sphere.Radius;
                }
            }
        }
    }
}