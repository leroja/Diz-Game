using DizGame.Source.Systems;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DizGame.Source.GameStates;
using DizGame.Source.LanguageBasedModels;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Threading;

namespace DizGame
{

    // kolla på allting 23 fps
    // kolla på nästan ingenting ca 40 fps

    // efter parallel foreach ca 41 för nästan ingenting och allting 24

    // har dock inte gjort på några utritnings system och de som har med kollisionerna att göra

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameOne : GameEngine.GameEngine
    {
        NetworkSystem client;

        private static Game instance;
        public static Rectangle bounds;

        /// <summary>
        /// Basic constructor for the Game
        /// </summary>
        public GameOne()
        {
            instance = this;
            client = new NetworkSystem();
            client.RunClient();
            //this.IsMouseVisible = true;
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
            bounds = Window.ClientBounds;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            SystemManager.Instance.SpriteBatch = SpriteBatch;

            client.DiscoverLocalPeers();
            
            AudioManager.Instance.AddSong("MenuSong", Content.Load<Song>("Songs/MenuSong"));
            AudioManager.Instance.AddSong("GameSong", Content.Load<Song>("Songs/GameSong"));
            AudioManager.Instance.AddSoundEffect("ShotEffect", Content.Load<SoundEffect>("SoundEffects/Gun-Shot"));

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
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
