using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimationContentClasses
{
    public class BoundingFrustum3D : IBounding3D
    {
        public BoundingFrustum Frustum { get; set; }

        public BoundingFrustum3D(BoundingFrustum frustum)
        {
            Frustum = frustum;
        }
        /// <summary>
        /// Checks if this Frustum intersects with another IBounding3D
        /// </summary>
        /// <param name="bounding"></param>
        /// <returns></returns>
        public override bool Intersects(IBounding3D bounding)
        {
            if (bounding is BoundingBox3D)
            {
                return Frustum.Intersects(((BoundingBox3D)bounding).Box);
            }
            else if (bounding is BoundingSphere3D)
            {
                return Frustum.Intersects(((BoundingSphere3D)bounding).Sphere);
            }
            return false;
        }

        /// <summary>
        /// This function should not be used
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        public override IBounding3D CreateMerged(IBounding3D bound)
        {
            return default(IBounding3D);
        }
    }
}
