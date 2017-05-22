using Microsoft.Xna.Framework;
using System;

namespace AnimationContentClasses
{
    /// <summary>
    /// An animation consists of multiple keyframes, each keyframe contains the information 
    /// for which part in a model (bone) that should be manipulated (how? with a transformation matrix) 
    /// durring the current time (Time) in the animation.
    /// </summary>
    public class KeyFrame
    {
        /// <summary>
        /// The index of the bone which is suposed to be transformed in this keyframe
        /// </summary>
        public int BoneIndex { get; private set; }

        /// <summary>
        /// Represents the beginning of a keyframe in an animation.
        /// </summary>
        public TimeSpan Time { get; private set; }

        /// <summary>
        /// A transformation Matrix representing a transformation for a single bone with regards to a single animation.
        /// </summary>
        public Matrix Transform { get; private set; }
        /// <summary>
        /// This class stores the transformation of a single bone as a matrix,
        /// the index of the bone the keyframe stores the transformation of,
        /// and the time from the beginning of the animation of the keyframe.
        /// </summary>
        /// <param name="BoneIndex">index of the bone this keyframe animates</param>
        /// <param name="Time">Time for the beginning of the animation of this keyframe</param>
        /// <param name="Transform">Bone transform for this keyframe</param>
        public KeyFrame(int BoneIndex, TimeSpan Time, Matrix Transform)
        {
            this.BoneIndex = BoneIndex;
            this.Time = Time;
            this.Transform = Transform;
        }

        private KeyFrame()
        {

        }

     
    }
}