using AnimationContentClasses;
using Microsoft.Xna.Framework;
using System;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// AnimationComponent class, used for/stores the relevant information to 
    /// create animations for a model.
    /// </summary>
    public class AnimationComponent : IComponent
    {
        #region Properties

        /// <summary>
        /// Information about the currently playing animation clip.
        /// </summary>
        public AnimationClip CurrentClipValue { get; private set; }
        /// <summary>
        /// The current time to controll where in the animation we are
        /// </summary>
        public TimeSpan CurrentTimeValue { get; set; }
        /// <summary>
        /// The current keyframe which is "playing"
        /// </summary>
        public int CurrentKeyframe { get; set; }


        /// <summary>
        /// Arrays of matrices relevant for the transformations of the animations
        /// </summary>
        public Matrix[] BoneTransforms { get; set; }
        /// <summary>
        /// Arrays of matrices relevant for the transformations of the animations
        /// </summary>
        public Matrix[] WorldTransforms { get; set; }
        /// <summary>
        /// Arrays of matrices relevant for the transformations of the animations
        /// </summary>
        public Matrix[] SkinTransforms { get; set; }
        /////////////////////////////////////////////////////////////////////////

        // Backlink to the bind pose and skeleton hierarchy data.
        /// <summary>
        /// SkinningDataValue is the parameter for which the animation data finally are stored.
        /// This contains the skeleton hierarchy data aswell as the bind pose for the model.
        /// </summary>
        public SkinningData SkinningDataValue { get; set; }

        //public Effect AnimationEffect { get; set; }
        //AnimatedModelEffect AnimatedModelEffect {get; set;}
        #endregion

        /// <summary>
        /// Basic constructor for the AnimationComponent class
        /// </summary>
        /// <param name="tag">Takes the tag which is stored within a model loaded with a content extension, this tag should contain all the relevant information for constructing the animations for a model</param>
        public AnimationComponent(object tag)
        {
            SkinningDataValue = tag as SkinningData;

            if (SkinningDataValue == null)
                throw new InvalidOperationException("This model does not contain a SkinningData tag.");

            BoneTransforms = new Matrix[SkinningDataValue.BindPose.Count];
            WorldTransforms = new Matrix[SkinningDataValue.BindPose.Count];
            SkinTransforms = new Matrix[SkinningDataValue.BindPose.Count];
        }

        /// <summary>
        /// Starts decoding the specified animation clip.
        /// </summary>
        public void StartClip(string clipName)
        {
            if (clipName == null)
                throw new ArgumentNullException("clipname is missing");

            CurrentClipValue = (SkinningDataValue.AnimationClips[clipName]);

            CurrentTimeValue = TimeSpan.Zero;
            CurrentKeyframe = 0;

            // Initialize bone transforms to the bind pose.
            SkinningDataValue.BindPose.CopyTo(BoneTransforms, 0);
        }
    }
}
