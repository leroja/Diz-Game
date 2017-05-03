﻿using DizGame.Source.Systems;
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

        private static Game instance;

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

            client.DiscoverLocalPeers();
            EntityFactory entf = new EntityFactory(Content, Device);

            var idC = entf.CreateChuckGreen();
            var idK = entf.CreateKitana();

            //entf.CreateStaticCam(Vector3.Zero, new Vector3(0, 0, -20));
            entf.AddChaseCamToEntity(idC, new Vector3(0, 20, 25));

            entf.CreateHeightMap("canyonHeightMap", "BetterGrass");
            //entf.CreateHeightMap("heightmap", "BetterGrass");

            SystemManager.Instance.AddSystem(new WindowTitleFPSSystem(this));
            SystemManager.Instance.AddSystem(new TransformSystem());
            SystemManager.Instance.AddSystem(new ModelBoundingSphereSystem());
            SystemManager.Instance.AddSystem(new ModelSystem());
            SystemManager.Instance.AddSystem(new KeyBoardSystem());
            SystemManager.Instance.AddSystem(new MovingSystem());
            SystemManager.Instance.AddSystem(new CameraSystem());
            SystemManager.Instance.AddSystem(new PhysicsSystem(true));
            SystemManager.Instance.AddSystem(new EnvironmentSystem());
            SystemManager.Instance.AddSystem(new MouseSystem());
            SystemManager.Instance.AddSystem(new BulletSystem());
            SystemManager.Instance.AddSystem(new PlayerSystem(entf));

            SystemManager.Instance.AddSystem(new HeightmapSystemTexture(Device));


            //SystemManager.Instance.AddSystem(new ConfiguredHeightMapSystem(this));

            base.Initialize();
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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
