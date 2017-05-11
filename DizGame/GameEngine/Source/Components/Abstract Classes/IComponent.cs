using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// Abstract class
    /// </summary>
    public abstract class IComponent
    {
        /// <summary>
        /// To get the components ID
        /// </summary>
        public int ID { get; set; }
    }
}
