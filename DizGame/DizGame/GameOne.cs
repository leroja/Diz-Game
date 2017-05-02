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

namespace DizGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameOne : GameEngine.GameEngine
    {
        NetworkSystem client;

        public GameOne()
        {

            client = new NetworkSystem();
            client.RunClient();
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            // test
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;

            //Graphics.IsFullScreen = true;
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

            client.DiscoverLocalPeers();
            EntityFactory entf = new EntityFactory(Content, Device);

            var idC = entf.CreateChuckGreen();
            //var idK = entf.CreateKitana();
            //var id = entf.CreateBullet("Bullet", new Vector3(0, 0, -20), Vector3.Zero, new Vector3(.3f, .3f, .3f));

            //entf.CreateStaticCam(Vector3.Zero, new Vector3(0, 0, -20));
            // House_Wood or House_Stone
            
            //ntf.createHouse("House_Stone", new Vector3(5, 0, -20));
            var id = entf.CreateKitana();
            //Model bullet = Content.Load<Model>("bullet/Bullet");
            //var id = entf.CreateBullet("Bullet", new Vector3(0, 0, -20), Vector3.Zero, new Vector3(.3f, .3f, .3f));
            entf.AddChaseCamToEntity(idC, new Vector3(0, 70, 50));

            entf.CreateHeightMap("canyonHeightMap", "BetterGrass");
            entf.makeMap(2,100);
            //entf.CreateHeightMap("heightmap", "BetterGrass");
            SystemManager.Instance.AddSystem(new TransformSystem());
            SystemManager.Instance.AddSystem(new ModelBoundingSphereSystem());
            SystemManager.Instance.AddSystem(new WindowTitleFPSSystem(this));
            SystemManager.Instance.AddSystem(new ModelSystem());
            
            SystemManager.Instance.AddSystem(new KeyBoardSystem());
            SystemManager.Instance.AddSystem(new MovingSystem());
            SystemManager.Instance.AddSystem(new CameraSystem());
            SystemManager.Instance.AddSystem(new PhysicsSystem(false));
            SystemManager.Instance.AddSystem(new EnvironmentSystem());
            SystemManager.Instance.AddSystem(new MouseSystem());
            SystemManager.Instance.AddSystem(new BulletSystem());
            SystemManager.Instance.AddSystem(new PlayerSystem(entf));

            SystemManager.Instance.AddSystem(new HeightmapSystemTexture(Device));


            //SystemManager.Instance.AddSystem(new ConfiguredHeightMapSystem(this));

            base.Initialize();

            //Mouse.SetPosition(Device.Viewport.Width / 2, Device.Viewport.Height / 2);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

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
