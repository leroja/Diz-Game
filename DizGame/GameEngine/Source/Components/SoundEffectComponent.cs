using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// A component that contains a list of sound effects that shall be played
    /// </summary>
    public class SoundEffectComponent : IComponent
    {
        /// <summary>
        /// a list that contains a number of sound effects that is going to be played
        /// Sound effect name, pan, pitch
        /// </summary>
        public List<Tuple<string, float, float>> SoundEffectsToBePlayed { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SoundEffectComponent()
        {
            SoundEffectsToBePlayed = new List<Tuple<string, float, float>>();
        }
    }
}
