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

        public bool Intersects(IBounding3D bounding)
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

        public IBounding3D CreateMerged(IBounding3D bound)
        {
            return default(IBounding3D);
        }
    }
}
