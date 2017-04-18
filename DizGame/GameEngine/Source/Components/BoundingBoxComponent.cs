﻿using GameEngine.Source.Components.Interface;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    class BoundingBoxComponent : IComponent
    {
        private BoundingBox box;

        public BoundingBoxComponent(BoundingBox bux)
        {
            box = bux;
            
        }

        public BoundingBox BoundingVolume
        {
            get
            {
                return box;
            }

            set
            {
                box = value;
            }
        }
    }
}