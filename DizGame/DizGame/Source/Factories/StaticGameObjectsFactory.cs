using ContentProject;
using ContentProject.Utils;
using DizGame.Source.Random_Stuff;
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
    /// A factory for creating Static game objects
    /// </summary>
    public class StaticGameObjectsFactory
    {
        private Border border;
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
        /// <param name="border">  </param>
        public List<int> MakeMap(int numberOfHouses, int numberOfStaticObjects, Border border)
        {
            this.border = border;
            List<int> entityIdList = new List<int>();

            List<Vector3> positions = new List<Vector3>();
            List<Vector3> unablePositions = new List<Vector3>();

            var a = ComponentManager.Instance.GetAllEntitiesWithComponentType<HeightmapComponent>();
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
        /// Creates a static game object
        /// </summary>
        /// <param name="nameOfModel"> The name of the objects model </param>
        /// <param name="position"> The position of the object </param>
        /// <returns> The entityID of the object </returns>
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
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.FogEnabled = true;
                            effect.FogColor = Color.LightGray.ToVector3();
                            effect.FogStart = 10;
                            effect.FogEnd = 400;
                        }
                    }
                    scale = new Vector3(5, 5, 5);
                    break;
            }
            BoundingVolume volume = (BoundingVolume)model.Tag;
            Util.ScaleBoundingVolume(ref volume, scale.X, position, out BoundingVolume scaledVolume);

            int entityID = ComponentManager.Instance.CreateID();

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
        /// <param name="position"> Position of the  house </param>
        /// <returns> The entityID of the house </returns>
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

        /// <summary>
        /// Gets target number of positions on heightmap
        /// </summary>
        /// <param name="numberOfPositions">  </param>
        /// <returns>  </returns>
        private List<Vector3> GetModelPositions(int numberOfPositions)
        {
            List<Vector3> positions = new List<Vector3>();
            Random r = new Random();

            List<int> heightList = ComponentManager.Instance.GetAllEntitiesWithComponentType<HeightmapComponent>();
            HeightmapComponent heigt = ComponentManager.Instance.GetEntityComponent<HeightmapComponent>(heightList[0]);
            for (int i = 0; i < numberOfPositions; i++)
            {
                var pot = new Vector3(r.Next((int)border.LowX, (int)border.HighX), 0, r.Next(Math.Abs((int)border.LowZ), Math.Abs((int)border.HighZ)));
                pot.Y = heigt.HeightMapData[(int)pot.X, (int)pot.Z];
                pot.Z = -pot.Z;
                positions.Add(pot);
            }
            return positions;
        }
    }
}