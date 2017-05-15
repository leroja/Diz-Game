using DizGame.Source.Systems;
using GameEngine.Source.Components;
using GameEngine.Source.Components.Abstract_Classes;
using GameEngine.Source.Managers;
using GameEngine.Source.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private TextSystem TextSystem;
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
            oldState = Keyboard.GetState();
            TextSystem = new TextSystem(SystemManager.Instance.SpriteBatch);
        }

        /// <summary>
        /// Entered Function for the MainMenu class, responsible
        /// for initializing all the options in the menu aswell as 
        /// creating all the different entities for the menu.
        /// </summary>
        public override void Entered()
        {
            SystemManager.Instance.AddSystem(TextSystem);
            string[] itemNames = {"One Player Game", "Multiplayer Game", "Settings", "Whatever"};
            ItemNames = itemNames;

            int y = 30;
            Color color;
             

            for(int i = 0; i < itemNames.Length; i++)
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
        }
        /// <summary>
        /// Removes all the entities created in this state which is not needed anymore.
        /// </summary>
        public override void Exiting()
        {
            foreach(int id in GameStateEntities)
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
        public override void Update()
        {
            KeyboardState newState;
            newState = Keyboard.GetState();
            TextComponent txc;

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
                    }
                    
                }
            }
            if (newState.IsKeyDown(Keys.Down))
            {
                if (!oldState.IsKeyDown(Keys.Down))
                {
                    if (SelectedItem < ItemNames.Length -1)
                    {
                        int index = GameStateEntities.ElementAt(SelectedItem);
                        txc = ComponentManager.Instance.GetEntityComponent<TextComponent>(index);
                        txc.Color = Color.LightPink;
                        SelectedItem++;
                        index = GameStateEntities.ElementAt(SelectedItem);
                        txc = ComponentManager.Instance.GetEntityComponent<TextComponent>(index);
                        txc.Color = Color.DeepPink;
                    }
                        
                }
            }

            if(newState.IsKeyDown(Keys.Enter))
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
                }
            }
            oldState = newState;

        }

        
    }
}
