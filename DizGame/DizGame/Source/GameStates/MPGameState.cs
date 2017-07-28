using GameEngine.Source.Components.Abstract_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DizGame.Source.Factories;
using GameEngine.Source.Managers;
using GameEngine.Source.Systems;
using Microsoft.Xna.Framework.Graphics;
using DizGame.Source.Systems;
using DizGame.Source.Random_Stuff;
using GameEngine.Source.NetworkStuff;
using DizGame.Source.NetworkStuff;

namespace DizGame.Source.GameStates
{
    /// <summary>
    /// A game state for multiplayer
    /// </summary>
    public class MPGameState : GameState
    {

        /// <summary>
        /// 
        /// </summary>
        public override List<int> GameStateEntities { get; }

        private Border border = new Border()
        {
            HighX = 954,
            LowX = 73,
            HighZ = -943,
            LowZ = -57
        };

        /// <summary>
        /// 
        /// </summary>
        public MPGameState(NetworkSystem netSystem)
        {
            GameStateEntities = new List<int>();

            var netSyncSys = new NetworkSyncSystem();
            netSystem.ScanForPeers = false;
            netSystem.Subscribe(netSyncSys);
            netSyncSys.Subscribe(netSystem);

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Entered()
        {
            ComponentManager.Instance.ClearManager();
            EntityFactory.Instance.CreateWorldComp();

            GameStateEntities.Add(EntityFactory.Instance.CreateNewSkyBox());

            InitializeSystems();

            AudioManager.Instance.PlaySong("GameSong");
            AudioManager.Instance.ChangeSongVolume(0.15f);
            AudioManager.Instance.ChangeGlobalSoundEffectVolume(1f);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Exiting()
        {
            AudioManager.Instance.ChangeSongVolume(1f);
            AudioManager.Instance.StopSong();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Obscuring()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Revealed()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Separate method for initializing all the systems required by this state
        /// just in order to make the code more readable.
        /// </summary>
        private void InitializeSystems()
        {
            SystemManager.Instance.AddSystem(new SkyboxSystem(GameOne.Instance.GraphicsDevice));
            AmmunitionSystem ammo = new AmmunitionSystem();
            CollisionSystem cSys = new CollisionSystem();
            PhysicsSystem pSys = new PhysicsSystem();
            cSys.Subscribe(new HealthSystem());
            cSys.Subscribe(ammo);
            cSys.Subscribe(new StaticColisionSystem());
            cSys.Subscribe(pSys);
            cSys.Subscribe(new HandleCollisionSystem());
            SystemManager.Instance.AddSystem(pSys);
            SystemManager.Instance.AddSystem(ammo);
            SystemManager.Instance.AddSystem(new ModelSystem(GameOne.Instance.GraphicsDevice));
            SystemManager.Instance.AddSystem(new HeightmapSystem(GameOne.Instance.GraphicsDevice));
            SystemManager.Instance.AddSystem(new TransformSystem());
            SystemManager.Instance.AddSystem(new KeyBoardSystem());
            SystemManager.Instance.AddSystem(new MovingSystem(border));
            SystemManager.Instance.AddSystem(new CameraSystem());
            SystemManager.Instance.AddSystem(new EnvironmentSystem());
            SystemManager.Instance.AddSystem(new MouseSystem());
            SystemManager.Instance.AddSystem(new BulletSystem());
            SystemManager.Instance.AddSystem(new PlayerSystem(GameOne.bounds));
            SystemManager.Instance.AddSystem(new ParticleRenderSystem(GameOne.Instance.GraphicsDevice));
            SystemManager.Instance.AddSystem(new ParticleUpdateSystem());
            SystemManager.Instance.AddSystem(new AnimationSystem());
            SystemManager.Instance.AddSystem(new SmokeSystem());
            SystemManager.Instance.AddSystem(new SoundEffectSystem());
            SystemManager.Instance.AddSystem(new _3DSoundSystem());
            //SystemManager.Instance.AddSystem(new AISystem());
            SystemManager.Instance.AddSystem(new SpectatingSystem());
            SystemManager.Instance.AddSystem(new ModelBoundingSystem());
            var id = ComponentManager.Instance.CreateID();
            GameStateEntities.Add(id);
            SystemManager.Instance.AddSystem(new FPSSystem(GameOne.Instance.Content.Load<SpriteFont>("Fonts/font"), id));
            SystemManager.Instance.AddSystem(new WorldSystem(GameOne.Instance));
            SystemManager.Instance.AddSystem(new _2DSystem(SystemManager.Instance.SpriteBatch));
            SystemManager.Instance.AddSystem(new TextSystem(SystemManager.Instance.SpriteBatch));
            SystemManager.Instance.AddSystem(new FlareSystem(SystemManager.Instance.SpriteBatch));
            SystemManager.Instance.AddSystem(new ResourceSystem(this.border));
            SystemManager.Instance.AddSystem(cSys);
            SystemManager.Instance.AddSystem(new HudSystem());
        }

    }
}