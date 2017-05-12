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

        /// <summary>
        /// Basic constructor for the Game
        /// </summary>
        public GameOne()
        {
            instance = this;
            client = new NetworkSystem();
            client.RunClient();
            this.IsMouseVisible = true;

            
        }
        /// <summary>
        /// Instance of the game
        /// </summary>
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

            Graphics.PreferredBackBufferHeight = Device.DisplayMode.Height / 2;
            Graphics.PreferredBackBufferWidth = Device.DisplayMode.Width / 2;
            Graphics.ApplyChanges();

            SpriteBatch = new SpriteBatch(GraphicsDevice);
            SystemManager.Instance.SpriteBatch = SpriteBatch;

            client.DiscoverLocalPeers();

            MainMenu startState = new MainMenu();
            GameStateManager.Instance.Push(startState);
            


            base.Initialize();
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

        /// <summary>
        /// Updateloop for the game, other stuff that's not updated in the Gameengine could be handled here.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            //Mouse.SetPosition(Device.Viewport.Width / 2, Device.Viewport.Height / 2);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

      


    }
}
