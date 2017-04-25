using DizGame.Source.Systems;
using Lidgren.Network;
using GameEngine.Source.Managers;
using GameEngine.Source.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            client.DiscoverLocalPeers();
            EntityFactory entf = new EntityFactory(Content);
            //entf.CreateDefaultCamera();
            entf.CreateChuckGreen();
            entf.CreateKitana();
            Model bullet = Content.Load<Model>("bullet");
            //entf.CreateBullet(bullet, new Vector3(10,10,10), new );

            SystemManager.Instance.AddSystem(new WindowTitleFPSSystem(this));
            SystemManager.Instance.AddSystem(new ModelSystem());
            SystemManager.Instance.AddSystem(new TransformSystem());
            SystemManager.Instance.AddSystem(new KeyBoardSystem());
            SystemManager.Instance.AddSystem(new MovingSystem());
            SystemManager.Instance.AddSystem(new CameraSystem());
            SystemManager.Instance.AddSystem(new MouseSystem());

            SystemManager.Instance.AddSystem(new BulletSystem());


            base.Initialize();

            Mouse.SetPosition(Device.Viewport.Width / 2, Device.Viewport.Height / 2);
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
