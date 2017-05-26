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
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.GameStates
{
    class ScoreScreen : GameState
    {
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
        /// Basic constructor for ScoreSreen
        /// </summary>
        public ScoreScreen()
        {
            GameStateEntities = new List<int>();
            SpriteFont = GameOne.Instance.Content.Load<SpriteFont>("Fonts/MenuFont");
            oldState = Keyboard.GetState();
            TextSystem = new TextSystem(SystemManager.Instance.SpriteBatch);
        }

        /// <summary>
        /// metod calld uppon entering gamestate. fixes whit potitions for text and prints the score list 
        /// </summary>
        public override void Entered()
        {
            int y = 50;
            Color color;
            TextComponent textComponent;
            SystemManager.Instance.AddSystem(TextSystem);
            List<int> ids = GetScoreStatistics();
            for (int i = 0; i < ids.Count; i++)
            {
                Vector2 position = new Vector2(80, y);
                color = Color.LightPink;
                ScoreComponent score = ComponentManager.Instance.GetEntityComponent<ScoreComponent>(ids[i]);
                if (i == 0)
                {
                    textComponent = new TextComponent(score.NameOfScorer + "        " + score.Score+ "      "+"Winner", position, color, SpriteFont, true);
                }
                else
                {
                    textComponent = new TextComponent(score.NameOfScorer + "        " + score.Score, position, color, SpriteFont, true);
                }
                int entityID = ComponentManager.Instance.CreateID();
                GameStateEntities.Add(entityID);
                ComponentManager.Instance.AddComponentToEntity(entityID, textComponent);
                y += 40;
            }
            AudioManager.Instance.PlaySong("MenuSong");
            AudioManager.Instance.ChangeRepeat();
            AudioManager.Instance.ChangeSongVolume(0.5f);
        }

        /// <summary>
        /// Fixes the list whit Scorers so it is in order for printing
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
            
            List<ScoreComponent> orderdList = Score.OrderBy(o => o.Score).ToList();
            foreach (var scorecomp in orderdList)
            {
                temp.Add(scorecomp.ID);
            }
            temp.Reverse();
            return temp;
        }

        /// <summary>
        /// Exiting function to remove all the entities which is no longer needed.
        /// </summary>
        public override void Exiting()
        {
            AudioManager.Instance.StopSong();
            List<int> tep = ComponentManager.Instance.GetAllEntitiesWithComponentType<ScoreComponent>();
            GameStateEntities.AddRange(tep);
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
        /// Not inplemented
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
