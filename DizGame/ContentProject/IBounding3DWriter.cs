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
    [ContentTypeWriter]
    class IBounding3DWriter : ContentTypeWriter<IBounding3D>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(IBounding3D).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, IBounding3D value)
        {
            if (value is BoundingBox3D)
            {
                output.WriteObject(((BoundingBox3D)value).Box);
            }
            else if (value is BoundingSphere3D)
            {
                output.WriteObject(((BoundingSphere3D)value).Sphere);
            }
        }
    }
}
