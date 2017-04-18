using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components.Interface;

namespace GameEngine.Source.Components
{
    class WorldComponent : IComponent
    {
        public Matrix _world { get; set; }

        public WorldComponent(Matrix world)
        {
            _world = world;
        }
        
    }
}
