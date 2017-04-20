using GameEngine.Source.Systems.Abstract_classes;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;

using Microsoft.Xna.Framework.Graphics;
using GameEngine.Source.Managers;
using GameEngine.Source.Objects;

namespace GameEngine.Source.Systems
{
    public class HeightMapSystem : IUpdate
    {

        private List<IComponent> heightmapComponents;
        private List<HeightMapObject> hmobjects;

        public HeightMapSystem(ref List<HeightMapObject> hmobjects)
        {
            this.hmobjects = hmobjects;
            heightmapComponents = new List<IComponent>();



            CreateHeightmapComponents();

            SetUpVertices();
            SetUpIndices();

            SetUpNormals();

            //SetData();
        }

        //To be used when we have decided where and when...
        //private void SetData()
        //{
        //    foreach (HeightMapComponent cmp in heightmapComponents)
        //    {
        //        cmp.vertexBuffer.SetData<VertexPositionNormalTexture>(cmp.vertices);
        //        cmp.indexBuffer.SetData<int>(cmp.indices);
        //    }
        //}


        private void CreateHeightmapComponents()
        {
            int index = 0;

            foreach (HeightMapObject hmobj in hmobjects)
            {

                int entityId = ComponentManager.Instance.CreateID();

                HeightMapComponent cmp = new HeightMapComponent()
                {
                    terrainMapName = hmobj.terrainMapName,
                    terrainHeight = hmobj.terrainHeight,
                    terrainWidth = hmobj.terrainWidth,
                    scaleFactor = hmobj.scaleFactor
                };

                initBuffers(cmp);

                hmobjects[index].entityId = entityId;
                ComponentManager.Instance.AddComponentToEntity(entityId, cmp);
            }
        }

        private void initBuffers(HeightMapComponent cmp)
        {
            int vertexCount = cmp.terrainWidth * cmp.terrainHeight;
            int indexCount = (cmp.terrainWidth - 1) * (cmp.terrainHeight - 1) * 6;

            cmp.vertices = new VertexPositionNormalTexture[vertexCount];
            cmp.indices = new int[indexCount];

            //to be set when we have decided where and when...
            //vertexBuffer = new VertexBuffer(gd, typeof(VertexPositionNormalTexture), vertexCount, BufferUsage.None);
            //indexBuffer = new IndexBuffer(gd, typeof(int), indexCount, BufferUsage.None);
        }

        private void SetUpVertices()
        {
            Random rnd = new Random();
            int index = 0;



            foreach (HeightMapObject hmobj in hmobjects)
            {
                HeightMapComponent cmp = ComponentManager.Instance.GetEntityComponent<HeightMapComponent>(hmobj.entityId);

                for (int x = 0; x < cmp.terrainWidth; x++)
                {
                    for (int y = 0; y < cmp.terrainHeight; y++)
                    {
                        index = x + y * cmp.terrainWidth;

                        cmp.vertices[index].Position =
                            new Vector3(x, hmobj.heightData[x, y], -y);

                        cmp.vertices[index].Position = Vector3.Transform(cmp.vertices[index].Position,
                                                                         Matrix.CreateScale(cmp.scaleFactor));

                        cmp.vertices[index].Normal = new Vector3(rnd.Next(0, 101) / 100f, rnd.Next(0, 101) / 100f, rnd.Next(0, 101) / 100f);
                        cmp.vertices[index].TextureCoordinate = new Vector2(0, 0);
                    }
                }

                index = 0;
            }
        }

        private void SetUpNormals()
        {

            Vector3 v1 = Vector3.Zero;
            Vector3 v2 = Vector3.Zero;

            foreach (HeightMapComponent cmp in heightmapComponents)
                for (int y = 0; y < cmp.terrainHeight - 1; y++)
                {
                    for (int x = 0; x < cmp.terrainWidth - 1; x++)
                    {
                        int lowerLeft = x + y * cmp.terrainWidth;
                        int lowerRight = (x + 1) + y * cmp.terrainWidth;
                        int topLeft = x + (y + 1) * cmp.terrainWidth;
                        int topRight = (x + 1) + (y + 1) * cmp.terrainWidth;

                        v1 = Vector3.Cross(cmp.vertices[topLeft].Position, cmp.vertices[lowerLeft].Position);


                        v2 = Vector3.Cross(cmp.vertices[topRight].Position, cmp.vertices[lowerRight].Position);

                        cmp.vertices[lowerLeft].Normal = Vector3.Normalize(Vector3.Add(v1, cmp.vertices[lowerRight].Normal));
                        cmp.vertices[topRight].Normal = Vector3.Normalize(Vector3.Add(v2, cmp.vertices[lowerLeft].Normal));

                    }
                }
        }


        private void SetUpIndices()
        {

            int counter = 0;

            foreach (HeightMapComponent cmp in heightmapComponents)
            {
                for (int y = 0; y < cmp.terrainHeight - 1; y++)
                {
                    for (int x = 0; x < cmp.terrainWidth - 1; x++)
                    {
                        int lowerLeft = x + y * cmp.terrainWidth;
                        int lowerRight = (x + 1) + y * cmp.terrainWidth;
                        int topLeft = x + (y + 1) * cmp.terrainWidth;
                        int topRight = (x + 1) + (y + 1) * cmp.terrainWidth;

                        cmp.indices[counter++] = topLeft;
                        cmp.indices[counter++] = lowerRight;
                        cmp.indices[counter++] = lowerLeft;

                        cmp.indices[counter++] = topLeft;
                        cmp.indices[counter++] = topRight;
                        cmp.indices[counter++] = lowerRight;
                    }
                }

                counter = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        //To be set and tested before use.
        //private void LoadHeightData()
        //{
        //    foreach (HeightMapComponent cmp in heightmapComponents)
        //        for (int x = 0; x < cmp.terrainWidth; x++)
        //        {
        //            for (int y = 0; y < cmp.terrainHeight; y++)
        //            {
        //                //System.Drawing.Color color = cmp.bmpHeightdata.GetPixel(x, y);
        //                //cmp.heightData[x, y] = ((color.R + color.G + color.B) / 3);
        //            }
        //        }

        //}

    }
}
