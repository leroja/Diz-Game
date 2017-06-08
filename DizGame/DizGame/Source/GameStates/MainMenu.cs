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
    /// <summary>
    /// Mainmenu class is derived from the GameState class
    /// and contains the logic for creating a suitable main menu for
    /// our game
    /// </summary>
    public class MainMenu : GameState
    {
        #region Properties
        /// <summary>
        /// Contiains the entities for this GameState
        /// </summary>
        public override List<int> GameStateEntities { get; }
        private string[] ItemNames;
        private int SelectedItem;
        private SpriteFont SpriteFont;
        private KeyboardState oldState;
        private KeyboardState newState;
        private TextSystem TextSystem;
        private Double time;
        #endregion

        /// <summary>
        /// Basic constructor for the MainMenu class,
        /// initializes basically everything that you should need.
        /// </summary>
        public MainMenu()
        {
            GameStateEntities = new List<int>();
            SelectedItem = 0;
            SpriteFont = GameOne.Instance.Content.Load<SpriteFont>("Fonts/MenuFont");
            oldState = new KeyboardState();
            TextSystem = new TextSystem(SystemManager.Instance.SpriteBatch);
        }

        /// <summary>
        /// Entered Function for the MainMenu class, responsible
        /// for initializing all the options in the menu aswell as 
        /// creating all the different entities for the menu.
        /// </summary>
        public override void Entered()
        {
            time = 1;
            SystemManager.Instance.AddSystem(TextSystem);
            string[] itemNames = { "One Player Game", "Multiplayer Game", "Settings", "Whatever", "Exit" };
            ItemNames = itemNames;

            int y = 30;
            Color color;

            for (int i = 0; i < itemNames.Length; i++)
            {
                Vector2 position = new Vector2(80, y);
                if (i == 0)
                    color = Color.DeepPink;
                else
                    color = Color.LightPink;

                TextComponent textComponent = new TextComponent(itemNames[i], position, color, SpriteFont, true);
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
        /// Removes all the entities created in this state which is not needed anymore.
        /// </summary>
        public override void Exiting()
        {
            AudioManager.Instance.StopSong();

            foreach (int id in GameStateEntities)
            {
                ComponentManager.Instance.RemoveEntity(id);
            }
            SystemManager.Instance.RemoveSystem(TextSystem);
        }

        /// <summary>
        /// Method derrived from the baseclass, usefull if there has to be any changes done 
        /// to specific entities or whatever when another states is pushed ontop of this state.
        /// </summary>
        public override void Obscuring()
        {

        }

        /// <summary>
        /// Method derrived from the baseclass, if there is some changes that needs to be done
        /// when this state is revealed again (if some other state previously have beed obscuring this state)
        /// then that logic should be in this method.
        /// </summary>
        public override void Revealed()
        {
        }

        /// <summary>
        /// Update function for the menu, responsible for handling
        /// the user input to the menu. So that the user will be able 
        /// to chose the different options in the menu.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            time -= gameTime.ElapsedGameTime.TotalSeconds;
            oldState = newState;
            newState = Keyboard.GetState();
            TextComponent txc;
            if (newState.IsKeyDown(Keys.Escape) && !oldState.IsKeyUp(Keys.Escape) && time < 0)
            {
                GameOne.Instance.Exit();
                //SystemManager.Instance.ThreadUpdateSystems.Abort();
            }

            if (newState.IsKeyDown(Keys.Up))
            {
                if (!oldState.IsKeyDown(Keys.Up))
                {
                    if (SelectedItem > 0)
                    {
                        int index;
                        index = GameStateEntities.ElementAt(SelectedItem);

                        txc = ComponentManager.Instance.GetEntityComponent<TextComponent>(index);
                        txc.Color = Color.LightPink;
                        SelectedItem--;

                        index = GameStateEntities.ElementAt(SelectedItem);
                        txc = ComponentManager.Instance.GetEntityComponent<TextComponent>(index);
                        txc.Color = Color.DeepPink;

                        AudioManager.Instance.PlaySoundEffect("MenuChange", 0, 0);
                    }
                }
            }
            if (newState.IsKeyDown(Keys.Down))
            {
                if (!oldState.IsKeyDown(Keys.Down))
                {
                    if (SelectedItem < ItemNames.Length - 1)
                    {
                        int index = GameStateEntities.ElementAt(SelectedItem);
                        txc = ComponentManager.Instance.GetEntityComponent<TextComponent>(index);
                        txc.Color = Color.LightPink;
                        SelectedItem++;
                        index = GameStateEntities.ElementAt(SelectedItem);
                        txc = ComponentManager.Instance.GetEntityComponent<TextComponent>(index);
                        txc.Color = Color.DeepPink;
                        AudioManager.Instance.PlaySoundEffect("MenuChange", 0, 0);
                    }
                }
            }
            if (newState.IsKeyDown(Keys.M) && !oldState.IsKeyDown(Keys.M))
            {
                if (AudioManager.Instance.IsMuted())
                    AudioManager.Instance.GlobalUnMute();
                else
                    AudioManager.Instance.GlobalMute();
            }

            if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
            {
                switch (SelectedItem)
                {
                    case 0:
                        PlayGameState newGame = new PlayGameState(false);
                        GameStateManager.Instance.Pop();
                        GameStateManager.Instance.Push(newGame);
                        break;
                    case 1:
                        PlayGameState newMultGame = new PlayGameState(true);
                        GameStateManager.Instance.Pop();
                        GameStateManager.Instance.Push(newMultGame);
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        GameOne.Instance.Exit();
                        //SystemManager.Instance.ThreadUpdateSystems.Abort();
                        break;
                }
            }
            oldState = newState;
        }
    }
}
