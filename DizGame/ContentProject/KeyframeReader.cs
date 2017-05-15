using System;
using AnimationContentClasses;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ContentProject
{
    public class KeyframeReader : ContentTypeReader<KeyFrame>
    {
        protected override KeyFrame Read(ContentReader input, KeyFrame existingInstance)
        {
            TimeSpan time = input.ReadObject<TimeSpan>();
            int boneIndex = input.ReadInt32();
            Matrix transform = input.ReadMatrix();
            return new KeyFrame(boneIndex, time, transform);
        }
    }
}