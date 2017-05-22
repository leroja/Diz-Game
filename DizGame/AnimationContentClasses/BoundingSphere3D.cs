using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimationContentClasses
{
    public class BoundingSphere3D : IBounding3D
    {
        public BoundingSphere Sphere;

        private BoundingSphere3D() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ball"></param>
        public BoundingSphere3D(BoundingSphere ball)
        {
            Sphere = ball;
        }
        /// <summary>
        /// Checks if this Sphere intersects with another IBounding3D
        /// </summary>
        /// <param name="bounding"></param>
        /// <returns></returns>
        public override bool Intersects(IBounding3D bounding)
        {
            if (bounding is BoundingBox3D)
            {
                return Sphere.Intersects(((BoundingBox3D)bounding).Box);
            }
            else if (bounding is BoundingSphere3D)
            {
                return Sphere.Intersects(((BoundingSphere3D)bounding).Sphere);
            }
            return false;
        }

        /// <summary>
        /// Merges this sphere with an other IBounding3D
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        public override IBounding3D CreateMerged(IBounding3D bound)
        {
            if (bound is BoundingBox3D)
            {
                //Vector3 min = ((BoundingBox3D)bound).Box.Min;
                //Vector3 max = ((BoundingBox3D)bound).Box.Max;
                //Vector3 middlePoint = (max - min) / 2 + min;
                //Vector3 tot = (max - min);
                //float temp = float.MinValue;
                //if (tot.X > temp)
                //    temp = tot.X;
                //if (tot.Y > temp)
                //    temp = tot.Y;
                //if (tot.Z > temp)
                //    temp = tot.Z;
                //BoundingSphere.CreateMerged(Sphere, new BoundingSphere(middlePoint, temp)
                return new BoundingSphere3D(BoundingSphere.CreateMerged(Sphere, BoundingSphere.CreateFromBoundingBox(((BoundingBox3D)bound).Box)));
            }
            if (bound is BoundingSphere3D)
            {
                return new BoundingSphere3D(BoundingSphere.CreateMerged(Sphere, ((BoundingSphere3D)bound).Sphere));
            }
            return default(IBounding3D);
        }
    }
}

