using DizGame.Source.Systems;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DizGame.Source.GameStates;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Threading;

namespace DizGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameOne : GameEngine.GameEngine
    {
        private static Game instance;

        /// <summary>
        /// 
        /// </summary>
        public static Rectangle bounds;

        /// <summary>
        /// Basic constructor for the Game
        /// </summary>
        public GameOne()
        {
            instance = this;
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



            AudioManager.Instance.AddSong("MenuSong", Content.Load<Song>("Songs/MenuSong"));
            AudioManager.Instance.AddSong("GameSong", Content.Load<Song>("Songs/GameSong"));
            AudioManager.Instance.AddSong("LobbySong", Content.Load<Song>("Songs/LobbySong"));

            AudioManager.Instance.AddSoundEffect("ShotEffect", Content.Load<SoundEffect>("SoundEffects/Gun-Shot"));
            AudioManager.Instance.AddSoundEffect("MenuChange", Content.Load<SoundEffect>("SoundEffects/menu-selection-sound"));
            AudioManager.Instance.AddSoundEffect("Ammo-Pickup", Content.Load<SoundEffect>("SoundEffects/Chambering A Round"));
            AudioManager.Instance.AddSoundEffect("Kill-Sound", Content.Load<SoundEffect>("SoundEffects/Anti Aircraft Gun"));
            AudioManager.Instance.AddSoundEffect("Kill-Sound1", Content.Load<SoundEffect>("SoundEffects/Blast"));
            AudioManager.Instance.AddSoundEffect("DeathSound", Content.Load<SoundEffect>("SoundEffects/NoNo"));
            AudioManager.Instance.AddSoundEffect("HealthPickUp", Content.Load<SoundEffect>("SoundEffects/Pick Up Health"));
            AudioManager.Instance.AddSoundEffect("Reload", Content.Load<SoundEffect>("SoundEffects/AK-47 Reload"));

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
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Update loop for the game, other stuff that's not updated in the Game Engine could be handled here.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}