using AnimationContentClasses;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace ContentProject.ReadersAndWriters
{
    class BoundingBox3DWriter : ContentTypeWriter<BoundingBox3D>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(BoundingBox3D).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, BoundingBox3D value)
        {
            output.WriteObject(value.Box);
        }
    }
}
