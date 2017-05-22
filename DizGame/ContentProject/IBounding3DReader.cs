using AnimationContentClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentProject.ReadersAndWriters
{
    class IBounding3DReader : ContentTypeReader<IBounding3D>
    {
        protected override IBounding3D Read(ContentReader input, IBounding3D existingInstance)
        {
            System.Diagnostics.Debugger.Launch();
            if (existingInstance is BoundingBox3D)
            {
                BoundingBox box = input.ReadObject<BoundingBox>();
                return new BoundingBox3D(box);
            }
            else if (existingInstance is BoundingSphere3D)
            {
                BoundingSphere sphere = input.ReadObject<BoundingSphere>();
                return new BoundingSphere3D(sphere);
            }
            return new BoundingSphere3D(new BoundingSphere());
        }
    }
}
