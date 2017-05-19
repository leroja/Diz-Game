using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationContentClasses
{
    public class BoundingBox3D : IBounding3D
    {
        /// <summary>
        /// The BoundingBox
        /// </summary>
        public BoundingBox Box { get; set; }

        /// <summary>
        /// Creates new BoundingBox3D and takes a BoundingBox as a parameter
        /// </summary>
        /// <param name="box"></param>
        public BoundingBox3D(BoundingBox box)
        {
            Box = box;
        }
        /// <summary>
        /// Checks if this Box intersect with another IBounding3D
        /// </summary>
        /// <param name="bounding"></param>
        /// <returns></returns>
        public override bool Intersects(IBounding3D bounding)
        {
            if (bounding is BoundingBox3D)
            {
                return Box.Intersects(((BoundingBox3D)bounding).Box);
            }
            else if (bounding is BoundingSphere3D)
            {
                return Box.Intersects(((BoundingSphere3D)bounding).Sphere);
            }
            else if (bounding is BoundingFrustum3D)
            {
                return Box.Intersects(((BoundingFrustum3D)bounding).Frustum);
            }
            return false;
        }
        /// <summary>
        /// Merges this Box with another IBounding3D
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        public override IBounding3D CreateMerged(IBounding3D bound)
        {
            if (bound is BoundingBox3D)
            {
                return new BoundingBox3D(BoundingBox.CreateMerged(Box, ((BoundingBox3D)bound).Box));
            }
            if (bound is BoundingSphere3D)
            {
                return new BoundingBox3D(BoundingBox.CreateMerged(Box, BoundingBox.CreateFromSphere(((BoundingSphere3D)bound).Sphere)));
            }
            return default(IBounding3D);
        }
    }
}
