using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ContentProject
{
    [ContentProcessor(DisplayName = "Model Processor - MonoGame.Extended")]
    class ModelProcessor : ContentProcessor<Model, TempModel>
    {
        public override TempModel Process(Model input, ContentProcessorContext context)
        {
            AddboundingSphereToModel(input, out TempModel tempModel);
            return tempModel;
        }

        private void AddboundingSphereToModel(Model model, out TempModel tempModel)
        {
            List<BoundingSphere> spheres = new List<BoundingSphere>();
            foreach (ModelBone mb in model.Bones)
            {
                Vector3 pos = Vector3.Transform(Vector3.One, mb.Transform);
            }
            tempModel = new TempModel(model, spheres);
        }
    }
}
