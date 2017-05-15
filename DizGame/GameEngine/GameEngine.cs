using GameEngine.Source.Managers;
using GameEngine.Source.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameEngine : Game
    {
        public GraphicsDeviceManager Graphics { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public GraphicsDevice Device { get; set; } 

        /// <summary>
        /// Constructor
        /// </summary>
        public GameEngine()
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = false
            };
            IsFixedTimeStep = false;
            Content.RootDirectory = "Content";
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            SystemManager.Instance.RunUpdateSystems(gameTime);
           
            GameStateManager.Instance.UpdateGameState();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            SystemManager.Instance.RunRenderSystems(gameTime);

            base.Draw(gameTime);
        }
    }
}
