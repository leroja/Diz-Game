using AnimationContentClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentProject
{
    /// <summary>
    /// AnimationProcessor extending the Modelprocessor class to enable custom content processing.
    /// </summary>
    [ContentProcessor(DisplayName = "ModelBoundingSphereProcessor")]
    public class ModelBoundingSphereProcessor : ModelProcessor
    {
        BoundingSphere sphere;
        List<BoundingSphere> sphereList = new List<BoundingSphere>();
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            ModelContent model = base.Process(input, context);

            foreach (ModelMeshContent meshContent in model.Meshes)
            {
                sphereList.Add(meshContent.BoundingSphere);
                sphere = BoundingSphere.CreateMerged(sphere, meshContent.BoundingSphere);
            }
            sphereList.Insert(0, sphere);
            model.Tag = sphereList;

            return model;
        }

    }
}
