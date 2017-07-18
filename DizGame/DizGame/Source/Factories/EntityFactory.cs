using AnimationContentClasses;
using AnimationContentClasses.Utils;
using DizGame.Source.Components;
using DizGame.Source.Random_Stuff;
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
    // TODO refine fog effect parameters
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
            };

            Texture2dDic = new Dictionary<string, Texture2D>() {
                { "BetterGrass", Content.Load<Texture2D>("HeightMapStuff/BetterGrass") },
                { "canyonHeightMap", Content.Load<Texture2D>("HeightMapStuff/canyonHeightMap") },
                { "heightmap", Content.Load<Texture2D>("HeightMapStuff/heightmap") },
                { "RockTexture", Content.Load<Texture2D>("MapObjects/Rock/Stone Texture") },
                { "Smoke", Content.Load<Texture2D>("ParticleTexture/Smoke") },
                { "Map3", Content.Load<Texture2D>("HeightMapStuff/Map11") },
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
                    Hour = 1,
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

        /// <summary>
        /// Places a crosshair on the screen
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
                new _3DAudioListenerComponent(),
                new _3DSoundEffectComponent(),
                new SoundEffectComponent(),
                new TransformComponent(new Vector3(120,45,-100), new Vector3(0.1f, 0.1f, 0.1f)),
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
                            if (!ComponentManager.Instance.CheckIfEntityHasComponent<PlayerComponent>(ModComp2.ID) &&
                                !ComponentManager.Instance.CheckIfEntityHasComponent<AIComponent>(ModComp2.ID))
                            {
                                ComponentManager.Instance.RemoveEntity(ModComp2.ID);
                                ComponentManager.Instance.RemoveEntity(ModComp2.ID);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="typeOfParticle"></param>
        /// <param name="EmitterLifeTime"></param>
        public void CreateParticleEmitter(Vector3 Position,string typeOfParticle, float EmitterLifeTime)
        {
            switch (typeOfParticle)
            {
                case "Smoke":
                   ParticleSettingsComponent setting = CreateSmokeSettings();
                    ParticleEmitterComponent emitter = new ParticleEmitterComponent(GameOne.Instance.GraphicsDevice,6000);
                    emitter.particleEffect = Content.Load<Effect>("Effects//ParticleEffect");
                    TransformComponent tran = new TransformComponent();
                    tran.Position = Position;
                    emitter.LifeTime = 30;
                    emitter.ParticleType = "Smoke";
                    setting.Duration = TimeSpan.FromSeconds(EmitterLifeTime-1);
                    var a = ComponentManager.Instance.CreateID();
                    ComponentManager.Instance.AddComponentToEntity(a, setting);
                    ComponentManager.Instance.AddComponentToEntity(a, emitter);
                    ComponentManager.Instance.AddComponentToEntity(a, tran);
                    break;
                case "Blood":
                    ParticleSettingsComponent settingB = CreateBloodSettings();
                    ParticleEmitterComponent emitterB = new ParticleEmitterComponent(GameOne.Instance.GraphicsDevice, 600);
                    emitterB.particleEffect = Content.Load<Effect>("Effects//ParticleEffect");
                    TransformComponent tranB = new TransformComponent();
                    tranB.Position = Position;
                    emitterB.LifeTime = 0.3f;
                    emitterB.ParticleType = "Blood";
                    settingB.Duration = TimeSpan.FromSeconds(EmitterLifeTime - 1);
                    var aB = ComponentManager.Instance.CreateID();
                    ComponentManager.Instance.AddComponentToEntity(aB, settingB);
                    ComponentManager.Instance.AddComponentToEntity(aB, emitterB);
                    ComponentManager.Instance.AddComponentToEntity(aB, tranB);
                    break;

                default:
                    break;
            }


        }
        /// <summary>
        /// Sets settings for Smoke Particles
        /// </summary>
        /// <returns>settings for smoke particles</returns>
        private ParticleSettingsComponent CreateBloodSettings()
        {
            ParticleSettingsComponent setting = new ParticleSettingsComponent();
            setting.texture = Texture2dDic["Smoke"];
            setting.MaxParticles = 600;
            setting.Duration = TimeSpan.FromSeconds(10);
            setting.MinHorizontalVelocity = -20;
            setting.MaxHorizontalVelocity = 20;
            setting.MinVerticalVelocity = -5;
            setting.MaxVerticalVelocity = 5;
            setting.Gravity = new Vector3(0, 0, 0);
            setting.EndVelocity = 1;
            setting.MaxColor = Color.Red;
            setting.MinColor = Color.Red;
            setting.MinRotateSpeed = -1;
            setting.MaxRotateSpeed = 1;
            setting.MinStartSize = 4;
            setting.MaxStartSize = 7;
            setting.MinEndSize = 10;
            setting.MaxEndSize = 50;

            return setting;
        }
        /// <summary>
        /// sets setings for blood particles
        /// </summary>
        /// <returns>settings for blood particles</returns>
        private ParticleSettingsComponent CreateSmokeSettings()
        {
            ParticleSettingsComponent setting = new ParticleSettingsComponent();
            setting.texture = Texture2dDic["Smoke"];
            setting.MaxParticles = 6000;
            setting.Duration = TimeSpan.FromSeconds(10);
            setting.MinHorizontalVelocity = 0;
            setting.MaxHorizontalVelocity = 15;
            setting.MinVerticalVelocity = 10;
            setting.MaxVerticalVelocity = 20;
            setting.Gravity = new Vector3(-20, -5, 0);
            setting.EndVelocity = 1;
            setting.MinRotateSpeed = -1;
            setting.MaxRotateSpeed = 1;
            setting.MinStartSize = 4;
            setting.MaxStartSize = 50;
            setting.MinEndSize = 35;
            setting.MaxEndSize = 140;

            return setting;
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
        /// e.g 10 chunks per side will create a total of 100 chunks for the whole heightmap </param>
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
        /// <param name="border">  </param>
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
        public int CreateAI(string ModelName, Vector3 position, float hysteria, Border border, float DirectionDuration, float rotation, float shootingCoolDown, float attackingDistance, float evadeDist, float turningSpeed, float updateFreq, List<Vector2> waypoints, float chaseDist, float DamagePerShot, string nameOfAi)
        {
            int AIEntityID = ComponentManager.Instance.CreateID();
            Model model = ModelDic[ModelName];
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.FogEnabled = true;
                    effect.FogColor = Color.Orange.ToVector3();
                    effect.FogStart = 10;
                    effect.FogEnd = 400;
                }
            }

            List<IComponent> components = new List<IComponent>
            {
                new _3DSoundEffectComponent(),
                new SoundEffectComponent(),
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
                new AIComponent(border, shootingCoolDown, waypoints){
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

        // TODO choose the best Skybox texture
        /// <summary>
        /// Creates a new skybox
        /// </summary>
        /// <returns> The enityID of the skybox entity </returns>
        public int CreateNewSkyBox()
        {
            int skyboxId = ComponentManager.Instance.CreateID();
            Model model = Content.Load<Model>("Skybox/cube");
            Effect skyBoxEffect = Content.Load<Effect>("Effects/Skybox");
            TextureCube skyboxTexture = Content.Load<TextureCube>("Skybox/SkyboxTextures/Sunset");
            List<IComponent> newcmpList = new List<IComponent>
            {
                new SkyBoxComponent(model){
                    SkyboxTextureCube = skyboxTexture,
                    SkyboxEffect = skyBoxEffect,
                    Size = 750
                },
            };
            ComponentManager.Instance.AddAllComponents(skyboxId, newcmpList);

            return skyboxId;
        }
    }
}