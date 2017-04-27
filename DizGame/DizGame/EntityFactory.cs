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
                { "House_Stone", Content.Load<Model>("House/WoodHouse/Cyprys_House") }
                { "Cartridge", Content.Load<Model>("Bullet/Cartridge") },
                { "Chuck", Content.Load<Model>("chuck/DR3_Chuck_Greene") },
                { "Kitana", Content.Load<Model>("Kitana/Kitana") }
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

        public int CreateChuckGreen()
        {
            int entityID = ComponentManager.Instance.CreateID();
            Model chuck = ModelDic["Chuck"];
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
                new TransformComponent(new Vector3(0,45,0), new Vector3(0.135f,0.135f,0.135f), Matrix.CreateRotationY(-MathHelper.PiOver2)),
                new ModelComponent(chuck),
                keys,
                mouse,
                new PlayerComponent(),
            };

            
            ComponentManager.Instance.AddAllComponents(entityID, components);

            return entityID;

        }

        public int CreateKitana()
        {
            int entityID = ComponentManager.Instance.CreateID();
            Model kitana = ModelDic["Kitana"];

            KeyBoardComponent keys = new KeyBoardComponent();
            keys.KeyBoardActions.Add("Forward", Keys.Up);
            keys.KeyBoardActions.Add("Backwards", Keys.Down);
            keys.KeyBoardActions.Add("Right", Keys.Right);
            keys.KeyBoardActions.Add("Left", Keys.Left);
            keys.KeyBoardActions.Add("Up", Keys.Space);


            List<IComponent> components = new List<IComponent>
            {
                new TransformComponent(new Vector3(5,0,-20), new Vector3(0.05f,0.05f,0.05f), Matrix.CreateRotationY(-MathHelper.PiOver2)),
                new ModelComponent(kitana),
                //new PhysicsComponent()
                //{
                //    Mass = 60f,
                //    PhysicsType = PhysicsType.Rigid,
                //    MaterialType = MaterialType.Skin,
                //    GravityType = GravityType.World,
                //    DragType = DragType.ManUpright
                //},
                keys,
            };


            ComponentManager.Instance.AddAllComponents(entityID, components);

            return entityID;
        }

        public int createHouse(string nameOfModel, Vector3 position)
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

            List<IComponent> components = new List<IComponent>
            {
            new TransformComponent(position, scale, Matrix.CreateRotationY(-MathHelper.PiOver2)),
                new ModelComponent(house),
                };
            ComponentManager.Instance.AddAllComponents(entityID, components);

            return entityID;

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

        // roation är inte riktigt det jag vill, oriantaion är nog mer det jag vill ha
        public int CreateBullet(string modelName, Vector3 pos, Quaternion rotation, Vector3 scale, float MaxRange)
        {
            int BulletEntity = ComponentManager.Instance.CreateID();
            //var mat = Matrix.CreateFromQuaternion(rotation);

            Model model = ModelDic[modelName];
            List<IComponent> componentList = new List<IComponent>()
            {
                new TransformComponent(pos, scale)
                {
                    QuaternionRotation = rotation
                    //Rotation = rotation
                },
                new  ModelComponent(model),

                //temp
                new MouseComponent(){
                    //MouseSensitivity = 1.9f
                },
                new BulletComponent(){
                    StartPos = pos,
                    MaxRange = MaxRange,
                },
            };

            ComponentManager.Instance.AddAllComponents(BulletEntity, componentList);

            return BulletEntity;
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
