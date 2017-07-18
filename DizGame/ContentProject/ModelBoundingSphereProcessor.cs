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
    /// AnimationProcessor extending the ModelProcessor class to enable custom content processing.
    /// </summary>
    [ContentProcessor(DisplayName = "ModelBoundingSphereProcessor")]
    public class ModelBoundingSphereProcessor : ModelProcessor
    {
        private static BoundingSphere sphere;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            ModelContent model = base.Process(input, context);
            //MeshContent mFirst = input as MeshContent;
            //if (mFirst == null)
            //{
            //    foreach (NodeContent node in input.Children)
            //    {
            //        MeshContent m = node as MeshContent;
            //        foreach (GeometryContent g in m.Geometry)
            //        {
            //            BoundingBox tempBox = BoundingBox.CreateFromPoints(g.Vertices.Positions);
            //            boxList.Add(tempBox);
            //            box = BoundingBox.CreateMerged(box, tempBox);
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (GeometryContent g in mFirst.Geometry)
            //    {
            //        BoundingBox tempBox = BoundingBox.CreateFromPoints(g.Vertices.Positions);
            //        boxList.Add(tempBox);
            //        box = BoundingBox.CreateMerged(box, tempBox);
            //    }
            //}
            ////Loop(input);
            //boxList.Insert(0, box);
            //System.Diagnostics.Debugger.Launch();
            sphere = default(BoundingSphere);
            model.Tag = Loop(input, this.Scale);

            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static BoundingVolume Loop(NodeContent content, float scale)
        {
            BoundingVolume volume = new BoundingVolume(1);
            var cont = content as MeshContent;
            if (cont != null)
            {

                foreach (GeometryContent g in cont.Geometry)
                {
                    //System.Diagnostics.Debugger.Launch();
                    BoundingSphere tempSphere = BoundingSphere.CreateFromPoints(g.Vertices.Positions);
                    tempSphere.Radius *= 1 / scale;
                    sphere = BoundingSphere.CreateMerged(sphere, tempSphere);
                    volume.Bounding = new BoundingSphere3D(tempSphere);
                    if (content.Children.Count > 0)
                    {
                        foreach (var n in content.Children)
                        {
                            volume.Volume.Add(Loop(n, scale));
                        }
                    }
                    return volume;
                }
            }
            if (content == null || content.Children.Count > 0)
            {
                foreach (var n in content.Children)
                {
                    volume.Volume.Add(Loop(n, scale));
                }
            }
            volume.Bounding = new BoundingSphere3D(sphere);
            return volume;
        }
    }
}
