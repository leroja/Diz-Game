using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content.Pipeline;
using AnimationContentClasses;

namespace ContentProject
{
    [ContentTypeWriter]
    public class KeyframeWriter : ContentTypeWriter<KeyFrame>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(KeyframeReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, KeyFrame value)
        {
            output.WriteObject(value.Time);
            output.Write(value.BoneIndex);
            output.Write(value.Transform);
        }
    }
}
