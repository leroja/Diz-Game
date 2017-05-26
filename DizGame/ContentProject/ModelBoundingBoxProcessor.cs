using AnimationContentClasses;
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
    /// <summary>
    /// 
    /// </summary>
    [ContentProcessor(DisplayName = "ModelBoundingBoxProcessor")]
    public class ModelBoundingBoxProcessor : ModelProcessor
    {
        static BoundingBox box;
        List<BoundingBox> boxList = new List<BoundingBox>();

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
            box = new BoundingBox();
            model.Tag = Loop(input, this.Scale);

            return model;
        }

        public static BoundingVolume Loop(NodeContent content, float scale)
        {
            BoundingVolume volume = new BoundingVolume(1);
            var cont = content as MeshContent;
            if (cont != null)
            {
                
                foreach (GeometryContent g in cont.Geometry)
                {
                    //System.Diagnostics.Debugger.Launch();
                    BoundingBox tempBox = BoundingBox.CreateFromPoints(g.Vertices.Positions);
                    box = BoundingBox.CreateMerged(box, tempBox);
                    return new BoundingVolume(new BoundingBox3D(tempBox));
                }
            }
            else
            {
                foreach(var n in content.Children)
                {
                    volume.Volume.Add(Loop(n, scale));
                }
            }
            volume.Bounding = new BoundingBox3D(box);
            return volume;
        }
    }
}
