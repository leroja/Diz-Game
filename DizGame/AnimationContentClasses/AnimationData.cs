using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationContentClasses
{
    public class AnimationData
    {
        public string Name { get; private set; }
        public TimeSpan Duration { get; private set; }
        public KeyFrame [] KeyFrames { get; private set; }
        /// <summary>
        /// This class stores all of the keyframes for all of the bones of an animation
        /// </summary>
        /// <param name="Duration">Total length of the clip</param>
        /// <param name="KeyFrames">List of keyframes for all of the bones of an animation</param>
        public AnimationData(string Name, TimeSpan Duration, KeyFrame[] KeyFrames)
        {
            this.Name = Name;
            this.Duration = Duration;
            this.KeyFrames = KeyFrames;
        }

        private AnimationData()
        {

        }
    }
}
