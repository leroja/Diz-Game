using GameEngine.Source.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems
{
    public abstract class ISystem
    {
        protected ComponentManager ComponentManager { get; } = ComponentManager.Instance;
    }
}
