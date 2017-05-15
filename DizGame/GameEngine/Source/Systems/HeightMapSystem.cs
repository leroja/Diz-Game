using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;

using Microsoft.Xna.Framework.Graphics;
using GameEngine.Source.Managers;
using GameEngine.Source.Objects;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

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

            RasterizerState rState = new RasterizerState()
            {
                CullMode = CullMode.None,
                FillMode = FillMode.Solid
            };
            game.GraphicsDevice.BlendState = BlendState.Opaque;
            game.GraphicsDevice.RasterizerState = rState;
        }

        //To be used when we have decided where and when...
        private void SetData()
        {
            foreach (HeightMapComponent cmp in ComponentManager.GetAllEntitiesAndComponentsWithComponentType<HeightMapComponent>().Values)
            {

                cmp.VertexBuffer.SetData<VertexPositionNormalTexture>(cmp.Vertices);

                cmp.IndexBuffer.SetData<int>(cmp.Indices);
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
                    TerrainMapName = hmobj.TerrainMapName,
                    TerrainHeight = hmobj.TerrainHeight,
                    TerrainWidth = hmobj.TerrainWidth,
                    //scaleFactor = hmobj.scaleFactor,
                    HeightData = hmobj.HeightData,
                };

                hmobjects[index++].EntityId = entityId;

                ComponentManager.Instance.AddComponentToEntity(entityId, cmp);

                ComponentManager.AddComponentToEntity(entityId, hmobj.Transform);
            }
        }


        private void InitBuffers()
        {
            foreach (HeightMapComponent cmp in ComponentManager.GetAllEntitiesAndComponentsWithComponentType<HeightMapComponent>().Values)
            {
                int vertexCount = cmp.TerrainWidth * cmp.TerrainHeight;
                int indexCount = (cmp.TerrainWidth - 1) * (cmp.TerrainHeight - 1) * 6;

                cmp.Vertices = new VertexPositionNormalTexture[vertexCount];
                cmp.Indices = new int[indexCount];

                cmp.VertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionNormalTexture), vertexCount, BufferUsage.None);
                cmp.IndexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(int), indexCount, BufferUsage.None);
            }
        }


        private void SetUpVertices()
        {

            foreach (HeightMapComponent cmp in ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<HeightMapComponent>().Values)
                Parallel.For(0, cmp.TerrainWidth, setupVeticesTerrainHeight);

        }

        private void setupVeticesTerrainHeight(int x)
        {
            HeightMapComponent cmp = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<HeightMapComponent>().Values.Cast<HeightMapComponent>().ElementAt(0);
            Random rnd = new Random();
            int index = 0;
            for (int y = 0; y < cmp.TerrainHeight; y++)
            {
                index = x + y * cmp.TerrainWidth;

                cmp.Vertices[index].Position =
                    new Vector3(x, cmp.HeightData[x, y], -y);

                //cmp.vertices[index].Position = Vector3.Transform(cmp.vertices[index].Position,
                //                                                 Matrix.CreateScale(1f));

                cmp.Vertices[index].Normal = new Vector3(rnd.Next(0, 101) / 100f, rnd.Next(0, 101) / 100f, rnd.Next(0, 101) / 100f);
                cmp.Vertices[index].TextureCoordinate = new Vector2(1, 1);
            }
        }

        private void SetUpNormals()
        {

            Vector3 v1 = Vector3.Zero;
            Vector3 v2 = Vector3.Zero;

            foreach (HeightMapComponent cmp in ComponentManager.GetAllEntitiesAndComponentsWithComponentType<HeightMapComponent>().Values)
                for (int y = 0; y < cmp.TerrainHeight - 1; y++)
                {
                    for (int x = 0; x < cmp.TerrainWidth - 1; x++)
                    {
                        int lowerLeft = x + y * cmp.TerrainWidth;
                        int lowerRight = (x + 1) + y * cmp.TerrainWidth;
                        int topLeft = x + (y + 1) * cmp.TerrainWidth;
                        int topRight = (x + 1) + (y + 1) * cmp.TerrainWidth;

                        v1 = Vector3.Cross(cmp.Vertices[topLeft].Position, cmp.Vertices[lowerLeft].Position);


                        v2 = Vector3.Cross(cmp.Vertices[topRight].Position, cmp.Vertices[lowerRight].Position);

                        cmp.Vertices[lowerLeft].Normal = Vector3.Normalize(Vector3.Add(v1, cmp.Vertices[lowerRight].Normal));
                        cmp.Vertices[topRight].Normal = Vector3.Normalize(Vector3.Add(v2, cmp.Vertices[lowerLeft].Normal));

                    }
                }
        }


        private void SetUpIndices()
        {

            int counter = 0;

            foreach (HeightMapComponent cmp in ComponentManager.GetAllEntitiesAndComponentsWithComponentType<HeightMapComponent>().Values)
            {
                for (int y = 0; y < cmp.TerrainHeight - 1; y++)
                {
                    for (int x = 0; x < cmp.TerrainWidth - 1; x++)
                    {
                        int lowerLeft = x + y * cmp.TerrainWidth;
                        int lowerRight = (x + 1) + y * cmp.TerrainWidth;
                        int topLeft = x + (y + 1) * cmp.TerrainWidth;
                        int topRight = (x + 1) + (y + 1) * cmp.TerrainWidth;

                        cmp.Indices[counter++] = topLeft;
                        cmp.Indices[counter++] = lowerRight;
                        cmp.Indices[counter++] = lowerLeft;

                        cmp.Indices[counter++] = topLeft;
                        cmp.Indices[counter++] = topRight;
                        cmp.Indices[counter++] = lowerRight;
                    }
                }

                counter = 0;
            }
        }

        private void SetEffectParameters()
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

        private void SetTransformParameters(TransformComponent transform)
        {
            effect.World = world.World
                         * Matrix.CreateTranslation(transform.Position)
                         * Matrix.CreateScale(transform.Scale);
        }

        public override void Draw(GameTime gameTime)
        {
            SetEffectParameters();

            foreach (HeightMapComponent cmp in ComponentManager.GetAllEntitiesAndComponentsWithComponentType<HeightMapComponent>().Values)
            {
                SetTransformParameters(ComponentManager.GetEntityComponent<TransformComponent>(cmp.ID));

                game.GraphicsDevice.SetVertexBuffer(cmp.VertexBuffer);
                game.GraphicsDevice.Indices = cmp.IndexBuffer;

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    
                    game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, cmp.IndexBuffer.IndexCount / 3);
                }
            }
        }
    }
}
