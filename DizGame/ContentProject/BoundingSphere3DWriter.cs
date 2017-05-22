using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using AnimationContentClasses;

namespace ContentProject.ReadersAndWriters
{
    [ContentTypeWriter]
    class BoundingSphere3DWriter : ContentTypeWriter<BoundingSphere3D>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(BoundingSphere3D).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, BoundingSphere3D value)
        {
            output.WriteObject(value.Sphere);
        }
    }
}
