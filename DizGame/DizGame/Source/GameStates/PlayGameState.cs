using DizGame.Source.Components;
using DizGame.Source.Factories;
using DizGame.Source.Random_Stuff;
using DizGame.Source.Systems;
using GameEngine.Source.Components;
using GameEngine.Source.Components.Abstract_Classes;
using GameEngine.Source.Managers;
using GameEngine.Source.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        /// List with ints representing the entity id's 'active' in this current state
        /// </summary>
        public override List<int> GameStateEntities { get; }
        private static EntityTracingSystem EntityTracingSystem { get; set; }
        private bool multiplayerGame;

        private Border border = new Border()
        {
            HighX = 954,
            LowX = 73,
            HighZ = -943,
            LowZ = -57
        };

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
            ComponentManager.Instance.ClearManager();
            EntityFactory.Instance.CreateWorldComp();

            GameStateEntities.Add(EntityFactory.Instance.CreateNewSkyBox());

            if (multiplayerGame)
            {
                CreateEntitiesForMultiplayerGame();
            }
            else
            {
                CreateEntitiesForSinglePlayerGame();
            }
            InitializeSystems();

            AudioManager.Instance.PlaySong("GameSong");
            AudioManager.Instance.ChangeSongVolume(0.15f);
            AudioManager.Instance.ChangeGlobalSoundEffectVolume(1f);
        }

        /// <summary>
        /// Exiting function to remove all the entities which is no longer needed.
        /// </summary>
        public override void Exiting()
        {
            AudioManager.Instance.ChangeSongVolume(1f);
            AudioManager.Instance.StopSong();
            List<int> ScoreID = ComponentManager.Instance.GetAllEntitiesWithComponentType<ScoreComponent>();
            foreach (var id in ScoreID)
            {
                GameStateEntities.Remove(id);
                if (ComponentManager.Instance.CheckIfEntityHasComponent<TextComponent>(id))
                {
                    ComponentManager.Instance.RemoveComponentFromEntity(id, ComponentManager.Instance.GetEntityComponent<TextComponent>(id));
                }
            }

            foreach (int entity in GameStateEntities)
            {
                ComponentManager.Instance.RemoveEntity(entity);
            }
            SystemManager.Instance.ClearSystems();
        }

        /// <summary>
        /// Hides everything visible in the state, except the time and the "HUD", dunno if
        /// we want to hide that. Since that in case the state is obscured, we don't really "want to leave it"
        /// we might just wanna check our inventory or such. Needs more adjusting if we want to manage a paused state
        /// only in the single player mode that is.
        /// </summary>
        public override void Obscuring()
        {
            foreach (int entityId in GameStateEntities)
            {
                ModelComponent mc = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entityId);
                HeightmapComponent hmct = ComponentManager.Instance.GetEntityComponent<HeightmapComponent>(entityId);
                if (mc != null)
                    mc.IsVisible = false;
                if (hmct != null)
                    hmct.IsVisible = false;

                EntityFactory.Instance.VisibleBullets = false;
            }
        }

        /// <summary>
        /// Method to show everything again that's been hidden in case of an obscuring state
        /// might also need some adjustment if we wanna handle a paused state, in single player mode that is.
        /// </summary>
        public override void Revealed()
        {
            foreach (int entityId in GameStateEntities)
            {
                ModelComponent mc = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entityId);
                HeightmapComponent hmct = ComponentManager.Instance.GetEntityComponent<HeightmapComponent>(entityId);
                if (mc != null)
                    mc.IsVisible = true;
                if (hmct != null)
                    hmct.IsVisible = true;

                EntityFactory.Instance.VisibleBullets = true;
            }
        }

        /// <summary>
        /// Method to run during the update part of the game, should contain logic
        /// for exiting the gamestate.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                ScoreScreen Score = new ScoreScreen();
                GameStateManager.Instance.Pop();
                GameStateManager.Instance.Push(Score);
            }

            if (CheckEndCriteria())
            {
                ScoreScreen Score = new ScoreScreen();
                GameStateManager.Instance.Pop();
                GameStateManager.Instance.Push(Score);
            }
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
            SystemManager.Instance.AddSystem(new AISystem());
            SystemManager.Instance.AddSystem(new SmokeSystem());
            SystemManager.Instance.AddSystem(new SoundEffectSystem());
            SystemManager.Instance.AddSystem(new _3DSoundSystem());
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
            //SystemManager.Instance.AddSystem(new BoundingSphereRenderer(GameOne.Instance.GraphicsDevice));
            //SystemManager.Instance.AddSystem(new BoundingBoxRenderer(GameOne.Instance.GraphicsDevice));
            SystemManager.Instance.AddSystem(cSys);
            SystemManager.Instance.AddSystem(new HudSystem());
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
                new Vector2(105, -105),
                new Vector2(290, -105),
                new Vector2(290, -290),
                new Vector2(105, -290),
            };


            List<int> aiEntityList = new List<int>
            {
                entf.CreateAI("Dude", new Vector3(100, 45, -800), 7, border, 3f, MathHelper.Pi, 0.9f, 100, 50, 0.7f, 1f, null, 150, 9, "AI 1"),
                entf.CreateAI("Dude", new Vector3(500, 39, -500), 7, border, 2.5f, MathHelper.Pi, 1.5f, 70f, 35f, 0.7f, 1f, null, 150, 7, "AI 2"),
                entf.CreateAI("Dude", new Vector3(800, 45, -150), 7, border, 2f, MathHelper.Pi, 0.2f, 25f, 15f, 0.7f, 1f, null, 150, 5, "AI 3"),
                entf.CreateAI("Dude", new Vector3(100, 39, -105), 7, border, 1, MathHelper.Pi, 1.5f, 75f, 50f, 0.7f, 1f, waypointList, 90, 2, "Patrolling AI"),
                entf.CreateAI("Dude", new Vector3(800, 45, -800), 7, border, 3f, MathHelper.Pi, 0.1f, 100, 50, 0.7f, 1f, null, 350, 1, "AI 5"),
                entf.CreateAI("Dude", new Vector3(105, 45, -150), 7, border, 3f, MathHelper.Pi, 0.3f, 100, 50, 0.7f, 1f, null, 350, 3, "AI 6"),
                entf.CreateAI("Dude", new Vector3(250, 45, -110), 7, border, 3f, MathHelper.Pi, 0.3f, 100, 50, 0.7f, 1f, null, 350, 3, "AI 7"),
                entf.CreateAI("Dude", new Vector3(150, 45, -800), 7, border, 3f, MathHelper.Pi, 0.3f, 100,50, 0.7f, 1f, null, 350, 3, "AI 8"),
                entf.CreateAI("Dude", new Vector3(250, 45, -500), 7, border, 3f, MathHelper.Pi, 0.3f, 100, 50, 0.7f, 1f, null, 350, 3, "AI 9"),
                entf.CreateAI("Dude", new Vector3(500, 45, -600), 7, border, 3f, MathHelper.Pi, 0.3f, 100, 50, 0.7f, 1f, null, 350, 3, "AI 10"),
            };
            GameStateEntities.AddRange(aiEntityList);


            GameStateEntities.Add(entf.PlaceCrossHair(new Vector2(GameOne.Instance.GraphicsDevice.Viewport.Width / 2, GameOne.Instance.GraphicsDevice.Viewport.Height / 2 + 20f)));

            var idC = entf.CreateDude("Player 1");
            entf.AddPOVCamToEntity(idC);
            //Add entity for the dude to this state
            GameStateEntities.Add(idC);

            int heightmapID = entf.CreateHeightMap("Map3", "BetterGrass", 10);
            //Add heightmap entity id to this state
            GameStateEntities.Add(heightmapID);

            //Add all static objects to this state i.e rocks, houses etc.
            List<int> entityIdList = entf.SGOFactory.MakeMap(10, 100, border);
            GameStateEntities.AddRange(entityIdList);

            entf.CreateParticleEmitter(new Vector3(5, 39, -45), "Smoke", 15);

            int HudID = entf.HudFactory.CreateHud(new Vector2(30, GameOne.Instance.GraphicsDevice.Viewport.Height - 50),
                new Vector2(GameOne.Instance.GraphicsDevice.Viewport.Width / 10, GameOne.Instance.GraphicsDevice.Viewport.Height - 50),
                new Vector2(0, 0), new List<Vector2>(), idC);
            //Add HUD id to this state
            GameStateEntities.Add(HudID);

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

        /// <summary>
        /// Function for deciding if criteria for endgame has been found.
        /// </summary>
        /// <returns></returns>
        private bool CheckEndCriteria()
        {
            List<int> numberOfPlayersAlive = new List<int>();
            numberOfPlayersAlive.AddRange(ComponentManager.Instance.GetAllEntitiesWithComponentType<PlayerComponent>());
            numberOfPlayersAlive.AddRange(ComponentManager.Instance.GetAllEntitiesWithComponentType<AIComponent>());
            if (numberOfPlayersAlive.Count <= 1)
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }
    }
}