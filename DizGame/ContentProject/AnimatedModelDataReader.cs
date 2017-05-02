using System;
using AnimationContentClasses;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ContentProject
{
    public class AnimatedModelDataReader : ContentTypeReader<AnimationModelData>
    {
        protected override AnimationModelData Read(ContentReader input, AnimationModelData existingInstance)
        {
            Matrix[] bonesBindPose = input.ReadObject<Matrix[]>();
            Matrix[] bonesInverseBindPose = input.ReadObject<Matrix[]>();
            int[] bonesParent = input.ReadObject<int[]>();
            AnimationData[] animations =
            input.ReadObject<AnimationData[]>();
            return new AnimationModelData(bonesBindPose,
            bonesInverseBindPose, bonesParent, animations);
        }
    }
}