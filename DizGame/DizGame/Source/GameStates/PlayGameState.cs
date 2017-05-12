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

        /// <summary>
        /// Basic constructor for the PlayGame-state 
        /// </summary>
        public PlayGameState()
        {
            GameStateEntities = new List<int>();
        }

        /// <summary>
        /// Entered function to initialize all the needed entities which we need for
        /// the gameplay.
        /// </summary>
        public override void Entered()
        {
            EntityFactory entf = EntityFactory.Instance;
            
            entf.CreateAI("Dude", new Vector3(30, 45, -80), 25, 300, 300, 5f, MathHelper.Pi);
            entf.CreateAI("Dude", new Vector3(65, 45, -10), 25, 300, 300, 3.5f, MathHelper.Pi);
            entf.CreateAI("Dude", new Vector3(135, 45, -50), 25, 300, 300, 2f, MathHelper.Pi);
            var idC = EntityFactory.Instance.CreateDude();
            entf.AddChaseCamToEntity(idC, new Vector3(0, 10, 25));
            //Add entity for the dude to this state
            GameStateEntities.Add(idC);

            int heightmapID = entf.CreateHeightMap("canyonHeightMap", "BetterGrass", 10);
            //Add hightmap entityid to this state
            GameStateEntities.Add(heightmapID);

            //Add all static objects to this state i.e rocks, houses etc.
            List<int> entityIdList = entf.MakeMap(10, 100);
            GameStateEntities.AddRange(entityIdList);

            InitializeSystems();

            
            int HudID = entf.CreateHud(new Vector2(30, GameOne.Instance.GraphicsDevice.Viewport.Height - 50),
                new Vector2(GameOne.Instance.GraphicsDevice.Viewport.Width / 10, GameOne.Instance.GraphicsDevice.Viewport.Height - 50),
                new Vector2(0, 0), new List<Vector2>());

            //Add HUD id to this state
            GameStateEntities.Add(HudID);

           
        }

        /// <summary>
        /// Exiting function to remove all the entities which is no longer needed.
        /// </summary>
        public override void Exiting()
        {
            //TODO: observera att vi kanske inte vill ta bort precis alla entiteter i detta statet,
            //Tex vill vi kanske ha kvar spelarna för att göra typ en "score-screen" i slutet.
            foreach (int entity in GameStateEntities)
                ComponentManager.Instance.RemoveEntity(entity);
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

        /// <summary>
        /// Seperate method for initializing all the systems required by this state
        /// just in order to make the code more readable.
        /// </summary>
        private void InitializeSystems()
        {

            SystemManager.Instance.AddSystem(new ModelSystem());
            SystemManager.Instance.AddSystem(new HeightmapSystemTexture(GameOne.Instance.GraphicsDevice));

            SystemManager.Instance.AddSystem(new TransformSystem());
            SystemManager.Instance.AddSystem(new ModelBoundingSphereSystem());

            SystemManager.Instance.AddSystem(new TransformSystem());
            SystemManager.Instance.AddSystem(new ModelBoundingSphereSystem());

            SystemManager.Instance.AddSystem(new TransformSystem());
            SystemManager.Instance.AddSystem(new ModelBoundingSphereSystem());
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
            SystemManager.Instance.AddSystem(new _2DSystem(SystemManager.Instance.SpriteBatch));
            SystemManager.Instance.AddSystem(new TextSystem(SystemManager.Instance.SpriteBatch));



        }
    }
}
