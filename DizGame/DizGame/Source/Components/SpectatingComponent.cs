using GameEngine.Source.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class SpectatingComponent : IComponent
    {
        /// <summary>
        /// The id of the ........
        /// </summary>
        public int SpectatedEntity { get; set; }
    }
}
