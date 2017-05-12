using DizGame.Source.Systems;
using Lidgren.Network;
using GameEngine.Source.Managers;
using GameEngine.Source.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DizGame.Source.ConfiguredSystems;
using GameEngine.Source.Factories;
using GameEngine.Source.Components;
using System.Collections.Generic;
using DizGame.Source.GameStates;
using DizGame.Source.LanguageBasedModels;

namespace DizGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameOne : GameEngine.GameEngine
    {
        NetworkSystem client;

        private static Game instance;

        public static EntityTracingSystem EntityTracingSystem { get; private set; }

        public GameOne()
        {
            instance = this;
            client = new NetworkSystem();
            client.RunClient();
            this.IsMouseVisible = true;

            
        }

        public static Game Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Device = Graphics.GraphicsDevice;
            // test
            //Graphics.PreferredBackBufferWidth = 1280;
            //Graphics.PreferredBackBufferHeight = 720;
            Graphics.PreferredBackBufferHeight = Device.DisplayMode.Height / 2;
            Graphics.PreferredBackBufferWidth = Device.DisplayMode.Width / 2;
            Graphics.ApplyChanges();
            //Graphics.IsFullScreen = true;
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            client.DiscoverLocalPeers();
            EntityFactory entf = EntityFactory.Instance;


            entf.CreateAI("Dude", new Vector3(30, 45, -10), 25, 100, 300, 5f, MathHelper.Pi);
            entf.CreateAI("Dude", new Vector3(40, 45, -10), 25, 100, 300, 3.5f, MathHelper.Pi);
            entf.CreateAI("Dude", new Vector3(50, 45, -10), 25, 100, 300, 2f, MathHelper.Pi);
            var idC = EntityFactory.Instance.CreateDude();

            //entf.CreateStaticCam(Vector3.Zero, new Vector3(0, 0, -20));
            //entf.AddChaseCamToEntity(idC, new Vector3(0, 75, 100));
            //entf.AddChaseCamToEntity(idC, new Vector3(0, 25, 25));
            entf.AddChaseCamToEntity(idC, new Vector3(0, 10, 25));
            //entf.AddChaseCamToEntity(idC, new Vector3(0, 0, 25));
            //entf.AddPOVCamToEntity(idC);
            //entf.AddChaseCamToEntity(idC, new Vector3(0, 0, 2));

            entf.CreateHeightMap("canyonHeightMap", "BetterGrass", 10);

            entf.MakeMap(10, 100);

            entf.CreateHud(new Vector2(30, GraphicsDevice.Viewport.Height-50), 
                new Vector2(GraphicsDevice.Viewport.Width / 10, GraphicsDevice.Viewport.Height - 50), 
                new Vector2(0, 0), new List<Vector2>());
            InitializeSystems(entf);

            base.Initialize();
        }

        /// <summary>
        /// Initializes the systems and adds them to the system manager
        /// </summary>
        /// <param name="entf"></param>
        private void InitializeSystems(EntityFactory entf)
        {
            SystemManager.Instance.AddSystem(new WorldSystem(this));
            //entf.CreateHeightMap("heightmap", "BetterGrass");
            SystemManager.Instance.AddSystem(new TransformSystem());
            SystemManager.Instance.AddSystem(new ModelBoundingSphereSystem());

            SystemManager.Instance.AddSystem(new TransformSystem());
            SystemManager.Instance.AddSystem(new ModelBoundingSphereSystem());
            SystemManager.Instance.AddSystem(new WindowTitleFPSSystem(this));
            SystemManager.Instance.AddSystem(new TransformSystem());
            SystemManager.Instance.AddSystem(new ModelBoundingSphereSystem());
            SystemManager.Instance.AddSystem(new ModelSystem());
            SystemManager.Instance.AddSystem(new KeyBoardSystem());
            SystemManager.Instance.AddSystem(new MovingSystem());
            SystemManager.Instance.AddSystem(new CameraSystem());
            SystemManager.Instance.AddSystem(new PhysicsSystem());
            SystemManager.Instance.AddSystem(new EnvironmentSystem());
            SystemManager.Instance.AddSystem(new MouseSystem());
            SystemManager.Instance.AddSystem(new BulletSystem());
            SystemManager.Instance.AddSystem(new PlayerSystem());

            SystemManager.Instance.AddSystem(new HeightmapSystemTexture(Device));
            SystemManager.Instance.AddSystem(new AnimationSystem());
            SystemManager.Instance.AddSystem(new AISystem());
            SystemManager.Instance.AddSystem(new TextSystem(SpriteBatch));
            SystemManager.Instance.AddSystem(new _2DSystem(SpriteBatch));

            EntityTracingSystem = new EntityTracingSystem();
            EntityTracingSystem.RecordInitialEntities();
            SystemManager.Instance.AddSystem(EntityTracingSystem);

            //Uncoment the two rows below to watch the gamemenu, you might wanna comment the other
            //stuffs before doing that
            //MainMenu startState = new MainMenu();
            //GameStateManager.Instance.Push(startState);


        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            TreeModel tree = new TreeModel(Device, 1f, MathHelper.PiOver4 - 0.4f, "F[LF]F[RF]F", 0, 1f, new string[] { "F" });
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            //Mouse.SetPosition(Device.Viewport.Width / 2, Device.Viewport.Height / 2);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //Remove comment below to make the gamestatemanager run the gamestates update functions
            //GameStateManager.Instance.UpdateGameState();
            base.Update(gameTime);
        }

        //protected override void Update(GameTime gameTime)
        //{
        //    client.ReadMessages();
        //    for(var i = 1; i < 10000; i++)
        //    {
        //        if(i % 4 == 0)
        //        {
        //            client.SendMessage("What's my name?");
        //        }
        //    }

        //}


    }
}
