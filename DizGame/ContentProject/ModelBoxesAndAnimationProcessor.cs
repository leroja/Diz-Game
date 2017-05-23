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
        BoundingSphere sphere;
        List<BoundingSphere> sphereList = new List<BoundingSphere>();
        Dictionary<string, object> modeldict;
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            ////System.Diagnostics.Debugger.Launch();

            ModelContent model = base.Process(input, context);

            modeldict = (Dictionary < string, object > )model.Tag;
            //foreach (ModelMeshContent meshContent in model.Meshes)
            //{
            //    sphereList.Add(meshContent.BoundingSphere);
            //    sphere = BoundingSphere.CreateMerged(sphere, meshContent.BoundingSphere);
            //}
            //sphereList.Insert(0, sphere);
            modeldict.Add("BoundingVolume", ModelBoundingBoxProcessor.Loop(input, this.Scale));
            model.Tag = modeldict;

            return model;

        }
    }
}
