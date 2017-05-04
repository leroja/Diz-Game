using Microsoft.Xna.Framework;
using System;

namespace AnimationContentClasses
{
    public class KeyFrame
    {
        public int BoneIndex { get; private set; }

        public TimeSpan Time { get; private set; }
        public Matrix Transform { get; private set; }
        /// <summary>
        /// This class stores the transformation of a single bone as a matrix,
        /// the index of the bone the keyframe stores the transformation of,
        /// and the time from the beginning of the animation of the keyframe.
        /// </summary>
        /// <param name="Bone">index of the bone this keyframe animates</param>
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