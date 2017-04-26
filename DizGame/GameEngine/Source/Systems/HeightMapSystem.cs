using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;

using Microsoft.Xna.Framework.Graphics;
using GameEngine.Source.Managers;
using GameEngine.Source.Objects;
using System.Linq;
using System.IO;

namespace GameEngine.Source.Systems
{
    public class HeightMapSystem : IRender
    {

        private List<IComponent> heightmapComponents;
        private List<HeightMapObject> hmobjects;
        private Game game;
        private WorldComponent world;
        private CameraComponent defaultCam;
        private Texture2D texture;

        private BasicEffect effect;

        public HeightMapSystem(Game game, ref List<HeightMapObject> hmobjects)
        {
            this.game = game;
            this.hmobjects = hmobjects;
            heightmapComponents = new List<IComponent>();

            List<int> temp = ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>();
            world = ComponentManager.GetEntityComponent<WorldComponent>(temp.First());

            List<int> entitiesWithCamera = ComponentManager.GetAllEntitiesWithComponentType<CameraComponent>();
            defaultCam = ComponentManager.GetEntityComponent<CameraComponent>(entitiesWithCamera.First());


            CreateHeightmapComponents();

            InitBuffers();

            SetUpVertices();
            SetUpIndices();

            SetUpNormals();

            SetData();

            texture = Texture2D.FromStream(game.GraphicsDevice, new StreamReader("Content/HeightMaps/grass.png").BaseStream);

            effect = new BasicEffect(game.GraphicsDevice);

            RasterizerState rState = new RasterizerState();
            rState.CullMode = CullMode.None;
            rState.FillMode = FillMode.Solid;
            game.GraphicsDevice.BlendState = BlendState.Opaque;
            game.GraphicsDevice.RasterizerState = rState;
        }

        //To be used when we have decided where and when...
        private void SetData()
        {
            foreach (HeightMapComponent cmp in ComponentManager.GetAllEntitiesAndComponentsWithComponentType<HeightMapComponent>().Values)
            {

                cmp.vertexBuffer.SetData<VertexPositionNormalTexture>(cmp.vertices);

                cmp.indexBuffer.SetData<int>(cmp.indices);
            }
        }


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
                    //scaleFactor = hmobj.scaleFactor,
                    heightData = hmobj.heightData,
                };

                hmobjects[index++].entityId = entityId;

                ComponentManager.Instance.AddComponentToEntity(entityId, cmp);

                ComponentManager.AddComponentToEntity(entityId, hmobj.transform);
            }
        }


        private void InitBuffers()
        {
            foreach (HeightMapComponent cmp in ComponentManager.GetAllEntitiesAndComponentsWithComponentType<HeightMapComponent>().Values)
            {
                int vertexCount = cmp.terrainWidth * cmp.terrainHeight;
                int indexCount = (cmp.terrainWidth - 1) * (cmp.terrainHeight - 1) * 6;

                cmp.vertices = new VertexPositionNormalTexture[vertexCount];
                cmp.indices = new int[indexCount];

                cmp.vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionNormalTexture), vertexCount, BufferUsage.None);
                cmp.indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(int), indexCount, BufferUsage.None);
            }
        }


        private void SetUpVertices()
        {
            Random rnd = new Random();
            int index = 0;

            foreach (HeightMapComponent cmp in ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<HeightMapComponent>().Values)
            {
                for (int x = 0; x < cmp.terrainWidth; x++)
                {
                    for (int y = 0; y < cmp.terrainHeight; y++)
                    {
                        index = x + y * cmp.terrainWidth;

                        cmp.vertices[index].Position =
                            new Vector3(x, cmp.heightData[x, y], -y);

                        //cmp.vertices[index].Position = Vector3.Transform(cmp.vertices[index].Position,
                        //                                                 Matrix.CreateScale(1f));

                        cmp.vertices[index].Normal = new Vector3(rnd.Next(0, 101) / 100f, rnd.Next(0, 101) / 100f, rnd.Next(0, 101) / 100f);
                        cmp.vertices[index].TextureCoordinate = new Vector2(1, 1);
                    }
                }

                index = 0;
            }
        }

        private void SetUpNormals()
        {

            Vector3 v1 = Vector3.Zero;
            Vector3 v2 = Vector3.Zero;

            foreach (HeightMapComponent cmp in ComponentManager.GetAllEntitiesAndComponentsWithComponentType<HeightMapComponent>().Values)
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

            foreach (HeightMapComponent cmp in ComponentManager.GetAllEntitiesAndComponentsWithComponentType<HeightMapComponent>().Values)
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

        private void setEffectParameters()
        {
            effect.World = world.World; 
            effect.View = defaultCam.View;
            effect.Projection = defaultCam.Projection;
            effect.EnableDefaultLighting();
            effect.PreferPerPixelLighting = true;
            effect.TextureEnabled = true;
            effect.LightingEnabled = true;

            effect.Texture = texture;
        }

        private void setTransformParameters(TransformComponent transform)
        {
            effect.World = world.World
                         * Matrix.CreateTranslation(transform.Position)
                         * Matrix.CreateScale(transform.Scale);
        }

        public override void Draw(GameTime gameTime)
        {
            setEffectParameters();

            foreach (HeightMapComponent cmp in ComponentManager.GetAllEntitiesAndComponentsWithComponentType<HeightMapComponent>().Values)
            {
                setTransformParameters(ComponentManager.GetEntityComponent<TransformComponent>(cmp.ID));

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    game.GraphicsDevice.SetVertexBuffer(cmp.vertexBuffer);
                    game.GraphicsDevice.Indices = cmp.indexBuffer;

                    game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, cmp.indexBuffer.IndexCount / 3);
                }
            }
        }
    }
}
