using AnimationContentClasses;
using AnimationContentClasses.Utils;
using DizGame.Source.LanguageBasedModels;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DizGame.Source.Factories
{
    /// <summary>
    /// A fatcory for creating Static game objects
    /// </summary>
    public class StaticGameObjectsFactory
    {

        private Dictionary<string, Model> _ModelDictionary;
        private Dictionary<string, Texture2D> _Texture2dDictionary;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ModelDic"> A dictionary containing a bunch of models </param>
        /// <param name="textDic"> A dictionary containing a bunch of textures </param>
        public StaticGameObjectsFactory(Dictionary<string, Model> ModelDic, Dictionary<string, Texture2D> textDic)
        {
            this._ModelDictionary = ModelDic;
            this._Texture2dDictionary = textDic;
        }

        /// <summary>
        /// Randomizes a map 
        /// </summary>
        /// <param name="numberOfHouses"> Number of houses </param>
        /// <param name="numberOfStaticObjects"> Number of static objects </param>
        public List<int> MakeMap(int numberOfHouses, int numberOfStaticObjects)
        {
            List<int> entityIdList = new List<int>();

            List<Vector3> positions = new List<Vector3>();
            List<Vector3> unablePositions = new List<Vector3>();
            List<Vector3> rockPositions = new List<Vector3>();

            var a = ComponentManager.Instance.GetAllEntitiesWithComponentType<HeightmapComponentTexture>();
            positions = GetModelPositions(numberOfHouses);
            for (int i = 0; i < numberOfHouses; i++)
            {
                if (!unablePositions.Contains(positions[i]))
                {
                    if (i % 2 == 0)
                    {
                        entityIdList.Add(CreateHouse("WoodHouse", positions[i]));
                        unablePositions.Add(positions[i]);
                    }
                    else
                    {
                        entityIdList.Add(CreateHouse("CyprusHouse", positions[i]));
                        unablePositions.Add(positions[i]);
                    }
                }
            }
            positions = GetModelPositions(numberOfStaticObjects);
            for (int j = 0; j < numberOfStaticObjects; j++)
            {
                if (!unablePositions.Contains(positions[j]))
                {
                    var modul = j % 2;
                    switch (modul)
                    {
                        case 0:
                            entityIdList.Add(CreateStaticObject("Tree", positions[j]));
                            break;
                        case 1:
                            entityIdList.Add(CreateStaticObject("Rock", positions[j]));
                            rockPositions.Add(positions[j]);
                            break;
                    }
                }
            }

            if(rockPositions.Count > 0)
                entityIdList.Add(CreateDryGrass(10, rockPositions[0], new Vector3(0.1f)));
            else
                entityIdList.Add(CreateDryGrass(10, GetModelPositions(1)[0], new Vector3(0.1f)));

            return entityIdList;
        }

        /// <summary>
        /// creates the static objects
        /// </summary>
        /// <param name="nameOfModel"></param>
        /// <param name="position"></param>
        public int CreateStaticObject(string nameOfModel, Vector3 position)
        {
            Vector3 scale = new Vector3();
            Model model = _ModelDictionary[nameOfModel];
            switch (nameOfModel)
            {
                case "Rock":
                    scale = new Vector3(5, 5, 5);
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.TextureEnabled = true;
                            effect.Texture = _Texture2dDictionary["RockTexture"];
                            effect.FogEnabled = true;
                            effect.FogColor = Color.LightGray.ToVector3();
                            effect.FogStart = 10;
                            effect.FogEnd = 400;
                        }
                    }

                    break;
                case "Tree":
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        //scale = new Vector3(5, 5, 5);
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.FogEnabled = true;
                            effect.FogColor = Color.LightGray.ToVector3();
                            effect.FogStart = 10;
                            effect.FogEnd = 400;
                            //effect.EnableDefaultLighting();
                        }
                    }
                    scale = new Vector3(5, 5, 5);
                    break;
            }
            BoundingVolume volume = (BoundingVolume)model.Tag;
            Util.ScaleBoundingVolume(ref volume, scale.X, position, out BoundingVolume scaledVolume);

            int entityID = ComponentManager.Instance.CreateID();
            
            //Vector3 mid = Vector3.Cross(min, max) / 2;
            //BoundingBox box1 = new BoundingBox(position - mid, position + mid);
            ModelComponent comp = new ModelComponent(model)
            {
                IsStatic = true,
                BoundingVolume = scaledVolume
            };
            PhysicsComponent phy = new PhysicsComponent()
            {
                PhysicsType = PhysicsType.Static
            };
            List<IComponent> components = new List<IComponent>
            {
                new TransformComponent(position, scale),
                comp,
                phy
            };
            ComponentManager.Instance.AddAllComponents(entityID, components);

            return entityID;
        }

        /// <summary>
        /// Creates a house of given model
        /// </summary>
        /// <param name="nameOfModel"> Name of the house model </param>
        /// <param name="position"> position of the  house </param>
        /// <returns></returns>
        public int CreateHouse(string nameOfModel, Vector3 position)
        {
            Vector3 scale = new Vector3();
            Model house = _ModelDictionary[nameOfModel];

            if (nameOfModel == "WoodHouse")
            {
                scale = Vector3.One;
            }
            else
            {
                scale = new Vector3(4f, 4f, 4f);
            }
            foreach (ModelMesh mesh in house.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.FogEnabled = true;
                    effect.FogColor = Color.LightGray.ToVector3();
                    effect.FogStart = 10;
                    effect.FogEnd = 400;
                }
            }
            BoundingVolume volume = (BoundingVolume)house.Tag;
            Util.ScaleBoundingVolume(ref volume, scale.X, position, out BoundingVolume scaledVolume);

            int entityID = ComponentManager.Instance.CreateID();

            ModelComponent mod = new ModelComponent(house)
            {
                IsStatic = true,
                BoundingVolume = scaledVolume
            };
            PhysicsComponent phy = new PhysicsComponent()
            {
                PhysicsType = PhysicsType.Static,
            };
            List<IComponent> components = new List<IComponent>
            {
            new TransformComponent(position, scale),
                mod,
                phy
                };
            ComponentManager.Instance.AddAllComponents(entityID, components);

            return entityID;
        }


        public int CreateDryGrass(int numberOfObjects, Vector3 position, Vector3 scale)
        {
            TreeModel tree = new TreeModel(GameOne.Instance.GraphicsDevice, 1f, MathHelper.PiOver4 - 0.5f, "F[LF]F[RF]F", 4, 1f, new string[] { "F" });
            int instanceCount = numberOfObjects;

            VertexDeclaration matriceVD = new VertexDeclaration(
                                     new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 0),
                                     new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1),
                                     new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 2),
                                     new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 3)
                                     );

            Matrix[] objectWorldMatrices = new Matrix[instanceCount];
            InitObjectMatrices(ref objectWorldMatrices, position, scale);
            //SpreadTheGrassAroundPosition(ref objectWorldMatrices, position, 2);

            VertexBuffer matriceVB = new VertexBuffer(GameOne.Instance.GraphicsDevice, matriceVD, instanceCount, BufferUsage.None);
            matriceVB.SetData(objectWorldMatrices);

            VertexBufferBinding[] bindings = new VertexBufferBinding[2];

            bindings[0] = new VertexBufferBinding(tree.VertexBuffer);
            bindings[1] = new VertexBufferBinding(matriceVB, 0, 1);

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            rs.FillMode = FillMode.Solid;

            HardwareInstancedComponent hwinstanced = new HardwareInstancedComponent()
            {
                GraphicsDevice = GameOne.Instance.GraphicsDevice,

                Effect = GameOne.Instance.Content.Load<Effect>("Effects/HardwareInstancedEffect"),

                IndexBuffer = tree.IndexBuffer,
                VertexBuffer = tree.VertexBuffer,

                IndicesPerPrimitive = 2,
                InstanceCount = instanceCount,

                MatriceVB = matriceVB,
                Bindings = bindings,
                MatriceVD = matriceVD,
                ObjectWorldMatrices = objectWorldMatrices,

                Texture = GameOne.Instance.Content.Load<Texture2D>("HeightMapStuff/drygrass"),

                RasterizerState = rs,

            };

            TransformComponent transform = new TransformComponent(position, scale);

            int entityID = ComponentManager.Instance.CreateID();

            ComponentManager.Instance.AddComponentToEntity(entityID, hwinstanced);
            ComponentManager.Instance.AddComponentToEntity(entityID, transform);

            return entityID;
        }


        private void InitObjectMatrices(ref Matrix[] objectMatrices, Vector3 position, Vector3 scale)
        {
            for (int i = 0; i < objectMatrices.Length; i++)
                objectMatrices[i] = Matrix.Identity
                                  * Matrix.CreateScale(scale)
                                  * Matrix.CreateRotationY(i * MathHelper.PiOver4)
                                  * Matrix.CreateTranslation(position);
        }

        private void SpreadTheGrassAroundPosition(ref Matrix[] objectMatrices, Vector3 position, int spread)
        {
            Random rnd = new Random();

            for(int i=0; i<objectMatrices.Length; i++)
            {
                objectMatrices[i] *= Matrix.CreateTranslation(
                    new Vector3(rnd.Next(-spread, spread),
                                0,
                                rnd.Next(-spread, spread)));


            }
        }

        /// <summary>
        /// Gets target number of potitions on heightmap
        /// </summary>
        /// <param name="numberOfPositions"></param>
        /// <returns></returns>
        private List<Vector3> GetModelPositions(int numberOfPositions)
        {
            List<Vector3> positions = new List<Vector3>();
            Random r = new Random();
            int mapWidht;
            int mapHeight;

            List<Vector3> SpawnProtection = new List<Vector3>();
            List<int> heightList = ComponentManager.Instance.GetAllEntitiesWithComponentType<HeightmapComponentTexture>();
            HeightmapComponentTexture heigt = ComponentManager.Instance.GetEntityComponent<HeightmapComponentTexture>(heightList[0]);

            mapWidht = heigt.Width;
            mapHeight = heigt.Height;
            for (int i = 0; i < numberOfPositions; i++)
            {
                var pot = new Vector3(r.Next(mapWidht - 20), 0, r.Next(mapHeight - 20));

                pot.Y = heigt.HeightMapData[(int)pot.X, (int)pot.Z];
                if (pot.X < 10)
                {
                    pot.X = pot.X + 10;
                }
                if (pot.Z < 10)
                {
                    pot.Z = pot.Z - 10;
                }
                pot.Z = -pot.Z;
                positions.Add(pot);
            }
            return positions;
        }
    }
}
