using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentProject
{
    [ContentProcessor(DisplayName = "Test")]
    class TestModelProcessor : ModelProcessor
    {
        private void AddboundingSphereToModel(ModelMeshCollection mc, out List<BoundingSphere> spheres)
        {
            spheres = new List<BoundingSphere>();
            List<Vector3> verticePositions = new List<Vector3>();
            foreach (ModelMesh mm in mc)
            {
                foreach (ModelMeshPart mp in mm.MeshParts)
                {
                    Vector3[] temp = new Vector3[mp.NumVertices];
                    mp.VertexBuffer.GetData<Vector3>(temp);
                    verticePositions.AddRange(temp);
                }
                for (int i = 0; i < verticePositions.Count; i+=6)
                {
                    BoundingSphere sphere = BoundingSphere.CreateFromPoints(new List<Vector3>
                    {
                        verticePositions[i],
                        verticePositions[i+1],
                        verticePositions[i+2],
                        verticePositions[i+3],
                        verticePositions[i+4],
                        verticePositions[i+5]
                    });
                    spheres.Add(sphere);
                }
            }
        }
    }
}
