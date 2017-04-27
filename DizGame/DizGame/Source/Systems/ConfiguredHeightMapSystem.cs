using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Systems;
using GameEngine;
using GameEngine.Source.Objects;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;

namespace DizGame.Source.ConfiguredSystems
{
    public class ConfiguredHeightMapSystem : IRender
    {

        public static HeightMapSystem HeightMapSystem { get; private set; }
        private Game game;

        public ConfiguredHeightMapSystem(Game game)
        {
            this.game = game;

            List<HeightMapObject> hmobj = GenerateHeighMapObjects;

            HeightMapSystem = new HeightMapSystem(game, ref hmobj);

        }

        public void ThisFunctionDoesNothing()
        {

        }

        private List<HeightMapObject> GenerateHeighMapObjects
        {
            get
            {
                List<HeightMapObject> hmobjs = new List<HeightMapObject>();


                Vector3 hmPos1 = new Vector3(-500, -400, 500);
                Vector3 hmScale1 = new Vector3(0.1f);
                TransformComponent transform = new TransformComponent(hmPos1, hmScale1);

                HeightMapObject hmobj1 = new HeightMapObject()
                {

                    //scaleFactor = 0.01f,
                    TerrainMapName = "Content/HeightMaps/heightmap.png",
                    Transform = transform,
                };





                hmobjs.Add(hmobj1);

                InitialiseHeightMapObject(hmobjs);

                return hmobjs;
            }
        }

        private void InitialiseHeightMapObject(List<HeightMapObject> hmobjs)
        {

            foreach (HeightMapObject hmobj in hmobjs)
            {
                Bitmap bmpHeightdata = new Bitmap(hmobj.TerrainMapName);
                hmobj.TerrainHeight = bmpHeightdata.Height;
                hmobj.TerrainWidth = bmpHeightdata.Width;

                //vertexCount = terrainWidth * terrainHeight;
                //indexCount = (terrainWidth - 1) * (terrainHeight - 1) * 6;

                //vertices = new VertexPositionNormalTexture[vertexCount];
                //indices = new int[indexCount];

                hmobj.HeightData = new float[hmobj.TerrainWidth, hmobj.TerrainHeight];

                for (int x = 0; x < hmobj.TerrainWidth; x++)
                {
                    for (int y = 0; y < hmobj.TerrainHeight; y++)
                    {
                        System.Drawing.Color color = bmpHeightdata.GetPixel(x, y);
                        hmobj.HeightData[x, y] = ((color.R + color.G + color.B) / 3);
                    }
                }

            }
            //vertexBuffer = new VertexBuffer(gd, typeof(VertexPositionNormalTexture), vertexCount, BufferUsage.None);
            //indexBuffer = new IndexBuffer(gd, typeof(int), indexCount, BufferUsage.None);

            //bmpTexture = new Bitmap(textureFileName);

            //texture = Texture2D.FromStream(gd, new StreamReader(textureFileName).BaseStream);
            //texture.Name = textureFileName;

        }

        public override void Draw(GameTime gameTime)
        {
            HeightMapSystem.Draw(gameTime);
        }
    }
}
