using DizGame.Source.Systems;
using GameEngine.Source.Components.Abstract_Classes;
using GameEngine.Source.Managers;
using GameEngine.Source.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.GameStates
{
    public class PlayGameState : GameState
    {
        #region Properties
        public override List<int> GameStateEntities { get; }
        public static EntityTracingSystem EntityTracingSystem { get; private set; }
        #endregion

        public PlayGameState()
        {
            GameStateEntities = new List<int>();
        }

        public override void Entered()
        {
            EntityFactory entf = EntityFactory.Instance;
            
            entf.CreateAI("Dude", new Vector3(30, 45, -10), 25, 100, 300, 5f, MathHelper.Pi);
            entf.CreateAI("Dude", new Vector3(40, 45, -10), 25, 100, 300, 3.5f, MathHelper.Pi);
            entf.CreateAI("Dude", new Vector3(50, 45, -10), 25, 100, 300, 2f, MathHelper.Pi);
            var idC = EntityFactory.Instance.CreateDude();
            entf.AddChaseCamToEntity(idC, new Vector3(0, 10, 25));

            entf.CreateHeightMap("canyonHeightMap", "BetterGrass", 10);

            entf.MakeMap(10, 100);

            entf.CreateHud(new Vector2(30, GameOne.Instance.GraphicsDevice.Viewport.Height - 50),
                new Vector2(GameOne.Instance.GraphicsDevice.Viewport.Width / 10, GameOne.Instance.GraphicsDevice.Viewport.Height - 50),
                new Vector2(0, 0), new List<Vector2>());

            InitializeSystems(entf);
        }

        public override void Exiting()
        {
            throw new NotImplementedException();
        }

        public override void Obscuring()
        {
            throw new NotImplementedException();
        }

        public override void Revealed()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
        }

        private void InitializeSystems(EntityFactory entf)
        {
            
            //entf.CreateHeightMap("heightmap", "BetterGrass");
            SystemManager.Instance.AddSystem(new TransformSystem());
            SystemManager.Instance.AddSystem(new ModelBoundingSphereSystem());

            SystemManager.Instance.AddSystem(new TransformSystem());
            SystemManager.Instance.AddSystem(new ModelBoundingSphereSystem());
            
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

            
            SystemManager.Instance.AddSystem(new AnimationSystem());
            SystemManager.Instance.AddSystem(new AISystem());

            

            EntityTracingSystem = new EntityTracingSystem();
            EntityTracingSystem.RecordInitialEntities();
            SystemManager.Instance.AddSystem(EntityTracingSystem);

            SystemManager.Instance.AddSystem(new WindowTitleFPSSystem(GameOne.Instance));
            SystemManager.Instance.AddSystem(new WorldSystem(GameOne.Instance));
            SystemManager.Instance.AddSystem(new HeightmapSystemTexture(GameOne.Instance.GraphicsDevice));
            SystemManager.Instance.AddSystem(new _2DSystem(SystemManager.Instance.SpriteBatch));
            

        }
    }
}
