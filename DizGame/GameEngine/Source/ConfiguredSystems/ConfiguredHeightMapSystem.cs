using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Systems;
using GameEngine;
using GameEngine.Source.Objects;
using GameEngine.Source.Managers;
using System.Drawing;

namespace GameEngine.Source.ConfiguredSystems
{
    public class ConfiguredHeightMapSystem
    {
        public static readonly ConfiguredHeightMapSystem Instance = new ConfiguredHeightMapSystem();

        private HeightMapSystem  hmSystem { get; set; }

        private ConfiguredHeightMapSystem()
        {
            List<HeightMapObject> hmobj = generateHeighMapObjects();

            hmSystem = new HeightMapSystem(ref hmobj);

        }

        public void ThisFunctionDoesNothing()
        {

        }

        private List<HeightMapObject> generateHeighMapObjects()
        {
            List<HeightMapObject> hmobjs = new List<HeightMapObject>();



            HeightMapObject hmobj1 = new HeightMapObject()
            {
                // det ska inte finnas något i Content Mappen i enginen
                scaleFactor = 1.0f,
                terrainMapName = "Content/HeightMaps/heightmap.png",
            };



            hmobjs.Add(hmobj1);

            initialiseHeightMapObject(hmobjs);

            return hmobjs;
        }

        private void initialiseHeightMapObject(List<HeightMapObject> hmobjs)
        {

            foreach (HeightMapObject hmobj in hmobjs)
            {
                Bitmap bmpHeightdata = new Bitmap(hmobj.terrainMapName);
                hmobj.terrainHeight = bmpHeightdata.Height;
                hmobj.terrainWidth = bmpHeightdata.Width;

                //vertexCount = terrainWidth * terrainHeight;
                //indexCount = (terrainWidth - 1) * (terrainHeight - 1) * 6;

                //vertices = new VertexPositionNormalTexture[vertexCount];
                //indices = new int[indexCount];

                hmobj.heightData = new float[hmobj.terrainWidth, hmobj.terrainHeight];

                for (int x = 0; x < hmobj.terrainWidth; x++)
                {
                    for (int y = 0; y < hmobj.terrainHeight; y++)
                    {
                        System.Drawing.Color color = bmpHeightdata.GetPixel(x, y);
                        hmobj.heightData[x, y] = ((color.R + color.G + color.B) / 3);
                    }
                }

            }
            //vertexBuffer = new VertexBuffer(gd, typeof(VertexPositionNormalTexture), vertexCount, BufferUsage.None);
            //indexBuffer = new IndexBuffer(gd, typeof(int), indexCount, BufferUsage.None);

            //bmpTexture = new Bitmap(textureFileName);

            //texture = Texture2D.FromStream(gd, new StreamReader(textureFileName).BaseStream);
            //texture.Name = textureFileName;

        }
    }
}
