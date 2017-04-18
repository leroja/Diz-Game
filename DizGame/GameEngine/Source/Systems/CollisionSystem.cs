using GameEngine.Source.Components;
using GameEngine.Source.Components.Interface;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems
{
    class CollisionSystem
    {
        public void CollisionDetection()
        {
            List<int> sphereEntities = ComponentManager.Instance.GetAllEntitiesWithComponentType<BoundingSphereComponent>();

            for (int i = 0; i < sphereEntities.Count - 1; i++)
            {
                BoundingSphereComponent sphereComp1 = ComponentManager.Instance.GetEntityComponent<BoundingSphereComponent>(sphereEntities[i]);

                for (int j = 1; j < sphereEntities.Count; i++)
                {
                    BoundingSphereComponent sphereComp2 = ComponentManager.Instance.GetEntityComponent<BoundingSphereComponent>(sphereEntities[j]);
                    if (sphereComp1.sphere.Intersects(sphereComp2.sphere))
                    {
                        if (ComponentManager.Instance.CheckIfEntityHasComponent<ModelComponent>(sphereEntities[i]) && ComponentManager.Instance.CheckIfEntityHasComponent<ModelComponent>(sphereEntities[j]))
                        {
                            ModelComponent mComp1 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(sphereEntities[i]);
                            ModelComponent mComp2 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(sphereEntities[j]);
                            List<BoundingSphere> spheres1 = mComp1.Model.Meshes.Select(s => s.BoundingSphere).ToList();
                            List<BoundingSphere> spheres2 = mComp2.Model.Meshes.Select(s => s.BoundingSphere).ToList();

                            if (FindFirstHit(spheres1, spheres2))
                            {
                                sphereComp1.hasCollided = true;
                                sphereComp2.hasCollided = true;
                            }
                        }
                    }
                }
            }
        }
        public bool FindFirstHit(List<BoundingSphere> spheres1, List<BoundingSphere> spheres2)
        {
            BoundingSphere s1 = spheres1.FirstOrDefault();
            BoundingSphere s2 = spheres2.FirstOrDefault();
            if (s1 != null || s2 != null)
            {
                if (s1.Intersects(s2)) return true;
                else if (spheres1.Count > spheres2.Count)
                {
                    spheres1.Remove(s1);
                    FindFirstHit(spheres1, spheres2);
                }
                else if (spheres1.Count < spheres2.Count)
                {
                    spheres2.Remove(s2);
                    FindFirstHit(spheres1, spheres2);
                }
            }
            return false;
        }
    }
}
