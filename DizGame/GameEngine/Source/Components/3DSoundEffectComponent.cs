using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// A component for entity that emit sounds
    /// </summary>
    public class _3DSoundEffectComponent : IComponent
    {
        /// <summary>
        /// A list that contains the sound effects that shall be played
        /// sound effect name, volume
        /// </summary>
        public List<Tuple<string, float>> SoundEffectsToBePlayed { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public _3DSoundEffectComponent()
        {
            SoundEffectsToBePlayed = new List<Tuple<string, float>>();
        }
    }
}
