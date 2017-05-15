using System;
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
    [ContentProcessor(DisplayName = "ModelWithBoxes")]
    class BoundingBoxProcessor : ModelProcessor
    {
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            //System.Diagnostics.Debugger.Launch();
            // Process the model with the default processor
            ModelContent model = base.Process(input, context);

            //Extract the model skeleton and all its animations
            BoundingVolume bv = CreateBoundingBoxes(model);
            // Stores the skeletal animation data in the model
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "BoundingVolume", bv }
            };
            model.Tag = dictionary;
            return model;
        }

        public static BoundingVolume CreateBoundingBoxes(ModelContent mc)
        {
            List<Vector3> verticePositions = new List<Vector3>();
            BoundingBox local = new BoundingBox();
            List<BoundingVolume> volume = Loop(mc.Bones, 3);
            foreach (BoundingVolume bound in volume)
            {
                local = BoundingBox.CreateMerged(local, ((BoundingBox3D)bound.Bounding).Box);
            }
            return new BoundingVolume(-1, volume, new BoundingBox3D(local));

        }

        public static BoundingVolume CreateFromMeshes(ModelContent mc)
        {
            return null;
        }

        private static List<BoundingVolume> Loop(ModelBoneContentCollection mbcc, float box_size)
        {
            List<BoundingVolume> bvs = new List<BoundingVolume>();
            foreach (ModelBoneContent mbc in mbcc)
            {
                BoundingBox box = new BoundingBox(Vector3.Transform(Vector3.One, mbc.Transform - Matrix.CreateTranslation(new Vector3(box_size/2, box_size/2, box_size/2))),
                    Vector3.Transform(Vector3.One, mbc.Transform + Matrix.CreateTranslation(new Vector3(box_size / 2, box_size / 2, box_size / 2))));
                BoundingVolume bv = new BoundingVolume(mbc.Index, new BoundingBox3D(box));
                if (mbc.Children.Count != 0)
                {
                    float tempsize = 0.0f;
                    if (box_size < 1)
                        tempsize = box_size + 1;
                    else tempsize = box_size - 1;
                    bv.Volume = Loop(mbc.Children, tempsize);
                    bvs.Add(bv);
                }
            }
            return bvs;
        }
    }
}
