using AnimationContentClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentProject
{
    [ContentProcessor(DisplayName = "AnimatedModelProcessor")]
    public class AnimationProcessor : ModelProcessor
    {
        public static string TEXTURES_PATH = "Textures/";
        public static string EFFECTS_PATH = "Effects/";
        public static string EFFECT_FILENAME = "AnimatedModel.fx";

        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            // Process the model with the default processor
            ModelContent model = base.Process(input, context);

            //Extract the model skeleton and all its animations
            AnimationModelData animatedModelData = ExtractSkeletonAndAnimations(input, context);

            // Stores the skeletal animation data in the model
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "AnimationModelData", animatedModelData }
            };
            model.Tag = dictionary;

            return model;
        }

        private AnimationModelData ExtractSkeletonAndAnimations(NodeContent input, ContentProcessorContext context)
        {
            //Finds the root boon of the skeleton
            BoneContent skeleton = MeshHelper.FindSkeleton(input);
            //Transforms the skeleton tree into a list using deep search
            //this list are in the same order as they are indexed by the mesh’s vertices.
            IList<BoneContent> boneList = MeshHelper.FlattenSkeleton(skeleton);
            context.Logger.LogImportantMessage("{0} bones found.", boneList.Count);
            Matrix[] bonesBindPose = new Matrix[boneList.Count];
            Matrix[] inverseBindPose = new Matrix[boneList.Count];
            int[] bonesParentIndex = new int[boneList.Count];
            List<string> boneNameList = new List<string>(boneList.Count);

            for (int i = 0; i < boneList.Count; i++)
            {
                bonesBindPose[i] = boneList[i].Transform;
                inverseBindPose[i] = Matrix.Invert(boneList[i].AbsoluteTransform);
                int parentIndex = boneNameList.IndexOf(boneList[i].Parent.Name);
                bonesParentIndex[i] = parentIndex;
                boneNameList.Add(boneList[i].Name);
            }

            // Extract all animations

            AnimationData[] animations = ExtractAnimations(skeleton.Animations, boneNameList, context);

            return new AnimationModelData(bonesBindPose, inverseBindPose, bonesParentIndex, animations);

        }

        private AnimationData[] ExtractAnimations(AnimationContentDictionary animationDictionary, List<string> boneNameList, ContentProcessorContext context)
        {
            context.Logger.LogImportantMessage("{0} animations found.", animationDictionary.Count);

            AnimationData[] animations = new AnimationData[animationDictionary.Count];

            int count = 0;
            foreach (AnimationContent animationContent in animationDictionary.Values)
            {
                // Store all keyframes of the animation
                List<KeyFrame> keyframes = new List<KeyFrame>();

                //Go through all animation channels, each bone has its own channel
                foreach (string animationKey in animationContent.Channels.Keys)
                {
                    AnimationChannel animationChannel = animationContent.Channels[animationKey];
                    int boneIndex = boneNameList.IndexOf(animationKey);
                    foreach (AnimationKeyframe keyframe in animationChannel)
                        keyframes.Add(new KeyFrame(
                        boneIndex, keyframe.Time, keyframe.Transform));

                }

                // Sort all animation frames by time
                keyframes.Sort();

                animations[count++] = new AnimationData(animationContent.Name, animationContent.Duration, keyframes.ToArray());

            }

            return animations;
        }

        protected override MaterialContent ConvertMaterial(MaterialContent material, ContentProcessorContext context)
        {
            return base.ConvertMaterial(material, context);
        }
    }
}
