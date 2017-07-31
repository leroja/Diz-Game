using GameEngine.Source.Components.Abstract_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.NetworkStuff;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework.Input;

namespace DizGame.Source.GameStates
{
    /// <summary>
    /// 
    /// </summary>
    public class LobbyGameState : GameState
    {
        private NetworkSystem networkSystem;
        private KeyboardState oldState;
        private KeyboardState newState;
        private int portnumber;
        /// <summary> </summary>
        public override List<int> GameStateEntities { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portNumber"></param>
        public LobbyGameState(int portNumber)
        {
            this.portnumber = portNumber;
            GameStateEntities = new List<int>();
            SystemManager.Instance.ClearSystems();
            networkSystem = new NetworkSystem(this.portnumber);
            SystemManager.Instance.AddSystem(networkSystem);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Entered()
        {
            ComponentManager.Instance.ClearManager();

            AudioManager.Instance.PlaySong("LobbySong");
            AudioManager.Instance.ChangeSongVolume(0.15f);
            AudioManager.Instance.ChangeGlobalSoundEffectVolume(1f);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Exiting()
        {
            SystemManager.Instance.ClearSystems();
            AudioManager.Instance.ChangeSongVolume(1f);
            AudioManager.Instance.StopSong();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Obscuring()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Revealed()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            oldState = newState;
            newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Escape) && !oldState.IsKeyDown(Keys.Escape))
            {
                var NewMainMenu = new MainMenu();
                GameStateManager.Instance.Pop();
                GameStateManager.Instance.Push(NewMainMenu);
            }

            if (StartGame())
            {
                var mpGame = new MPGameState(this.networkSystem);
                GameStateManager.Instance.Pop();
                GameStateManager.Instance.Push(mpGame);
            }

        }

        // TODO: 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool StartGame()
        {
            return false;
        }
    }
}