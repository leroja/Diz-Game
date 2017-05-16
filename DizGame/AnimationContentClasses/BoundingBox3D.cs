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
        public BoundingBox Box { get; set; }

        public BoundingBox3D(BoundingBox box)
        {
            Box = box;
        }

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
