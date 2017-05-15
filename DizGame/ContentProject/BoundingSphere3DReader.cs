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
    class BoundingSphere3DReader : ContentTypeReader<BoundingSphere3D>
    {

        protected override BoundingSphere3D Read(ContentReader input, BoundingSphere3D existingInstance)
        {
            return new BoundingSphere3D(input.ReadObject<BoundingSphere>());
        }
    }
}
