using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Components
{
    public class BoundingSphereComponent : IComponent
    {
        public BoundingSphere Sphere { get; set; }
        public bool HasCollided { get; set; }
        
        public BoundingSphereComponent(BoundingSphere ball)
        {
            Sphere = ball;
            HasCollided = false;
        }

    }
}

