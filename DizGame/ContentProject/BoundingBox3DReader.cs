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
    class BoundingBox3DReader : ContentTypeReader<BoundingBox3D>
    {
        protected override BoundingBox3D Read(ContentReader input, BoundingBox3D existingInstance)
        {
            return new BoundingBox3D(input.ReadObject<BoundingBox>());
        }
    }
}
