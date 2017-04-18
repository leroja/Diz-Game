using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Source.Components.Interface;

namespace GameEngine.Source.Components
{
    class BoundingSphereComponent : IComponent
    {
        public BoundingSphere sphere;
        public bool hasCollided;
        
        public BoundingSphereComponent(BoundingSphere ball)
        {
            sphere = ball;
            hasCollided = false;
        }

    }
}

