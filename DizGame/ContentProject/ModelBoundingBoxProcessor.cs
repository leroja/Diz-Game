using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentProject
{
    class ModelBoundingBoxProcessor
    {
        [ContentProcessor(DisplayName = "ModelBoundingBoxProcessor")]
        public class ModelBoundingSphereProcessor : ModelProcessor
        {
            BoundingBox box;
            List<BoundingBox> boxList = new List<BoundingBox>();
            public override ModelContent Process(NodeContent input, ContentProcessorContext context)
            {
                ModelContent model = base.Process(input, context);

                foreach (ModelMeshContent meshContent in model.Meshes)
                {
                    BoundingBox tempbox = BoundingBox.CreateFromSphere(meshContent.BoundingSphere);
                    boxList.Add(tempbox);
                    box = BoundingBox.CreateMerged(box, tempbox);   
                }
                boxList.Insert(0, box);
                model.Tag = boxList;

                return model;
            }

        }
    }
}
