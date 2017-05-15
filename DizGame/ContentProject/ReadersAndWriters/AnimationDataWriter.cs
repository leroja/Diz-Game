using AnimationContentClasses;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace ContentProject
{
    [ContentTypeWriter]
    public class AnimationDataWriter : ContentTypeWriter<AnimationData>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(AnimationDataReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, AnimationData value)
        {
            output.Write(value.Name);
            output.WriteObject(value.Duration);
            output.WriteObject(value.KeyFrames);
        }
    }
}
