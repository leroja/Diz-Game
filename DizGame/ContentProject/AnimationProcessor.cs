using AnimationContentClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentProject
{
    /// <summary>
    /// AnimationProcessor extending the ModelProcessor class to enable custom content processing.
    /// </summary>
    [ContentProcessor(DisplayName = "AnimatedModelProcessor")]
    public class AnimationProcessor : ModelProcessor
    {

        /// <summary>
        /// Overridden method to enable custom processing of models loaded within the content pipeline
        /// </summary>
        /// <param name="input">the data retrieved from the content pipeline</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            //System.Diagnostics.Debugger.Launch();
            //ValidateMesh(input, context, null);

            BoneContent skeleton = MeshHelper.FindSkeleton(input);
            if (skeleton == null)
                throw new InvalidContentException("Input skeleton not found.");

            //FlattenTransforms(input, skeleton);

            IList<BoneContent> bones = MeshHelper.FlattenSkeleton(skeleton);

            if (bones.Count > SkinnedEffect.MaxBones)
            {
                throw new InvalidContentException(string.Format(
                    "Skeleton has {0} bones, but the maximum supported is {1}.",
                    bones.Count, SkinnedEffect.MaxBones));
            }

            List<Matrix> bindPose = new List<Matrix>();
            List<Matrix> inverseBindPose = new List<Matrix>();
            List<int> skeletonHierarchy = new List<int>();

            foreach (BoneContent bone in bones)
            {
                bindPose.Add(bone.Transform);
                inverseBindPose.Add(Matrix.Invert(bone.AbsoluteTransform));
                skeletonHierarchy.Add(bones.IndexOf(bone.Parent as BoneContent));
            }

            // Convert animation data to our runtime format.
            Dictionary<string, AnimationClip> animationClips;
            animationClips = ProcessAnimations(skeleton.Animations, bones);

            // Chain to the base ModelProcessor class so it can convert the model data.
            ModelContent model = base.Process(input, context);
            Dictionary<string, object> modeldict = new Dictionary<string, object>
            {
                ["SkinningData"] = new SkinningData(animationClips, bindPose,
                                     inverseBindPose, skeletonHierarchy)
            };
            // Store our custom animation data in the Tag property of the model.
            model.Tag = modeldict;

            return model;
        }

        static Dictionary<string, AnimationClip> ProcessAnimations(AnimationContentDictionary animations, IList<BoneContent> bones)
        {
            // Build up a table mapping bone names to indices.
            Dictionary<string, int> boneMap = new Dictionary<string, int>();

            for (int i = 0; i < bones.Count; i++)
            {
                    boneMap.Add(bones[i].Name, i);
            }

            // Convert each animation in turn.
            Dictionary<string, AnimationClip> animationClips = new Dictionary<string, AnimationClip>();

            foreach (KeyValuePair<string, AnimationContent> animation in animations)
            {
                AnimationClip processed = ProcessAnimation(animation.Value, boneMap);

                animationClips.Add(animation.Key, processed);
            }

            if (animationClips.Count == 0)
            {
                throw new InvalidContentException(
                            "Input file does not contain any animations.");
            }

            return animationClips;
        }

        static AnimationClip ProcessAnimation(AnimationContent animation, Dictionary<string, int> boneMap)
        {
            List<KeyFrame> keyframes = new List<KeyFrame>();

            // For each input animation channel.
            foreach (KeyValuePair<string, AnimationChannel> channel in animation.Channels)
            {
                // Look up what bone this channel is controlling.
                int boneIndex = boneMap[channel.Key];
                
                // Convert the keyframe data.
                foreach (AnimationKeyframe keyframe in channel.Value)
                {
                    keyframes.Add(new KeyFrame(boneIndex, keyframe.Time,
                                               keyframe.Transform));
                }
            }

            // Sort the merged keyframes by time.
            keyframes.Sort(CompareKeyframeTimes);

            if (keyframes.Count == 0)
                throw new InvalidContentException("Animation has no keyframes.");

            if (animation.Duration <= TimeSpan.Zero)
                throw new InvalidContentException("Animation has a zero duration.");

            return new AnimationClip(animation.Duration, keyframes);
        }

        static int CompareKeyframeTimes(KeyFrame a, KeyFrame b)
        {
            return a.Time.CompareTo(b.Time);
        }

    

        /// <summary>
        /// Force all the materials to use our skinned model effect.
        /// </summary>
        [DefaultValue(MaterialProcessorDefaultEffect.SkinnedEffect)]
        public override MaterialProcessorDefaultEffect DefaultEffect
        {
            get { return MaterialProcessorDefaultEffect.SkinnedEffect; }
            set { }
        }

    }
}
