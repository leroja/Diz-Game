using System;
using AnimationContentClasses;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace ContentProject
{
    public class AnimationDataReader : ContentTypeReader<AnimationData>
    {
        protected override AnimationData Read(ContentReader input, AnimationData existingInstance)
        {
            string name = input.ReadString();
            TimeSpan duration = input.ReadObject<TimeSpan>();
            KeyFrame[] keyframes = input.ReadObject<KeyFrame[]>();
            return new AnimationData(name, duration, keyframes);
        }
    }
}