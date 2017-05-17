
using DizGame.Source.Components;
using DizGame.Source.Enums;
using DizGame.Source.Factories;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using GameEngine.Source.Factories;
using GameEngine.Source.Managers;
using GameEngine.Source.Systems;
using GameEngine.Source.RandomStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DizGame.Source.Components.ResourceComponent;

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
        /// <summary>
        /// Hud factory
        /// </summary>
        public HudFactory HudFactory { get; set; }
        /// <summary>
        /// A Bool that says whether the models are vivible or not
        /// </summary>
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
        /// Private constructor of the entityfactory
        /// </summary>
        private EntityFactory()
        {
            VisibleBullets = true;
            hmFactory = new HeightMapFactory(GameOne.Instance.GraphicsDevice);
            CreateWorldComp();
            this.Content = GameOne.Instance.Content;

            HudFactory = new HudFactory(Content);

            ModelDic = new Dictionary<string, Model>
            {
                { "Bullet", Content.Load<Model>("Bullet/Bullet") },
                { "Cartridge", Content.Load<Model>("Bullet/Cartridge") },
                { "House_Wood", Content.Load<Model>("MapObjects/Farmhouse/medievalHouse1") } ,
                { "House_Stone", Content.Load<Model>("MapObjects/WoodHouse/Cyprys_House") } ,
                { "Tree", Content.Load<Model>("MapObjects/Tree/lowpolytree") },
                { "Rock", Content.Load<Model>("MapObjects/Rock/Rock") },
                { "Dude", Content.Load<Model>("Dude/Dude72") },
                { "Heart", Content.Load<Model>("MapObjects/Heart/Heart") },
            };

            Texture2dDic = new Dictionary<string, Texture2D>() {
                {"BetterGrass", Content.Load<Texture2D>("HeightMapStuff/BetterGrass") },
                {"canyonHeightMap", Content.Load<Texture2D>("HeightMapStuff/canyonHeightMap")},
                {"heightmap", Content.Load<Texture2D>("HeightMapStuff/heightmap") },
                {"RockTexture", Content.Load<Texture2D>("MapObjects/Rock/Stone Texture") },
                { "Smoke", Content.Load<Texture2D>("ParticleTexture/Smoke") },
                {"Map3", Content.Load<Texture2D>("HeightMapStuff/Map3") },
            };
        }
       
        /// <summary>
        /// Creates the World Component
        /// </summary>
        public void CreateWorldComp()
        {
            var worldEntId = ComponentManager.Instance.CreateID();
            var compList = new List<IComponent>() {
                new WorldComponent(Matrix.Identity)
                {
                    IsSunActive = true,
                    DefineHour = 2,
                    
                },
            };
            FlareFactory.CreateFlare(GameOne.Instance.Content, GameOne.Instance.GraphicsDevice, worldEntId);
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
            keys.AddActionAndKey("Mute", Keys.M);
            

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
        /// Creates a parrical emmiter and sets positions and options
        /// </summary>
        /// <param name="Position"> Position of emiter</param>
        /// <param name="TextureName">Name of texture</param>
        /// <param name="nParticles">maximum number of particles in emiter. used for size of vectors</param>
        /// <param name="Particlelifetime"> life of particke</param>
        /// <param name="FadeTime">Fade time on particles</param>
        /// <param name="direction">Direction of particles</param>
        /// <param name="scale">Scale on Particle</param>
        /// <param name="EmitterLifeTime">Life time on emitter</param>
        public void CreateParticleEmiter(Vector3 Position,String TextureName,int nParticles, float Particlelifetime, float FadeTime, Vector3 direction, int scale,int EmitterLifeTime)
        {
            TransformComponent tran = new TransformComponent(Position, new Vector3(scale));
            ParticleEmiterComponent emiter = new ParticleEmiterComponent(TextureName, nParticles, Particlelifetime, Texture2dDic[TextureName], FadeTime, direction)
            {
                EmiterLife = EmitterLifeTime,
                effect = Content.Load<Effect>("Effects/ParticleEffect"),
            };
            int id = ComponentManager.Instance.CreateID();
            GenerateParticle(emiter);

            ComponentManager.Instance.AddComponentToEntity(id, tran);
            ComponentManager.Instance.AddComponentToEntity(id, emiter);
        }
        /// <summary>
        /// Called for instancning vectore to store particles in
        /// </summary>
        /// <param name="emiter"> ParticleEmitterComponent</param>
        public void GenerateParticle(ParticleEmiterComponent emiter)
        {
            emiter.particle = new ParticleVertex[emiter.nParticles * 4];
            emiter.indices = new int[emiter.nParticles * 6];

            var z = Vector3.Zero;
            int x = 0;
            for (int i = 0; i < emiter.nParticles * 4; i += 4)
            {
                emiter.particle[i + 0] = new ParticleVertex(z, new Vector2(0, 0),
                z, 0, -1);
                emiter.particle[i + 1] = new ParticleVertex(z, new Vector2(0, 1),
                z, 0, -1);
                emiter.particle[i + 2] = new ParticleVertex(z, new Vector2(1, 1),
                z, 0, -1);
                emiter.particle[i + 3] = new ParticleVertex(z, new Vector2(1, 0),
                z, 0, -1);

                emiter.indices[x++] = i + 0;
                emiter.indices[x++] = i + 3;
                emiter.indices[x++] = i + 2;
                emiter.indices[x++] = i + 2;
                emiter.indices[x++] = i + 1;
                emiter.indices[x++] = i + 0;
            }
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
        /// <param name="flareAble"></param>
        public void CreateStaticCam(Vector3 CameraPosition, Vector3 lookAt, bool flareAble = false)
        {
            ComponentManager.Instance.AddAllComponents(ComponentManager.Instance.CreateID(), new List<IComponent>() {
                new TransformComponent(CameraPosition, Vector3.One),
                new CameraComponent(CameraType.StaticCam)
                {
                    LookAt = lookAt,
                    IsFlareable = flareAble
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
        /// Adds a POV camera to an entity
        /// </summary>
        /// <param name="entityID"> ID of the entity </param>
        /// <param name="isFlareable"></param>
        public void AddPOVCamToEntity(int entityID, bool isFlareable = false)
        {
            ComponentManager.Instance.AddComponentToEntity(entityID, new CameraComponent(CameraType.Pov)
            {
                Offset = new Vector3(0, 10, 30),
                IsFlareable = isFlareable
            });
        }

        /// <summary>
        /// Adds an chase Camera to an entity
        /// </summary>
        /// <param name="EntityId"> The ID of the enitt that the camera should follow </param>
        /// <param name="Offset"> How far behind the camera should be </param>
        /// <param name="isFlareable"></param>
        public void AddChaseCamToEntity(int EntityId, Vector3 Offset, bool isFlareable = false)
        {
            CameraComponent chaseCam = new CameraComponent(CameraType.Chase)
            {
                Offset = Offset,
                IsFlareable = isFlareable
            };
            ComponentManager.Instance.AddComponentToEntity(EntityId, chaseCam);
        }

        /// <summary>
        /// Creates a new bullet
        /// </summary>
        /// <param name="modelName"> Name of the bullet model to use </param>
        /// <param name="pos"> The starting position of the bullet </param>
        /// <param name="scale"> How much to scale the bullet </param>
        /// <param name="MaxRange"> The max range of the bullet </param>
        /// <param name="initialVelocity"> The initial velocity of the bullet </param>
        /// <param name="rotation"> The rotation oi the bullet </param>
        /// <param name="damage"> How much damage the bullet does </param>
        /// <returns> The enityId of the bullet incase someone would need it </returns>
        public int CreateBullet(string modelName, Vector3 pos, Vector3 scale, float MaxRange, float initialVelocity, Vector3 rotation, float damage)
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
                    Damage = damage,
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
            
            AnimationComponent anm = new AnimationComponent(((Dictionary<string, object>)mcp.Model.Tag)["SkinningData"]);

            ComponentManager.Instance.AddComponentToEntity(entityID, anm);

            var sk = anm.SkinningDataValue.AnimationClips.Keys;

            anm.StartClip(sk.First());
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

        /// <summary>
        /// Creates a new AI Entity
        /// </summary>
        /// <param name="ModelName"> The name of the model the AI sjould use </param>
        /// <param name="position"> The initial position of the AI </param>
        /// <param name="hysteria">  </param>
        /// <param name="widthBound">  </param>
        /// <param name="heightBound">  </param>
        /// <param name="DirectionDuration"> A value in seconds for how long the AI will stick to its choosen direction </param>
        /// <param name="rotation"> In what range the new rotaion can be. eg. -PI --- +PI </param>
        /// <param name="shootingCoolDown"> The delay between the shoots for when the AI is shooting
        /// Ideally somewhere between 0 and 1 second </param>
        /// <param name="attackingDistance"> From how far the AI will start shooting </param>
        /// <param name="evadeDist"> How far from the other AIs/players the AI want to be at a minimum </param>
        /// <param name="turningSpeed"> How fast the AI will turn each update </param>
        /// <param name="updateFreq"> How often the AI will update its rotation based on the closest enemy in seconds </param>
        /// <param name="waypoints"> A list of waypoints for the patrolling AI. If the AI not patrolling this can be null </param>
        /// <param name="chaseDist"> In what distance the enemy have to be for the AI to chase it </param>
        /// <param name="DamagePerShot"> How much damage the AI does per shot </param>
        /// <returns> The ID of the new AI Entity  </returns>
        public int CreateAI(string ModelName, Vector3 position, float hysteria, int widthBound, int heightBound, float DirectionDuration, float rotation, float shootingCoolDown, float attackingDistance, float evadeDist, float turningSpeed, float updateFreq, List<Vector2> waypoints, float chaseDist, float DamagePerShot)
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
                    DamagePerShot = DamagePerShot,
                },
            };

            ComponentManager.Instance.AddAllComponents(AIEntityID, components);
            
            TestingTheAnimationsWithDude(AIEntityID);

            return AIEntityID;
        }
        public void CreateHealthResource(Vector3 position)
        {
            int newEntityId = ComponentManager.Instance.CreateID();

            //adjust the scales differently for the models if needed
            TransformComponent tcp = new TransformComponent(position, new Vector3(1, 1, 1));
            tcp.Rotation = new Vector3(0.1f, 0, 0);
            
            
            
            //Also create the model component for the entity + check visabillity

            List<IComponent> resourceCompList = new List<IComponent>
            {
                new ResourceComponent(ResourceType.Health),
                tcp,
                // add the model here
            };
        }
        public void CreateAmmoResource()
        {

        }
    }
}
