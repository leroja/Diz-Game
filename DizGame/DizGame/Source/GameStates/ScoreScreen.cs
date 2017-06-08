using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Components.Abstract_Classes;
using GameEngine.Source.Managers;
using GameEngine.Source.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DizGame.Source.GameStates
{
    class ScoreScreen : GameState
    {
        //private const int start = 55;
        private const int BetweenNameAndScore = 155;
        private const int BetweenScoreAndKills = 255;
        private const int BetweenKillsAndHits = 355;
        private const int BetweenHitsAndAdditional = 455;

        #region Properties
        /// <summary>
        /// List whit Entities. Used for cntolling entites in state
        /// </summary>
        public override List<int> GameStateEntities { get; }
        /// <summary>
        /// Spritefont used for printing text
        /// </summary>
        private SpriteFont SpriteFont;
        /// <summary>
        /// The previuc keuboardstate to handel input
        /// </summary>
        private KeyboardState oldState;
        /// <summary>
        /// The newest keyboardstate to chek input. ussed to get to next stage
        /// </summary>
        private KeyboardState newState;
        /// <summary>
        /// Textsystem for printing the txt in the scorelist
        /// </summary>
        private TextSystem TextSystem;
        #endregion

        /// <summary>
        /// Basic constructor for ScoreScreen
        /// </summary>
        public ScoreScreen()
        {
            GameStateEntities = new List<int>();
            SpriteFont = GameOne.Instance.Content.Load<SpriteFont>("Fonts/MenuFont");
            oldState = Keyboard.GetState();
            TextSystem = new TextSystem(SystemManager.Instance.SpriteBatch);
        }

        /// <summary>
        /// method called upon entering gamestate. fixes with positions for text and prints the score list 
        /// </summary>
        public override void Entered()
        {
            var world = ComponentManager.Instance.GetAllEntitiesWithComponentType<WorldComponent>();
            var ev = ComponentManager.Instance.GetEntityComponent<TextComponent>(world.FirstOrDefault());
            ev.Color = Color.Black;
            ev.Position = new Vector2(700, 10);
            
            SystemManager.Instance.AddSystem(TextSystem);

            int entiID = ComponentManager.Instance.CreateID();
            GameStateEntities.Add(entiID);

            var longestNameLenght = GetLongestName().Length;
            int start = 55 - longestNameLenght;

            List<string> names = new List<string>()
            {
                "Name",
                "Score",
                "Kills",
                "Hits",
                "Additional Stuff"
            };
            List<TextComponent> li = new List<TextComponent>()
            {
                new TextComponent("Name", new Vector2(start, 10), Color.Navy, SpriteFont, true),
                new TextComponent("Score", new Vector2(BetweenNameAndScore, 10), Color.Navy, SpriteFont, true),
                new TextComponent("Kills", new Vector2(BetweenScoreAndKills, 10), Color.Navy, SpriteFont, true),
                new TextComponent("Hits", new Vector2(BetweenKillsAndHits, 10), Color.Navy, SpriteFont, true),
                new TextComponent("Awards", new Vector2(BetweenHitsAndAdditional, 10), Color.Navy, SpriteFont, true), 
            };

            var textComp = new TextComponent(names, li);
            ComponentManager.Instance.AddComponentToEntity(entiID, textComp);
            
            int y = 50;
            TextComponent textComponent;
            List<int> ids = GetScoreStatistics();
            for (int i = 0; i < ids.Count; i++)
            {
                ScoreComponent score = ComponentManager.Instance.GetEntityComponent<ScoreComponent>(ids[i]);
                
                li = new List<TextComponent>()
                {
                    new TextComponent(score.NameOfScorer, new Vector2(start, y), Color.DeepPink, SpriteFont, true),
                    new TextComponent(score.Score.ToString(), new Vector2(BetweenNameAndScore + 10, y), Color.OrangeRed, SpriteFont, true),
                    new TextComponent(score.Kills.ToString(), new Vector2(BetweenScoreAndKills + 10, y), Color.DarkRed, SpriteFont, true),
                    new TextComponent(score.Hits.ToString(), new Vector2(BetweenKillsAndHits + 10, y), Color.YellowGreen, SpriteFont, true),
                    new TextComponent(score.Awards, new Vector2(BetweenHitsAndAdditional, y), Color.Navy, SpriteFont, true),
                };

                textComponent = new TextComponent(names, li);

                int entityID = ComponentManager.Instance.CreateID();
                GameStateEntities.Add(entityID);
                ComponentManager.Instance.AddComponentToEntity(entityID, textComponent);
                y += 40;
            }

            int entID = ComponentManager.Instance.CreateID();
            GameStateEntities.Add(entID);
            textComp = new TextComponent("Press Space to continue", new Vector2(GameOne.Instance.Window.ClientBounds.Width / 2 - 100, GameOne.Instance.Window.ClientBounds.Height - 50), Color.Black, SpriteFont, true);
            ComponentManager.Instance.AddComponentToEntity(entID, textComp);

            AudioManager.Instance.PlaySong("MenuSong");
            AudioManager.Instance.ChangeRepeat();
            AudioManager.Instance.ChangeSongVolume(0.5f);
        }

        /// <summary>
        /// Fixes the list with Scorers so it is in order for printing
        /// </summary>
        /// <returns></returns>
        private List<int> GetScoreStatistics()
        {
            List<ScoreComponent> Score = new List<ScoreComponent>();
            List<int> temp = new List<int>();
            var a = ComponentManager.Instance.GetAllEntitiesWithComponentType<ScoreComponent>();
            foreach (var id in a)
            {
                Score.Add(ComponentManager.Instance.GetEntityComponent<ScoreComponent>(id));
            }

            List<ScoreComponent> orderdList = Score.OrderByDescending(o => o.Score).ToList();
            orderdList[0].Awards += " Winner,";
            foreach (var scorecomp in orderdList)
            {
                temp.Add(scorecomp.ID);
            }

            orderdList = Score.OrderByDescending(o => o.Kills).ToList();
            orderdList[0].Awards += " Most Kills,";
            orderdList = Score.OrderByDescending(o => o.Hits).ToList();
            orderdList[0].Awards += " Most Hits,";

            return temp;
        }

        /// <summary>
        /// Find the longest name of the all the Players/AIs
        /// </summary>
        /// <returns></returns>
        private string GetLongestName()
        {
            List<ScoreComponent> Score = new List<ScoreComponent>();
            List<string> temp = new List<string>();
            var a = ComponentManager.Instance.GetAllEntitiesWithComponentType<ScoreComponent>();
            foreach (var id in a)
            {
                Score.Add(ComponentManager.Instance.GetEntityComponent<ScoreComponent>(id));
            }

            return Score.OrderByDescending(o => o.NameOfScorer).ToList()[0].NameOfScorer;
        }

        /// <summary>
        /// Exiting function to remove all the entities which is no longer needed.
        /// </summary>
        public override void Exiting()
        {
            AudioManager.Instance.StopSong();
            GameStateEntities.AddRange(ComponentManager.Instance.GetAllEntitiesWithComponentType<ScoreComponent>());
            GameStateEntities.AddRange(ComponentManager.Instance.GetAllEntitiesWithComponentType<WorldComponent>());
            foreach (int id in GameStateEntities)
            {
                ComponentManager.Instance.RemoveEntity(id);
            }
            SystemManager.Instance.RemoveSystem(TextSystem);
        }

        /// <summary>
        /// not implemented
        /// </summary>
        public override void Obscuring()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public override void Revealed()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to run durring the update part of the game, should contain logic
        /// for exiting the gamestate.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            oldState = newState;
            newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Space) && !oldState.IsKeyDown(Keys.Space))
            {
                MainMenu NewMainMenu = new MainMenu();
                GameStateManager.Instance.Pop();
                GameStateManager.Instance.Push(NewMainMenu);
            }
            if (newState.IsKeyDown(Keys.M) && !oldState.IsKeyDown(Keys.M))
            {
                if (AudioManager.Instance.IsMuted())
                    AudioManager.Instance.GlobalUnMute();
                else
                    AudioManager.Instance.GlobalMute();
            }
            oldState = newState;
        }
    }
}