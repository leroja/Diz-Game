﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimationContentClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace ContentProject
{
    [ContentProcessor(DisplayName = "ModelBoxesWithAnimationProcessor")]
    class ModelBoxesAndAnimationProcessor : AnimationProcessor
    {
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            //System.Diagnostics.Debugger.Launch();
            
            BoundingVolume bv = BoundingBoxProcessor.CreateBoundingBoxes(input);
            ModelContent model = base.Process(input, context);
            // Stores the bounding data in the model
            ((Dictionary<string, object>)model.Tag).Add("BoundingVolume", bv);
            return model;
        }
    }
}
