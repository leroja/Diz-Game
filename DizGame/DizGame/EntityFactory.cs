
using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using GameEngine.Source.Factories;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame
{
    public class EntityFactory
    {
        private ContentManager Content;
        private Dictionary<string, Model> ModelDic;
        private Dictionary<string, Texture2D> Texture2dDic;
        private HeightMapFactory hmFactory;
        public EntityFactory(ContentManager Content, GraphicsDevice Device)
        {

            hmFactory = new HeightMapFactory(Device);
            CreateWorldComp();
            this.Content = Content;
            ModelDic = new Dictionary<string, Model>
            {
                { "Bullet", Content.Load<Model>("Bullet/Bullet") },
                { "Cartridge", Content.Load<Model>("Bullet/Cartridge") },
                { "House_Wood", Content.Load<Model>("House/Farmhouse/medievalHouse1") } ,
                { "House_Stone", Content.Load<Model>("House/WoodHouse/Cyprys_House") } ,
                { "Tree", Content.Load<Model>("House/Tree/lowpolytree") },
                { "Rock", Content.Load<Model>("House/Rock/Rock") },
                { "Dude", Content.Load<Model>("Dude/dude")}
            };

            Texture2dDic = new Dictionary<string, Texture2D>() {
                {"BetterGrass", Content.Load<Texture2D>("HeightMapStuff/BetterGrass") },
                {"canyonHeightMap", Content.Load<Texture2D>("HeightMapStuff/canyonHeightMap")},
                {"test", Content.Load<Texture2D>("HeightMapStuff/test") },
                {"heightmap", Content.Load<Texture2D>("HeightMapStuff/heightmap") }
            };
        }
       
        public void CreateWorldComp()
        {
            var worldEntId = ComponentManager.Instance.CreateID();
            var compList = new List<IComponent>() {
                new WorldComponent(Matrix.Identity)
            };

            ComponentManager.Instance.AddAllComponents(worldEntId, compList);
        }

        public int CreateDude()
        {
            int entityID = ComponentManager.Instance.CreateID();
            Model chuck = ModelDic["Dude"];
            KeyBoardComponent keys = new KeyBoardComponent();
            keys.KeyBoardActions.Add("Forward", Keys.W);
            keys.KeyBoardActions.Add("Backwards", Keys.S);
            keys.KeyBoardActions.Add("Right", Keys.D);
            keys.KeyBoardActions.Add("Left", Keys.A);
            keys.KeyBoardActions.Add("Up", Keys.Space);

            MouseComponent mouse = new MouseComponent();
            mouse.AddActionToButton("Fire", "LeftButton");

            List<IComponent> components = new List<IComponent>
            {
                new TransformComponent(new Vector3(0,45,0), new Vector3(0.135f,0.135f,0.135f)),
                new ModelComponent(chuck),
                keys,
                mouse,
                new PlayerComponent(),
                //                new PhysicsComponent()
                //{
                //    Volume = 22.5f,
                //    Density = 2.66f,
                //    PhysicsType = PhysicsType.Rigid,
                //    MaterialType = MaterialType.Skin,
                //    GravityType = GravityType.World,
                //    DragType = DragType.ManUpright
                //                },
                new TestComponent(),
            };

            ComponentManager.Instance.AddAllComponents(entityID, components);


            TestingTheAnimationsWithDude(entityID);
            return entityID;

        }

        /// <summary>
        /// Creates a house of given model
        /// </summary>
        /// <param name="nameOfModel">name of the model</param>
        /// <param name="position">position on house</param>
        /// <returns></returns>
        public int CreateHouse(string nameOfModel, Vector3 position)
        {
            Vector3 scale = new Vector3();
            Model house = ModelDic[nameOfModel];
            
            if(nameOfModel == "House_Wood")
            {
                scale = new Vector3(0.04f, 0.04f, 0.04f);
            }
            else
            {
                scale = new Vector3(4f, 4f, 4f);
            }
            int entityID = ComponentManager.Instance.CreateID();
            ModelComponent mod = new ModelComponent(house)
            {
                IsStatic = true
            };
            List<IComponent> components = new List<IComponent>
            {
            new TransformComponent(position, scale, Matrix.CreateRotationY(-MathHelper.PiOver2)),
                mod
                
                };
            ComponentManager.Instance.AddAllComponents(entityID, components);

            return entityID;

        }

        public void CreateStaticObject(string nameOfModel, Vector3 position)
        {
            Vector3 scale = new Vector3();
            Model model = ModelDic[nameOfModel];

            switch (nameOfModel)
            {
                case "Rock":
                    scale = new Vector3(5, 5, 5);
                    break;
                case "Tree":
                    scale = new Vector3(5,5,5);
                    break;
            }
            int entityID = ComponentManager.Instance.CreateID();
            ModelComponent comp = new ModelComponent(model)
            {
                IsStatic = true
            };
            List<IComponent> components = new List<IComponent>
            {
            new TransformComponent(position, scale, Matrix.CreateRotationY(-MathHelper.PiOver2)),
                comp
                };
            ComponentManager.Instance.AddAllComponents(entityID, components);
        }
        
        public void MakeMap(int numberOfPlayers,int numberOfStaticObjects)
        {
           List<Vector3> posisions = new List<Vector3>();
            List<Vector3> unablePositions = new List<Vector3>();
           var a =  ComponentManager.Instance.GetAllEntitiesWithComponentType<HeightmapComponentTexture>();
            posisions = GetModelPositions(numberOfPlayers);
           for (int i = 0; i < numberOfPlayers; i ++)
            {
                if (!unablePositions.Contains(posisions[i]))
                {
                    var mod = i % 2;
                    if (mod == 0)
                    {
                        CreateHouse("House_Wood", posisions[i]);
                        unablePositions.Add(posisions[i]);
                    }
                    else
                    {
                        CreateHouse("House_Stone", posisions[i]);
                        unablePositions.Add(posisions[i]);
                    }
                }
                else
                {
                    i--;
                }
            }
           posisions = GetModelPositions(numberOfStaticObjects);
           for (int j = 0; j < numberOfStaticObjects; j++)
            {
                if (!unablePositions.Contains(posisions[j]))
                {
                    var modul = j % 2;
                    switch (modul){
                        case 0:
                            CreateStaticObject("Tree", posisions[j]);
                            break;
                        case 1:
                            CreateStaticObject("Rock", posisions[j]);
                            break; ;

                    }
                }
            }
            
        }
        
        public List<Vector3> GetModelPositions(int numberOfPositions)
        {
            List<Vector3> pos = new List<Vector3>();
            Random r = new Random();
            int mapWidht; ;
            int mapHeight;
            List<int> heightList = ComponentManager.Instance.GetAllEntitiesWithComponentType<HeightmapComponentTexture>();
            HeightmapComponentTexture heigt = ComponentManager.Instance.GetEntityComponent<HeightmapComponentTexture>(heightList[0]);
            mapWidht = heigt.Width;
            mapHeight = heigt.Height;
            for (int i = 0; i < numberOfPositions; i++)
            {
                var pot = new Vector3(r.Next(mapWidht-100), 0,r.Next(mapHeight-100));
                pot.Y = heigt.HeightMapData[(int)pot.X,(int)pot.Z];
                if(pot.X < 100 )
                {
                    pot.X = pot.X + 100;
                }
                if (pot.Z < -100)
                {
                    pot.Z = pot.Z - 100;
                }

                pot.Z = -pot.Z;
                pos.Add(pot);
            }
            return pos;
        }

            

        public void CreateStaticCam(Vector3 CameraPosition, Vector3 lookAt)
        {
            ComponentManager.Instance.AddAllComponents(ComponentManager.Instance.CreateID(), new List<IComponent>() {
                new TransformComponent(CameraPosition, Vector3.One),
                new CameraComponent(CameraType.StaticCam)
                {
                    LookAt = lookAt
                }
            });
        }

        public void AddChaseCamToEntity(int EntityId, Vector3 Offset)
        {
            CameraComponent chaseCam = new CameraComponent(CameraType.Chase)
            {
                Offset = Offset
            };
            ComponentManager.Instance.AddComponentToEntity(EntityId, chaseCam);
        }
        
        // Todo lägg till fysik component, projectile
        public int CreateBullet(string modelName, Vector3 pos, Quaternion Orientation, Vector3 scale, float MaxRange)
        {
            pos = new Vector3(pos.X, pos.Y + 4.5f, pos.Z);
            int BulletEntity = ComponentManager.Instance.CreateID();

            Model model = ModelDic[modelName];
            List<IComponent> componentList = new List<IComponent>()
            {
                new TransformComponent(pos, scale)
                {
                    QuaternionRotation = Orientation
                },
                new  ModelComponent(model),

                
                new BulletComponent(){
                    StartPos = pos,
                    MaxRange = MaxRange,
                },
                new PhysicsComponent()
                {
                    MaterialType = MaterialType.Metal,
                    Bounciness = .1f,
                    Density = .308f,
                    Volume = 0.008f,
                    DragType = DragType.Bullet,
                    IsInAir = true,
                    GravityType = GravityType.World,
                    ReferenceArea = (float)Math.PI * (float)Math.Pow((double)3.5, 2),
                    PhysicsType = PhysicsType.Projectiles,
                    //InitialVelocity = new Vector3(rotation.X,rotation.Y,rotation.Z) * 100,
                },
            };

            ComponentManager.Instance.AddAllComponents(BulletEntity, componentList);

            return BulletEntity;
        }

        public void TestingTheAnimationsWithDude(int entityID)
        {
            
            //Model model = Content.Load<Model>("Dude/dude");
            //int entityID = ComponentManager.Instance.CreateID();
            
            
            //Effect effect = Content.Load<Effect>("Effects/AnimationEffect");

          
            AnimationComponent anm = new AnimationComponent(entityID);

            ComponentManager.Instance.AddComponentToEntity(entityID, anm);

            anm.StartClip("Take 001");

          
        }

        public void CreateHeightMap(string heightmap, string heightTexture)
        {
            int HeightmapEnt = ComponentManager.Instance.CreateID();
            var hmp = hmFactory.CreateTexturedHeightMap(Texture2dDic[heightmap], Texture2dDic[heightTexture], 10);
            List<IComponent> HeightmapCompList = new List<IComponent>
            {
                hmp,
                new TransformComponent(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            };
            ComponentManager.Instance.AddAllComponents(HeightmapEnt, HeightmapCompList);
        }
    }
}
