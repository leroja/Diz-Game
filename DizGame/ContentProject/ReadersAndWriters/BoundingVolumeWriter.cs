using AnimationContentClasses;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentProject
{
    class BoundingVolume3DWriter : ContentTypeWriter<BoundingVolume>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(BoundingVolume).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, BoundingVolume value)
        {
            output.Write(value.BoundingID);
            output.WriteObject(value.Volume);
            output.WriteObject(value.Bounding);
        }
    }
}
