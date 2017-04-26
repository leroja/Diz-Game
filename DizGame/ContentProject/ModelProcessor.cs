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
    class ModelProcessor : ContentProcessor<Model, Model>
    {
        public override Model Process(Model input, ContentProcessorContext context)
        {
            AddboundingSphereToModel(input.Meshes);
            return input;
        }

        private void AddboundingSphereToModel(ModelMeshCollection mc)
        {
            List<Vector3> verticePositions = new List<Vector3>();
            foreach (ModelMesh mm in mc)
            {
                foreach(ModelMeshPart mp in mm.MeshParts)
                {
                    Vector3[] temp = new Vector3[mp.NumVertices];
                    mp.VertexBuffer.GetData<Vector3>(temp);
                    verticePositions.AddRange(temp);
                }
                BoundingSphere sphere = BoundingSphere.CreateFromPoints(verticePositions);
                mm.BoundingSphere = sphere;
            }
        }
    }
}
