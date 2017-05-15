using AnimationContentClasses;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentProject
{
    class BoundingVolumeReader : ContentTypeReader<BoundingVolume>
    {
        protected override BoundingVolume Read(ContentReader input, BoundingVolume existingInstance)
        {
            int id = input.ReadInt32();
            List<BoundingVolume> vol = input.ReadObject<List<BoundingVolume>>();
            IBounding3D bound = input.ReadObject<IBounding3D>();
            return new BoundingVolume(id, vol, bound);
        }
    }
}
