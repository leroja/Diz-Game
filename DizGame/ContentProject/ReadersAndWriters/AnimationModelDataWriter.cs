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
    public class AnimationModelDataWriter : ContentTypeWriter<AnimationModelData>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(AnimatedModelDataReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, AnimationModelData value)
        {
            output.WriteObject(value.BindPose);
            output.WriteObject(value.InverseBindPose);
            output.WriteObject(value.BonesParent);
            output.WriteObject(value.AnimationData);
        }
    }
}
