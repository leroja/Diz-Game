using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationContentClasses
{
    /// <summary>
    /// AnimationClip class contains information for one single "clip".
    /// While a model can have multiple animationclips, one clip is one single 
    /// animation. For a humanoid character animtions could for example be: running, walking and jumping
    /// each of these would then be represented as three animationclips.
    /// </summary>
    public class AnimationClip
    {
        /// <summary>
        /// Duration is the total time for how long the clip should be 'running'
        /// </summary>
        public TimeSpan Duration { get; private set; }
        /// <summary>
        /// Keyframes is data for different part of the animation, (could maybe be described like 'screenshots' 
        /// for the model at different times while running the animation) which contains information about what 
        /// part of the model that should be manipulated durring the animation at which time and so forth.
        /// </summary>
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
