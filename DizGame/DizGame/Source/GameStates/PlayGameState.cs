using AnimationContentClasses;
using DizGame.Source.Systems;
using GameEngine.Source.Components;
using GameEngine.Source.Components.Abstract_Classes;
using GameEngine.Source.Managers;
using GameEngine.Source.Systems;
using GameEngine.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace DizGame.Source.GameStates
{
    /// <summary>
    /// PlayGameState is a state which represents the basic gameplay logic.
    /// </summary>
    public class PlayGameState : GameState
    {
        #region Properties
        /// <summary>
        /// List with ints representing the entityid's 'active' in this current state
        /// </summary>
        public override List<int> GameStateEntities { get; }
        private static EntityTracingSystem EntityTracingSystem { get; set; }
        private bool multiplayerGame;

        #endregion

        /// <summary>
        /// Basic constructor for the PlayGame-state 
        /// </summary>
        /// <param name="multiplayerGame">a boolean variable, should be set to true if the user 
        /// wants to start a multiplayer game or false if he/she wants to play against AI</param>
        public PlayGameState(bool multiplayerGame)
        {
            GameStateEntities = new List<int>();
            this.multiplayerGame = multiplayerGame;
        }

        /// <summary>
        /// Entered function to initialize all the needed entities which we need for
        /// the gameplay.
        /// </summary>
        public override void Entered()
        {
            InitializeSystems();

            if (multiplayerGame)
            {
                CreateEntitiesForMultiplayerGame();
            }
            else
            {
                CreateEntitiesForSinglePlayerGame();
            }
            AudioManager.Instance.PlaySong("GameSong");
            AudioManager.Instance.ChangeSongVolume(0.25f);
            AudioManager.Instance.ChangeGlobalSoundEffectVolume(0.75f);

        }

        /// <summary>
        /// Exiting function to remove all the entities which is no longer needed.
        /// </summary>
        public override void Exiting()
        {
            AudioManager.Instance.ChangeSongVolume(1f);
            AudioManager.Instance.StopSong();
            //TODO: observera att vi kanske inte vill ta bort precis alla entiteter i detta statet,
            //Tex vill vi kanske ha kvar spelarna + tillhörande componenter för att göra typ en "score-screen" i slutet.
            foreach (int entity in GameStateEntities)
                ComponentManager.Instance.RemoveEntity(entity);
        }

        /// <summary>
        /// Hides everything visible in the state, except the time and the "hud", dunno if
        /// we want to hide that. Since that incase the state is obscured, we dont really "want to leave it"
        /// we might just wanna check our inventory or such. Needs more adjusting if we want to manage a paused state
        /// only in the single player mode that is.
        /// </summary>
        public override void Obscuring()
        {
            foreach (int entityId in GameStateEntities)
            {
                ModelComponent mc = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entityId);
                HeightmapComponentTexture hmct = ComponentManager.Instance.GetEntityComponent<HeightmapComponentTexture>(entityId);
                if (mc != null)
                    mc.IsVisible = false;
                if (hmct != null)
                    hmct.IsVisible = false;

                EntityFactory.Instance.VisibleBullets = false;
            }
        }
        /// <summary>
        /// Method to show everything again that's been hidden incase of an obscuring state
        /// might also need some adjustment if we wanna handle a paused state, in single player mode that is.
        /// </summary>
        public override void Revealed()
        {
            foreach (int entityId in GameStateEntities)
            {
                ModelComponent mc = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entityId);
                HeightmapComponentTexture hmct = ComponentManager.Instance.GetEntityComponent<HeightmapComponentTexture>(entityId);
                if (mc != null)
                    mc.IsVisible = true;
                if (hmct != null)
                    hmct.IsVisible = true;

                EntityFactory.Instance.VisibleBullets = true;
            }
        }
        /// <summary>
        /// Method to run durring the update part of the game, should contain logic
        /// for exiting the gamestate.
        /// </summary>
        public override void Update()
        {
            //Bara för att testa så att obscuring och revealed metoderna fungerar.
            //Allting döljs men det återstår väl o se om tex AI:n fortfarande "rör" sig/och kan
            //attakera under tiden som allting är "dolt" eller vad man ska säga, för updatesen bör fortfarande
            //Göras som vanligt bara att modell-systemet inte ritar ut modellerna liksom
            //KeyboardState state = Keyboard.GetState();
            //if (state.IsKeyDown(Keys.B))
            //    Obscuring();
            //if (state.IsKeyDown(Keys.V))
            //    Revealed();
        }

        /// <summary>
        /// Seperate method for initializing all the systems required by this state
        /// just in order to make the code more readable.
        /// </summary>
        private void InitializeSystems()
        {
            //CollisionSystem cSys = new CollisionSystem();
            PhysicsSystem pSys = new PhysicsSystem();
            //cSys.Subscribe(pSys);
            SystemManager.Instance.AddSystem(pSys);
            //SystemManager.Instance.AddSystem(cSys);
            SystemManager.Instance.AddSystem(new ModelSystem());
            SystemManager.Instance.AddSystem(new HeightmapSystemTexture(GameOne.Instance.GraphicsDevice));
            //SystemManager.Instance.AddSystem(new GameTransformSystem());
            SystemManager.Instance.AddSystem(new TransformSystem());
            SystemManager.Instance.AddSystem(new KeyBoardSystem());
            SystemManager.Instance.AddSystem(new MovingSystem());
            SystemManager.Instance.AddSystem(new CameraSystem());
            SystemManager.Instance.AddSystem(new EnvironmentSystem());
            SystemManager.Instance.AddSystem(new MouseSystem());
            SystemManager.Instance.AddSystem(new BulletSystem());
            SystemManager.Instance.AddSystem(new PlayerSystem());
            SystemManager.Instance.AddSystem(new ParticleRenderSystem(GameOne.Instance.GraphicsDevice));
            SystemManager.Instance.AddSystem(new ParticleUpdateSystem());
            SystemManager.Instance.AddSystem(new AnimationSystem());
            SystemManager.Instance.AddSystem(new AISystem());

            //EntityTracingSystem = new EntityTracingSystem();
            //EntityTracingSystem.RecordInitialEntities();
            //SystemManager.Instance.AddSystem(EntityTracingSystem);
            SystemManager.Instance.AddSystem(new ModelBoundingSystem());
            SystemManager.Instance.AddSystem(new WindowTitleFPSSystem(GameOne.Instance));
            SystemManager.Instance.AddSystem(new WorldSystem(GameOne.Instance));
            SystemManager.Instance.AddSystem(new _2DSystem(SystemManager.Instance.SpriteBatch));
            SystemManager.Instance.AddSystem(new TextSystem(SystemManager.Instance.SpriteBatch));
            SystemManager.Instance.AddSystem(new FlareSystem(SystemManager.Instance.SpriteBatch));
            SystemManager.Instance.AddSystem(new ResourceSystem());
            //SystemManager.Instance.AddSystem(new BoundingSphereRenderer(GameOne.Instance.GraphicsDevice));
            //SystemManager.Instance.AddSystem(new BoundingBoxRenderer(GameOne.Instance.GraphicsDevice));
        }

        /// <summary>
        /// Function for initializing the entities which are needed for a single player game 
        /// against AI
        /// </summary>
        private void CreateEntitiesForSinglePlayerGame()
        {
            EntityFactory entf = EntityFactory.Instance;

            var waypointList = new List<Vector2>()
            {
                new Vector2(5, -5),
                new Vector2(290, -5),
                new Vector2(290, -290),
                new Vector2(5, -290),
            };

            List<int> aiEntityList = new List<int>
            {
                entf.CreateAI("Dude", new Vector3(30, 45, -80), 5, 300, 300, 3f, MathHelper.Pi, 0.9f, 100, 40, 0.7f, 1f, null, 150, 9),
                entf.CreateAI("Dude", new Vector3(65, 39, -10), 5, 300, 300, 2.5f, MathHelper.Pi, 1.5f, 50f, 25f, 0.7f, 1f, null, 150, 7),
                entf.CreateAI("Dude", new Vector3(135, 45, -50), 5, 300, 300, 2f, MathHelper.Pi, 0.2f, 25f, 15f, 0.7f, 1f, null, 150, 5),
                entf.CreateAI("Dude", new Vector3(45, 39, -30), 5, 300, 300, 1, MathHelper.Pi, 1.5f, 15f, 25f, 0.2f, 1f, waypointList, 90, 2),
            };
            GameStateEntities.AddRange(aiEntityList);


            GameStateEntities.Add(entf.PlaceCrossHair(new Vector2(GameOne.Instance.GraphicsDevice.Viewport.Width / 2, GameOne.Instance.GraphicsDevice.Viewport.Height / 2 + 20f)));

            var idC = entf.CreateDude();
            //entf.AddChaseCamToEntity(idC, new Vector3(0, 10, 25), true);
            entf.AddPOVCamToEntity(idC);
            //entf.CreateStaticCam(new Vector3(-20, 45, 20), new Vector3(45, 50, -30));
            //Add entity for the dude to this state
            GameStateEntities.Add(idC);

            int heightmapID = entf.CreateHeightMap("Map3", "BetterGrass", 10);
            //Add hightmap entityid to this state
            GameStateEntities.Add(heightmapID);

            //Add all static objects to this state i.e rocks, houses etc.
            List<int> entityIdList = entf.MakeMap(10, 100);
            GameStateEntities.AddRange(entityIdList);

            entf.CreateParticleEmiter(new Vector3(1, 2000, 1), "Smoke", 400, 10, 30, new Vector3(0, 1, 0), 5, 120);

            int HudID = entf.HudFactory.CreateHud(new Vector2(30, GameOne.Instance.GraphicsDevice.Viewport.Height - 50),
                new Vector2(GameOne.Instance.GraphicsDevice.Viewport.Width / 10, GameOne.Instance.GraphicsDevice.Viewport.Height - 50),
                new Vector2(0, 0), new List<Vector2>());
            //Add HUD id to this state
            GameStateEntities.Add(HudID);

            //var ids = ComponentManager.Instance.GetAllEntitiesWithComponentType<ModelComponent>();
            //foreach (var modelEnt in ids)
            //{
            //    var modelComp = ComponentManager.Instance.GetEntityComponent<ModelComponent>(modelEnt);
            //    var transComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(modelEnt);
            //    modelComp.BoundingVolume = new BoundingVolume(0, new BoundingSphere3D(
            //        new BoundingSphere(new Vector3(transComp.Position.X, transComp.Position.Y, transComp.Position.Z), 3)));
            //}
            entf.SpawnProtection();
        }

        /// <summary>
        /// Function for initializing all the entities which are needed for a 
        /// multiplayer game.
        /// </summary>
        private void CreateEntitiesForMultiplayerGame()
        {
            //TODO: initziera alla entiteter som krävs för ett multiplayer game, eventuellt om vi 
            //"Redirictar" till en lobby eftersom att vi behöver hitta alla klienter och så först, kanske borde skapa ett helt nytt state för detta ändå?
            throw new NotImplementedException();
        }
    }
}
