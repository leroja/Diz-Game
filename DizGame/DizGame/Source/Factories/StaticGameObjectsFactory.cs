using AnimationContentClasses;
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
                            break;
                    }
                }
            }
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
            int entityID = ComponentManager.Instance.CreateID();

            GetMinMax(((BoundingBox3D)volume.Bounding).Box, scale.X, position, out Vector3 min, out Vector3 max);
            //Vector3 mid = Vector3.Cross(min, max) / 2;
            //BoundingBox box1 = new BoundingBox(position - mid, position + mid);
            BoundingBox box = new BoundingBox(min, max);
            ModelComponent comp = new ModelComponent(model)
            {
                IsStatic = true,
                BoundingVolume = new BoundingVolume(entityID, new BoundingBox3D(box))
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
                scale = new Vector3(0.04f, 0.04f, 0.04f);
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
            int entityID = ComponentManager.Instance.CreateID();
            GetMinMax(((BoundingBox3D)volume.Bounding).Box, scale.X, position, out Vector3 min, out Vector3 max);
            BoundingBox box = new BoundingBox(min, max);
            ModelComponent mod = new ModelComponent(house)
            {
                IsStatic = true,
                BoundingVolume = new BoundingVolume(entityID, new BoundingBox3D(box))
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
        
        /// <summary>
        /// Gets the right min and max vector3 for the models boundingbox
        /// </summary>
        /// <param name="box"></param>
        /// <param name="scale"></param>
        /// <param name="position"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        private void GetMinMax(BoundingBox box, float scale, Vector3 position, out Vector3 min, out Vector3 max)
        {
            min = box.Min * scale;
            max = box.Max * scale;
            float xDelta = (max - min).X;
            float zDelta = (max - min).Z;
            float yDelta = (max - min).Y;
            min.Y = position.Y;
            min.X = position.X - xDelta / 2;
            min.Z = position.Z - zDelta / 2;
            max.Y = position.Y + yDelta;
            max.X = position.X + xDelta / 2;
            max.Z = position.Z + zDelta / 2;
        }
    }
}
