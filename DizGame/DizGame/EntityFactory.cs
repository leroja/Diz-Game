
using DizGame.Source.Components;
using DizGame.Source.Enums;
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
    /// <summary>
    /// Factory for creating various entities which might be used by the game in the end.
    /// </summary>
    public class EntityFactory
    {

        private static EntityFactory instance;

        private ContentManager Content;
        private Dictionary<string, Model> ModelDic;
        private Dictionary<string, Texture2D> Texture2dDic;
        private HeightMapFactory hmFactory;
        public bool VisibleBullets { get; set; }

        /// <summary>
        /// The instance of the Entity Factory
        /// </summary>
        public static EntityFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityFactory();
                }
                return instance;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private EntityFactory()
        {
            VisibleBullets = true;
            hmFactory = new HeightMapFactory(GameOne.Instance.GraphicsDevice);
            CreateWorldComp();
            this.Content = GameOne.Instance.Content;
            ModelDic = new Dictionary<string, Model>
            {
                { "Bullet", Content.Load<Model>("Bullet/Bullet") },
                { "Cartridge", Content.Load<Model>("Bullet/Cartridge") },
                { "House_Wood", Content.Load<Model>("MapObjects/Farmhouse/medievalHouse1") } ,
                { "House_Stone", Content.Load<Model>("MapObjects/WoodHouse/Cyprys_House") } ,
                { "Tree", Content.Load<Model>("MapObjects/Tree/lowpolytree") },
                { "Rock", Content.Load<Model>("MapObjects/Rock/Rock") },
                { "Dude", Content.Load<Model>("Dude/dude")},
            };

            Texture2dDic = new Dictionary<string, Texture2D>() {
                {"BetterGrass", Content.Load<Texture2D>("HeightMapStuff/BetterGrass") },
                //{"canyonHeightMap", Content.Load<Texture2D>("HeightMapStuff/canyonHeightMap")},
                {"canyonHeightMap", Content.Load<Texture2D>("HeightMapStuff/Map3")},
                {"heightmap", Content.Load<Texture2D>("HeightMapStuff/heightmap") },
                {"RockTexture", Content.Load<Texture2D>("MapObjects/Rock/Stone Texture") }
            };
        }
       
        /// <summary>
        /// 
        /// </summary>
        public void CreateWorldComp()
        {
            var worldEntId = ComponentManager.Instance.CreateID();
            var compList = new List<IComponent>() {
                new WorldComponent(Matrix.Identity)
            };

            ComponentManager.Instance.AddAllComponents(worldEntId, compList);
        }
        /// <summary>
        /// Function to add the entity which might be the model for the different players
        /// </summary>
        /// <returns>return a int which represents the entity id for the object</returns>
        public int CreateDude()
        {
            int entityID = ComponentManager.Instance.CreateID();
            Model chuck = ModelDic["Dude"];
            KeyBoardComponent keys = new KeyBoardComponent();
            keys.AddActionAndKey("Forward", Keys.W);
            keys.AddActionAndKey("Backwards", Keys.S);
            keys.AddActionAndKey("Right", Keys.D);
            keys.AddActionAndKey("Left", Keys.A);
            keys.AddActionAndKey("Up", Keys.Space);
            

            MouseComponent mouse = new MouseComponent();
            mouse.AddActionToButton("Fire", "LeftButton");
            mouse.MouseSensitivity = 0.2f;

            List<IComponent> components = new List<IComponent>
            {
                new TransformComponent(new Vector3(20,45,-10), new Vector3(0.1f, 0.1f, 0.1f)),
                new ModelComponent(chuck),
                keys,
                mouse,
                new PlayerComponent(),
                new PhysicsComponent()
                {
                    Volume = 22.5f,
                    Density = 2.66f,
                    PhysicsType = PhysicsType.Rigid,
                    MaterialType = MaterialType.Skin,
                    GravityType = GravityType.World,
                    DragType = DragType.ManUpright
                },
            };

            ComponentManager.Instance.AddAllComponents(entityID, components);


            TestingTheAnimationsWithDude(entityID);
            return entityID;

        }

        // Todo lägg till FOG
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

        // Todo lägg till FOG
        /// <summary>
        /// creates the static objects
        /// </summary>
        /// <param name="nameOfModel"></param>
        /// <param name="position"></param>
        public int CreateStaticObject(string nameOfModel, Vector3 position)
        {
            Vector3 scale = new Vector3();
            Model model = ModelDic[nameOfModel];

            switch (nameOfModel)
            {
                case "Rock":
                    scale = new Vector3(5, 5, 5);
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        foreach( BasicEffect effect in mesh.Effects)
                        {
                            effect.TextureEnabled = true;
                            effect.Texture = Texture2dDic["RockTexture"];
                        }
                    }
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

            return entityID;
        }

        /// <summary>
        /// Randomizes a map 
        /// </summary>
        /// <param name="numberOfHouses"></param>
        /// <param name="numberOfStaticObjects"></param>
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
                        entityIdList.Add(CreateHouse("House_Wood", positions[i]));
                        unablePositions.Add(positions[i]);
                    }
                    else
                    {
                        entityIdList.Add(CreateHouse("House_Stone", positions[i]));
                        unablePositions.Add(positions[i]);
                    }
                }
                else
                {
                    i--;
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
        /// Gets target number of potitions on heightmap
        /// </summary>
        /// <param name="numberOfPositions"></param>
        /// <returns></returns>
        public List<Vector3> GetModelPositions(int numberOfPositions)
        {
            List<Vector3> positions = new List<Vector3>();
            Random r = new Random();
            int mapWidht;
            int mapHeight;
            List<int> heightList = ComponentManager.Instance.GetAllEntitiesWithComponentType<HeightmapComponentTexture>();
            HeightmapComponentTexture heigt = ComponentManager.Instance.GetEntityComponent<HeightmapComponentTexture>(heightList[0]);
            mapWidht = heigt.Width;
            mapHeight = heigt.Height;
            for (int i = 0; i < numberOfPositions; i++)
            {
                var pot = new Vector3(r.Next(mapWidht-10), 0,r.Next(mapHeight-10));
                pot.Y = heigt.HeightMapData[(int)pot.X,(int)pot.Z];
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
        /// Creates a static camera on the specified position and that is looking att the specified lookat
        /// </summary>
        /// <param name="CameraPosition"> Position of the camera </param>
        /// <param name="lookAt"> A position that the camera should look at </param>
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
        
        /// <summary>
        /// Removes the current camera
        /// </summary>
        public void RemoveCam()
        {
            var temp = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<CameraComponent>();
            ComponentManager.Instance.RemoveComponentFromEntity(temp.Keys.First(), temp.Values.First());
        }


        // todo, funkar inte
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityID"></param>
        public void AddPOVCamToEntity(int entityID)
        {
            ComponentManager.Instance.AddComponentToEntity(entityID, new CameraComponent(CameraType.Pov) {
                Offset = new Vector3(0, 10, 30)
            });
                
        }

        /// <summary>
        /// Adds an chase Camera to an entity
        /// </summary>
        /// <param name="EntityId"> The ID of the enitt that the camera should follow </param>
        /// <param name="Offset"> How far behind the camera should be </param>
        public void AddChaseCamToEntity(int EntityId, Vector3 Offset)
        {
            CameraComponent chaseCam = new CameraComponent(CameraType.Chase)
            {
                Offset = Offset
            };
            ComponentManager.Instance.AddComponentToEntity(EntityId, chaseCam);
        }
        
        // todo write comment
        /// <summary>
        /// Creates an bullet .... 
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="pos"></param>
        /// <param name="Orientation"></param>
        /// <param name="scale"></param>
        /// <param name="forward"></param>
        /// <param name="MaxRange"></param>
        /// <param name="initialVelocity"></param>
        /// <param name="rotation"></param>
        /// <returns> The enityId of the bullet in case someone would need it sometime </returns>
        public int CreateBullet(string modelName, Vector3 pos, Vector3 scale, Vector3 forward, float MaxRange, float initialVelocity, Vector3 rotation)
        {
            pos = new Vector3(pos.X, pos.Y + 4.5f, pos.Z);
            int BulletEntity = ComponentManager.Instance.CreateID();

            Model model = ModelDic[modelName];
            List<IComponent> componentList = new List<IComponent>()
            {
                new TransformComponent(pos, scale)
                {
                    Rotation = rotation,
                },
                new  ModelComponent(model){
                    IsVisible = VisibleBullets,
                },
                
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
                    
                    InitialVelocity = Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z).Forward * initialVelocity,
                }, 
            };

            ComponentManager.Instance.AddAllComponents(BulletEntity, componentList);

            return BulletEntity;
        }
        /// <summary>
        /// A temporary class responsible for adding a 
        /// </summary>
        /// <param name="entityID"></param>
        public void TestingTheAnimationsWithDude(int entityID)
        {

            ModelComponent mcp = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entityID);

            

            AnimationComponent anm = new AnimationComponent(mcp.Model.Tag);

            ComponentManager.Instance.AddComponentToEntity(entityID, anm);

            anm.StartClip("Take 001");

          
        }
        
        /// <summary>
        /// Creates an Height based on the specified heightMap
        /// </summary>
        /// <param name="heightmap"> Name of the heightMap texture that shall be used to build the hieghtMap </param>
        /// <param name="heightTexture"> the texture that each chunk of the height map will have </param>
        /// <param name="numberOfChunksPerSide"> number of chunks per side
        /// eg 10 chunks per side will create a total of 100 chunks for the whole heightmap </param>
        public int CreateHeightMap(string heightmap, string heightTexture, int numberOfChunksPerSide)
        {
            int HeightmapEnt = ComponentManager.Instance.CreateID();
            var hmp = hmFactory.CreateTexturedHeightMap(Texture2dDic[heightmap], Texture2dDic[heightTexture], numberOfChunksPerSide);
            List<IComponent> HeightmapCompList = new List<IComponent>
            {
                hmp,
                new TransformComponent(new Vector3(0, 0, 0), new Vector3(1, 1, 1))
            };
            ComponentManager.Instance.AddAllComponents(HeightmapEnt, HeightmapCompList);

            return HeightmapEnt;
        }

        // todo write comment
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int CreateAI(string ModelName, Vector3 position, float hysteria, int widthBound, int heightBound, float DirectionDuration, float rotation, float shootingCoolDown, float attackingDistance, float evadeDist, float turningSpeed, float updateFreq, List<Vector2> waypoints, float chaseDist)
        {
            int AIEntityID = ComponentManager.Instance.CreateID();
            Model model = ModelDic[ModelName];

            var BoundRec = new Rectangle(0, 0, widthBound, heightBound);

            List<IComponent> components = new List<IComponent>
            {
                new TransformComponent(position, new Vector3(0.1f, 0.1f, 0.1f)),
                new ModelComponent(model),
                new PhysicsComponent()
                {
                    Volume = 22.5f,
                    Density = 2.66f,
                    PhysicsType = PhysicsType.Rigid,
                    MaterialType = MaterialType.Skin,
                    GravityType = GravityType.World,
                    DragType = DragType.ManUpright
                },
                new AIComponent(BoundRec, shootingCoolDown, waypoints){
                    Hysteria = hysteria,
                    AttackingDistance = attackingDistance,
                    DirectionChangeRoation = rotation,
                    DirectionDuration = DirectionDuration,
                    EvadeDistance = evadeDist,
                    TurningSpeed = turningSpeed,
                    UpdateFrequency = updateFreq,
                    ChaseDistance = chaseDist,
                },
            };

            ComponentManager.Instance.AddAllComponents(AIEntityID, components);


            TestingTheAnimationsWithDude(AIEntityID);

            return AIEntityID;
        }
        /// <summary>
        /// Function to create gaming hud.
        /// </summary>
        /// <param name="healthPosition"></param>
        /// <param name="AmmunitionPosition"></param>
        /// <param name="PlayersRemainingPosition"></param>
        /// <param name="SlotPositions"></param>
        public int CreateHud(Vector2 healthPosition, Vector2 AmmunitionPosition, Vector2 PlayersRemainingPosition, List<Vector2> SlotPositions)
        {
            int HudID = ComponentManager.Instance.CreateID();
            SpriteFont font = Content.Load<SpriteFont>("Fonts\\Font");

            HealthComponent health = new HealthComponent();
            AmmunitionComponent ammo = new AmmunitionComponent()
            {
                ActiveMagazine = new Tuple<AmmunitionType, int, int>(AmmunitionType.AK_47, 30, Magazine.GetSize(AmmunitionType.AK_47))
            };
            Texture2DComponent slot1 = new Texture2DComponent(Content.Load<Texture2D>("Icons\\squareTest"))
            {
                Scale = new Vector2(0.2f, 0.2f),
            };
            slot1.Position = new Vector2(GameOne.Instance.GraphicsDevice.Viewport.Width/2 - ((slot1.Width * slot1.Scale.X)/2), GameOne.Instance.GraphicsDevice.Viewport.Height);


            List<TextComponent> textComponents = new List<TextComponent>
            {
                new TextComponent(health.Health.ToString(), healthPosition, Color.Pink, font, true, Color.WhiteSmoke, true, 0.3f), // health
                new TextComponent(ammo.ActiveMagazine.Item2 + "/" + ammo.ActiveMagazine.Item3 + " " + ammo.ActiveMagazine.Item1 + " Clips left: " + ammo.AmmountOfActiveMagazines , AmmunitionPosition, Color.DeepPink, font, true, Color.WhiteSmoke, true, 0.3f), // ammo
                // TODO: en player remaining vet inte om vi skall göra en komponent för det? :) <- Det är väl bara att plocka ut typ alla "health component" o kolla hur många som har mer än 0 i hälsa?
            };
            List<string> names = new List<string>
            {
                "Health",
                "Ammunition",

                // TODO: en player remaining vet inte om vi skall göra en komponent för det? :)
            };
            foreach (Vector2 slots in SlotPositions)
            {

            }


            List<IComponent> components = new List<IComponent>
            {
                health,
                ammo,
                slot1,
                new TextComponent(names, textComponents)
            };
            ComponentManager.Instance.AddAllComponents(HudID, components);

            return HudID;
        }
    }
}
