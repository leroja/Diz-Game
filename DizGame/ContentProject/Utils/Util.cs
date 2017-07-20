using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentProject.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Gets the right min and max vector3 for the models boundingBox
        /// </summary>
        /// <param name="box"></param>
        /// <param name="scale"></param>
        /// <param name="position"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static void GetMinMax(BoundingBox box, float scale, Vector3 position, out Vector3 min, out Vector3 max)
        {
            min = box.Min * scale;
            max = box.Max * scale;
            float xDelta = (max - min).X;
            float zDelta = (max - min).Z;
            float yDelta = (max - min).Y;
            min.Y = position.Y;
            min.X = position.X - xDelta / 2;
            min.Z = position.Z - zDelta / 2;
            max.Y = position.Y + yDelta;
            max.X = position.X + xDelta / 2;
            max.Z = position.Z + zDelta / 2;
        }
        /// <summary>
        /// Static function used for scaling BoundingVolumes
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="scale"></param>
        /// <param name="position"></param>
        /// <param name="scaled"></param>
        public static void ScaleBoundingVolume(ref BoundingVolume volume, float scale, Vector3 position, out BoundingVolume scaled)
        {
            scaled = volume;
            List<BoundingVolume> temp = new List<BoundingVolume>();
            if (volume.Bounding.GetType().Name == "BoundingBox3D")
            {
                GetMinMax(((BoundingBox3D)volume.Bounding).Box, scale, position, out Vector3 min, out Vector3 max);

                foreach (var vol in volume.Volume)
                {
                    GetMinMax(((BoundingBox3D)vol.Bounding).Box, scale, position, out Vector3 min2, out Vector3 max2);
                    temp.Add(new BoundingVolume( new BoundingBox3D(new BoundingBox(min2, max2))));
                }
                scaled = new BoundingVolume(temp, new BoundingBox3D(new BoundingBox(min, max)));
            }
            else if (volume.Bounding.GetType().Name == "BoundingSphere3D")
            {
                BoundingSphere sphere = ((BoundingSphere3D)volume.Bounding).Sphere;
                sphere.Radius = ((BoundingSphere3D)volume.Bounding).Sphere.Radius * scale;
                sphere.Center = position;
                sphere.Center.Y += sphere.Radius;
                foreach (var vol in volume.Volume)
                {
                    BoundingSphere sphere2 = ((BoundingSphere3D)vol.Bounding).Sphere;
                    sphere2.Radius = ((BoundingSphere3D)vol.Bounding).Sphere.Radius * scale;
                    sphere2.Center = position;
                    sphere2.Center.Y += sphere.Radius;
                    temp.Add(new BoundingVolume( new BoundingSphere3D(sphere2)));
                }
                scaled = new BoundingVolume(temp, new BoundingSphere3D(sphere));
            }
        }
    }
}
