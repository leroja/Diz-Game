﻿using AnimationContentClasses;
using AnimationContentClasses.Utils;
using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using GameEngine.Source.Factories;
using GameEngine.Source.Managers;
using GameEngine.Source.RandomStuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DizGame.Source.Factories
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
        /// HUD factory
        /// </summary>
        public HudFactory HudFactory { get; set; }
        /// <summary>
        /// Factory to create resources
        /// </summary>
        public ResourceFactory ResourceFactory { get; set; }
        /// <summary>
        /// A Factory for creating static game object. e.g a house
        /// </summary>
        public StaticGameObjectsFactory SGOFactory { get; set; }
        /// <summary>
        /// A Bool that says whether the models are visible or not
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
        /// Private constructor of the entityFactory
        /// </summary>
        private EntityFactory()
        {
            VisibleBullets = true;
            this.Content = GameOne.Instance.Content;

            ModelDic = new Dictionary<string, Model>
            {
                { "Bullet", Content.Load<Model>("Bullet/Bullet") },
                { "Cartridge", Content.Load<Model>("Bullet/Cartridge") },
                { "CyprusHouse", Content.Load<Model>("MapObjects/CyprusHouse/Cyprus_House2") } ,
                { "Tree", Content.Load<Model>("MapObjects/Tree/lowpolytree") },
                { "Rock", Content.Load<Model>("MapObjects/Rock/Rock") },
                { "Dude", Content.Load<Model>("Dude/Dude72") },
                { "Heart", Content.Load<Model>("MapObjects/Heart/Heart") },
                { "WoodHouse", Content.Load<Model>("MapObjects/WoodHouse/WoodHouse1") },
                { "skybox", Content.Load<Model>("MapObjects/Skybox/skybox3") },
            };

            Texture2dDic = new Dictionary<string, Texture2D>() {
                { "BetterGrass", Content.Load<Texture2D>("HeightMapStuff/BetterGrass") },
                { "canyonHeightMap", Content.Load<Texture2D>("HeightMapStuff/canyonHeightMap") },
                { "heightmap", Content.Load<Texture2D>("HeightMapStuff/heightmap") },
                { "RockTexture", Content.Load<Texture2D>("MapObjects/Rock/Stone Texture") },
                { "Smoke", Content.Load<Texture2D>("ParticleTexture/Smoke") },
                { "Map3", Content.Load<Texture2D>("HeightMapStuff/Map3") },
                { "CrossHair", Content.Load<Texture2D>("Icons/crosshairTransparent") },
            };
            hmFactory = new HeightMapFactory(GameOne.Instance.GraphicsDevice);
            HudFactory = new HudFactory(Content);
            ResourceFactory = new ResourceFactory(ModelDic);
            SGOFactory = new StaticGameObjectsFactory(ModelDic, Texture2dDic);
        }

        /// <summary>
        /// Creates the World Component
        /// </summary>
        public int CreateWorldComp()
        {
            var worldEntId = ComponentManager.Instance.CreateID();
            var compList = new List<IComponent>() {
                new WorldComponent(Matrix.Identity)
                {
                    IsSunActive = true,
                    DefineHour = 1,
                    Day = 1,
                    Hour = 0,
                    ModulusValue = 2,
                },
            };
            FlareFactory.CreateFlare(GameOne.Instance.Content, GameOne.Instance.GraphicsDevice, worldEntId);
            ComponentManager.Instance.AddAllComponents(worldEntId, compList);

            ComponentManager.Instance.AddComponentToEntity(ComponentManager.Instance.GetAllEntitiesWithComponentType<WorldComponent>()[0],
                new TextComponent("WorldTime",
                new Vector2(GameOne.Instance.GraphicsDevice.Viewport.Width / 2 - 50, 0),
                Color.White,
                Content.Load<SpriteFont>("Fonts/font"),
                true));
            return worldEntId;
        }

        // fixa till så cross hair är där skotten går, inte riktigt helt 100 just nu
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"> Position of the crosshair on the screen </param>
        /// <returns></returns>
        public int PlaceCrossHair(Vector2 position)
        {
            var Id = ComponentManager.Instance.CreateID();

            var textureComp = new Texture2DComponent(Texture2dDic["CrossHair"])
            {
                Scale = new Vector2(0.1f, 0.1f),
                
            };
            textureComp.Position = new Vector2(position.X - 7f, position.Y);

            ComponentManager.Instance.AddComponentToEntity(Id, textureComp);

            return Id;
        }

        /// <summary>
        /// Function to add the entity which might be the model for the different players
        /// </summary>
        /// <returns>return a int which represents the entity id for the object</returns>
        public int CreateDude(string nameOfPlayer)
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
            keys.AddActionAndKey("Sprint", Keys.LeftShift);
            keys.AddActionAndKey("SpectateUp", Keys.Up);
            keys.AddActionAndKey("SpectateDown", Keys.Down);


            MouseComponent mouse = new MouseComponent();
            mouse.AddActionToButton("Fire", "LeftButton");
            mouse.MouseSensitivity = 0.2f;

            List<IComponent> components = new List<IComponent>
            {
                new TransformComponent(new Vector3(20,45,-10), new Vector3(0.1f, 0.1f, 0.1f)),
                new ModelComponent(chuck),
                keys,
                mouse,
                new HealthComponent(),
                new AmmunitionComponent(),
                new ScoreComponent()
                {
                    NameOfScorer = nameOfPlayer
                },
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

        /// <summary>
        /// Checks if objects get the same position as Characters. if they have the same position the object it is removed
        /// </summary>
        public void SpawnProtection()
        {
            var modelList = ComponentManager.Instance.GetAllEntitiesWithComponentType<ModelComponent>();
            foreach (var Model1 in modelList)
            {
                foreach (var Model2 in modelList)
                {
                    ModelComponent ModComp1 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(Model1);
                    ModelComponent ModComp2 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(Model2);

                    if (ModComp1 != ModComp2)
                    {
                        if (ModComp2 != null && ModComp1 != null && ModComp1.BoundingVolume.Bounding.Intersects(ModComp2.BoundingVolume.Bounding))
                        {
                            if (!ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(ModComp2.ID) && !ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(ModComp2.ID))
                            {
                                ComponentManager.Instance.RemoveEntity(ModComp2.ID);
                                ComponentManager.Instance.RemoveEntity(ModComp2.ID);
                            }
                        }
                    }
                }
            }
            //List<int> spawnPositions = new List<int>();
            //List<int> modelId = ComponentManager.Instance.GetAllEntitiesWithComponentType<ModelComponent>();

            //spawnPositions.AddRange(ComponentManager.Instance.GetAllEntitiesWithComponentType<PlayerComponent>());
            //spawnPositions.AddRange(ComponentManager.Instance.GetAllEntitiesWithComponentType<AIComponent>());
            //foreach (var id in spawnPositions)
            //{
            //    TransformComponent tran = ComponentManager.Instance.GetEntityComponent<TransformComponent>(id);
            //    float minX = tran.Position.X - 10;
            //    float minZ = tran.Position.Z + 10;
            //    float maxX = tran.Position.X + 10;
            //    float maxZ = tran.Position.Z - 10;

            //    foreach (var model in modelId)
            //    {
            //        ModelComponent mod = ComponentManager.Instance.GetEntityComponent<ModelComponent>(model);
            //        TransformComponent Comp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(model);
            //        if (Comp != null && mod.IsStatic == true)
            //        {
            //            if ((Comp.Position.X >= minX && Comp.Position.X <= maxX) && (Comp.Position.Z <= minZ && Comp.Position.Z >= maxZ))
            //            {
            //                ComponentManager.Instance.RemoveEntity(model);
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Creates a particle emitter and sets positions and options
        /// </summary>
        /// <param name="Position"> Position of emitter</param>
        /// <param name="TextureName">Name of texture</param>
        /// <param name="nParticles">maximum number of particles in emitter. Used for size of vectors</param>
        /// <param name="Particlelifetime"> lifetime of particle </param>
        /// <param name="FadeTime">Fade time on particles</param>
        /// <param name="direction">Direction of particles</param>
        /// <param name="scale">Scale on Particle</param>
        /// <param name="EmitterLifeTime">Life time on emitter</param>
        public void CreateParticleEmitter(Vector3 Position, String TextureName, int nParticles, float Particlelifetime, float FadeTime, Vector3 direction, int scale, int EmitterLifeTime)
        {
            TransformComponent tran = new TransformComponent(Position, new Vector3(scale));
            ParticleEmitterComponent emitter = new ParticleEmitterComponent(TextureName, nParticles, Particlelifetime, Texture2dDic[TextureName], FadeTime, direction)
            {
                EmitterLife = EmitterLifeTime,
                Effect = Content.Load<Effect>("Effects/ParticleEffect"),
            };
            int id = ComponentManager.Instance.CreateID();
            GenerateParticle(emitter);

            ComponentManager.Instance.AddComponentToEntity(id, tran);
            ComponentManager.Instance.AddComponentToEntity(id, emitter);
        }

        /// <summary>
        /// Called for instancing a vector to store the particles in
        /// </summary>
        /// <param name="emitter"> ParticleEmitterComponent</param>
        public void GenerateParticle(ParticleEmitterComponent emitter)
        {
            emitter.Particles = new ParticleVertex[emitter.NumberOfParticles * 4];
            emitter.Indices = new int[emitter.NumberOfParticles * 6];

            var z = Vector3.Zero;
            var pos = new Vector3(10, 10, 10);
            int x = 0;
            for (int i = 0; i < emitter.NumberOfParticles * 4; i += 4)
            {
                emitter.Particles[i + 0] = new ParticleVertex(pos, new Vector2(0, 0),
                pos, 0, -1);
                emitter.Particles[i + 1] = new ParticleVertex(pos, new Vector2(0, 1),
                pos, 0, -1);
                emitter.Particles[i + 2] = new ParticleVertex(pos, new Vector2(1, 1),
                pos, 0, -1);
                emitter.Particles[i + 3] = new ParticleVertex(pos, new Vector2(1, 0),
                pos, 0, -1);

                emitter.Indices[x++] = i + 0;
                emitter.Indices[x++] = i + 3;
                emitter.Indices[x++] = i + 2;
                emitter.Indices[x++] = i + 2;
                emitter.Indices[x++] = i + 1;
                emitter.Indices[x++] = i + 0;
            }
        }

        /// <summary>
        /// Creates a static camera on the specified position and that is looking at the specified lookAt
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
        
        /// <summary>
        /// Adds a POV camera to an entity
        /// </summary>
        /// <param name="entityID"> ID of the entity </param>
        /// <param name="isFlareable"></param>
        public void AddPOVCamToEntity(int entityID, bool isFlareable = false)
        {
            ComponentManager.Instance.AddComponentToEntity(entityID, new CameraComponent(CameraType.Pov)
            {
                IsFlareable = isFlareable
            });
        }

        /// <summary>
        /// Adds an chase Camera to an entity
        /// </summary>
        /// <param name="EntityId"> The ID of the entity that the camera should follow </param>
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
        /// <param name="rotation"> The rotation of the bullet </param>
        /// <param name="damage"> How much damage the bullet does </param>
        /// <param name="ownerID"> Entity Id of the Owner </param>
        /// <returns> The enityId of the bullet in case someone would need it </returns>
        public int CreateBullet(string modelName, Vector3 pos, Vector3 scale, float MaxRange, float initialVelocity, Vector3 rotation, float damage, int ownerID)
        {
            pos = new Vector3(pos.X, pos.Y + 4.5f, pos.Z);
            int BulletEntity = ComponentManager.Instance.CreateID();
            Model model = ModelDic[modelName];
            BoundingVolume volume = (BoundingVolume)model.Tag;
            Util.ScaleBoundingVolume(ref volume, scale.X, pos, out BoundingVolume scaledVolume);
            List<IComponent> componentList = new List<IComponent>()
            {
                new TransformComponent(pos, scale)
                {
                    Rotation = rotation,
                },

                new  ModelComponent(model){
                    IsVisible = VisibleBullets,
                    BoundingVolume = scaledVolume
                },

                new BulletComponent(){
                    StartPos = pos,
                    MaxRange = MaxRange,
                    Damage = damage,
                    Owner = ownerID
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

                    Velocity = Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z).Forward * initialVelocity,
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
            TransformComponent tcp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entityID);
            ModelComponent mcp = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entityID);

            AnimationComponent anm = new AnimationComponent(((Dictionary<string, object>)mcp.Model.Tag)["SkinningData"]);

            ComponentManager.Instance.AddComponentToEntity(entityID, anm);

            var sk = anm.SkinningDataValue.AnimationClips.Keys;

            anm.StartClip(sk.First());
            Dictionary<string, object> dict = (Dictionary<string, object>)mcp.Model.Tag;
            BoundingVolume volume = (BoundingVolume)dict["BoundingVolume"];
            Util.ScaleBoundingVolume(ref volume, tcp.Scale.X, tcp.Position, out BoundingVolume scaledVolume);
            mcp.BoundingVolume = scaledVolume;
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
        /// <param name="ModelName"> The name of the model the AI should use </param>
        /// <param name="position"> The initial position of the AI </param>
        /// <param name="hysteria">  </param>
        /// <param name="widthBound">  </param>
        /// <param name="heightBound">  </param>
        /// <param name="DirectionDuration"> A value in seconds for how long the AI will stick to its chosen direction </param>
        /// <param name="rotation"> In what range the new rotation can be. e.g. -PI --- +PI </param>
        /// <param name="shootingCoolDown"> The delay between the shoots for when the AI is shooting
        /// Ideally somewhere between 0 and 1 second </param>
        /// <param name="attackingDistance"> From how far the AI will start shooting </param>
        /// <param name="evadeDist"> How far from the other AIs/players the AI want to be at a minimum </param>
        /// <param name="turningSpeed"> How fast the AI will turn each update </param>
        /// <param name="updateFreq"> How often the AI will update its rotation based on the closest enemy in seconds </param>
        /// <param name="waypoints"> A list of waypoints for the patrolling AI. If the AI not patrolling this can be null </param>
        /// <param name="chaseDist"> In what distance the enemy have to be for the AI to chase it </param>
        /// <param name="DamagePerShot"> How much damage the AI does per shot </param>
        /// <param name="nameOfAi"> The name of the AI used for scoring</param>
        /// <returns> The ID of the new AI Entity  </returns>
        public int CreateAI(string ModelName, Vector3 position, float hysteria, int widthBound, int heightBound, float DirectionDuration, float rotation, float shootingCoolDown, float attackingDistance, float evadeDist, float turningSpeed, float updateFreq, List<Vector2> waypoints, float chaseDist, float DamagePerShot, string nameOfAi)
        {
            int AIEntityID = ComponentManager.Instance.CreateID();
            Model model = ModelDic[ModelName];
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.FogEnabled = true;
                    effect.FogColor = Color.LightGray.ToVector3();
                    effect.FogStart = 10;
                    effect.FogEnd = 400;
                }
            }

            var BoundRec = new Rectangle(0, 0, widthBound, heightBound);

            List<IComponent> components = new List<IComponent>
            {
                new TransformComponent(position, new Vector3(0.1f, 0.1f, 0.1f)),
                new ModelComponent(model),
                new HealthComponent(),
                new AmmunitionComponent(),
                new ScoreComponent()
                {
                    NameOfScorer = nameOfAi
                },
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int CreateNewSkyBox()
        {
            int skyboxid = ComponentManager.Instance.CreateID();
            Model model = ModelDic["skybox"];
            //Texture2D texture = Content.Load<Texture2D>("MapObjects/Skybox/skybox_texture");
            //Effect skyBoxEffect = Content.Load<Effect>("Effects/SkyBox");

            List<IComponent> newcmpList = new List<IComponent>
            {
                new SkyBoxComponent(model), //507 och 505 rör du inte!
                new TransformComponent(new Vector3(507,45,-510), new Vector3(505,200,510))
            };
            ComponentManager.Instance.AddAllComponents(skyboxid, newcmpList);

            return skyboxid;
        }
    }
}