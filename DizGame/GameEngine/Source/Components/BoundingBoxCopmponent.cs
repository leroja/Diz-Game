using GameEngine.Source.Components.Interface;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    class BoundingBoxCopmponent : IComponent
    {
        public BoundingBox box;

        public BoundingBoxCopmponent(BoundingBox bux)
        {
            box = bux;

        }
    }
}
