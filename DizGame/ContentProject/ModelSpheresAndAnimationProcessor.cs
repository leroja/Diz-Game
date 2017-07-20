using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace ContentProject
{
    [ContentProcessor(DisplayName = "ModelSpheresWithAnimationProcessor")]
    class ModelSpheresAndAnimationProcessor : AnimationProcessor
    {
        Dictionary<string, object> modeldict;

        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            ModelContent model = base.Process(input, context);

            modeldict = (Dictionary<string, object>)model.Tag;

            modeldict.Add("BoundingVolume", ModelBoundingSphereProcessor.Loop(input, this.Scale));
            model.Tag = modeldict;

            return model;
        }
    }
}
