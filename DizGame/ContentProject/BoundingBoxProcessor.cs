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
            
            //Extract the model skeleton and all its animations
            BoundingVolume bv = CreateBoundingBoxes(input);

            ModelContent model = base.Process(input, context);
            // Stores the skeletal animation data in the model
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary["BoundingVolume"] = bv;
            model.Tag = dictionary;
            return model;
        }

        public static BoundingVolume CreateBoundingBoxes(NodeContent node)
        {
            List<Vector3> verticePositions = new List<Vector3>();
            BoundingBox local = new BoundingBox();
            List<BoundingVolume> volume = new List<BoundingVolume>();
            volume.AddRange(Loop(node.Children, 3));
            foreach (BoundingVolume bound in volume)
            {
                local = BoundingBox.CreateMerged(local, ((BoundingBox3D)bound.Bounding).Box);
            }
            return new BoundingVolume(-1, volume, new BoundingBox3D(local));
        }

        private static List<BoundingVolume> Loop(NodeContentCollection ncc, float box_size)
        {
            List<BoundingVolume> bvs = new List<BoundingVolume>();
            foreach (NodeContent bone in ncc)
            {
                BoundingBox box = new BoundingBox(Vector3.Transform(Vector3.One, bone.Transform - Matrix.CreateTranslation(new Vector3(box_size/2, box_size/2, box_size/2))),
                    Vector3.Transform(Vector3.One, bone.Transform + Matrix.CreateTranslation(new Vector3(box_size / 2, box_size / 2, box_size / 2))));
                BoundingVolume bv = new BoundingVolume(ncc.IndexOf(bone.Parent as BoneContent), new BoundingBox3D(box));
                if (bone.Children.Count != 0)
                {
                    float tempsize = 0.0f;
                    if (box_size < 1)
                        tempsize = box_size + 1;
                    else tempsize = box_size - 1;
                    bv.Volume = Loop(bone.Children, tempsize);
                    bvs.Add(bv);
                }
            }
            return bvs;
        }
    }
}
