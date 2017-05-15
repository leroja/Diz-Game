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
    [ContentProcessor(DisplayName ="ModelWithSpheres")]
    class BoundingSphereProcessor : ModelProcessor
    {
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            //System.Diagnostics.Debugger.Launch();
            // Process the model with the default processor
            ModelContent model = base.Process(input, context);

            //Extract the model skeleton and all its animations
            BoundingVolume bv = CreateBoundingSpheres(model);
            // Stores the skeletal animation data in the model
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "BoundingVolume", bv }
            };
            model.Tag = dictionary;
            return model;
        }

        public static BoundingVolume CreateBoundingSpheres(ModelContent mc)
        {
            List<Vector3> verticePositions = new List<Vector3>();
            BoundingSphere local = new BoundingSphere();
            List<BoundingVolume> volume = Loop(mc.Bones, 3);
            foreach (BoundingVolume bound in volume)
            {
                local = BoundingSphere.CreateMerged(local, ((BoundingSphere3D)bound.Bounding).Sphere);
            }
            return new BoundingVolume(-1, volume, new BoundingSphere3D(local));

        }

        private static List<BoundingVolume> Loop(ModelBoneContentCollection mbcc, float sphere_size)
        {
            List<BoundingVolume> bvs = new List<BoundingVolume>();
            foreach (ModelBoneContent mbc in mbcc)
            {
                BoundingSphere sphere = new BoundingSphere(Vector3.Transform(Vector3.One, mbc.Transform), sphere_size);
                BoundingVolume bv = new BoundingVolume(mbc.Index, new BoundingSphere3D(sphere));
                if (mbc.Children.Count != 0)
                {
                    float tempsize = 0.0f;
                    if (sphere_size < 1)
                        tempsize = sphere_size + 1;
                    else tempsize = sphere_size - 1;
                    bv.Volume = Loop(mbc.Children, tempsize);
                    bvs.Add(bv);
                }
            }
            return bvs;
        }
    }
}
