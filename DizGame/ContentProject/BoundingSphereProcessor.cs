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
    [ContentProcessor(DisplayName = "ModelWithSpheres")]
    class BoundingSphereProcessor : ModelProcessor
    {
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {

            System.Diagnostics.Debugger.Launch();
            //Extract the model skeleton and all its animations

            BoundingVolume bv = CreateBoundingSpheresFromMeshes(input);

            ModelContent model = base.Process(input, context);
            // Stores the skeletal animation data in the model
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "BoundingVolume", bv }
            };
            model.Tag = dictionary;
            return model;
        }

        public static BoundingVolume CreateBoundingSpheres(NodeContent boneList)
        {
            BoundingSphere local = new BoundingSphere();
            List<BoundingVolume> volume = new List<BoundingVolume>();
            volume = Loop(boneList.Children, 3);
            foreach (BoundingVolume bound in volume)
            {
                local = BoundingSphere.CreateMerged(local, ((BoundingSphere3D)bound.Bounding).Sphere);
            }
            return new BoundingVolume(-1, volume, new BoundingSphere3D(local));
        }

        public static BoundingVolume CreateBoundingSpheresFromMeshes(NodeContent boneList)
        {
            return Loop(boneList.Children, 3)[1];
        }

        private static List<BoundingVolume> Loop(NodeContentCollection ncc, float sphere_size)
        {
            List<BoundingVolume> bvs = new List<BoundingVolume>();
            foreach (NodeContent node in ncc)
            {
                BoundingSphere sphere = new BoundingSphere(Vector3.Transform(Vector3.One, node.Transform), sphere_size);
                BoundingVolume bv = new BoundingVolume(ncc.IndexOf(node.Parent as BoneContent), new BoundingSphere3D(sphere));
                if (node.Children.Count != 0)
                {
                    float tempsize = 0.0f;
                    if (sphere_size < 1)
                        tempsize = sphere_size + 1;
                    else tempsize = sphere_size - 1;
                    bv.Volume = Loop(node.Children, tempsize);
                    bvs.Add(bv);
                }
            }
            return bvs;
        }

        private static List<BoundingVolume> LoopMeshes(NodeContentCollection ncc, float sphere_size)
        {
            List<Vector3> points = new List<Vector3>();
            List<BoundingVolume> bvs = new List<BoundingVolume>();
            foreach (MeshContent node in ncc)
            {
                foreach (GeometryContent geoCol in node.Geometry)
                {
                    VertexContent content = geoCol.Vertices;
                    //BoundingSphere sphere = BoundingSphere.CreateFromPoints(geoCol.Vertices.)
                    BoundingVolume bv = new BoundingVolume(ncc.IndexOf(node.Parent as BoneContent), new BoundingSphere3D(new BoundingSphere()));
                    if (node.Children.Count != 0)
                    {
                        float tempsize = 0.0f;
                        if (sphere_size < 1)
                            tempsize = sphere_size + 1;
                        else tempsize = sphere_size - 1;
                        bv.Volume = LoopMeshes(node.Children, tempsize);
                        bvs.Add(bv);
                    }
                }

            }
            return bvs;
        }
    }
}
