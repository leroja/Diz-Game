using GameEngine.Source.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// An abstact class for the systems in the game engine and the game
    /// </summary>
    public abstract class ISystem
    {
        /// <summary>
        /// 
        /// </summary>
        protected ComponentManager ComponentManager { get; } = ComponentManager.Instance;
    }
}
