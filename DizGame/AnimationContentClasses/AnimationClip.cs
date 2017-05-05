using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationContentClasses
{
    public class AnimationClip
    {
     
        public TimeSpan Duration { get; private set; }
        public List<KeyFrame> KeyFrames { get; private set; }
        /// <summary>
        /// This class stores all of the keyframes for all of the bones of an animation
        /// </summary>
        /// <param name="Duration">Total length of the clip</param>
        /// <param name="KeyFrames">List of keyframes for all of the bones of an animation</param>
        public AnimationClip(TimeSpan Duration, List<KeyFrame> KeyFrames)
        {
            
            this.Duration = Duration;
            this.KeyFrames = KeyFrames;
        }

        private AnimationClip()
        {

        }
    }
}
