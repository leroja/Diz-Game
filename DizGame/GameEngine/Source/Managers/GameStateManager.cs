using GameEngine.Source.Components.Abstract_Classes;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameEngine.Source.Managers
{
    /// <summary>
    /// Instance which handles the Games different states. The different game states
    /// should be added to this manager and manipulated through the appropriate
    /// functions which are provided by this manager.
    /// </summary>
    public class GameStateManager
    {
        #region Properties
        private static GameStateManager instance;
        private Stack<GameState> stack;
        #endregion

        /// <summary>
        /// Public instance 'constructor' for the gamestate manager to successfully
        /// use the singleton pattern
        /// </summary>
        public static GameStateManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameStateManager();
                }
                return instance;
            }
        }
        private GameStateManager()
        {
            stack = new Stack<GameState>();
        }

        /// <summary>
        /// Pop function to enable the removal of a gamestate from the stack, 
        /// means that the gamestate is no longer the current gamestate
        /// </summary>
        /// <returns></returns>
        public GameState Pop()
        {
            GameState previousState, removedState;
            removedState = stack.Pop();
            removedState.Exiting();

            //means that there is another state on the stack waiting to be revealed again
            if (stack.Count > 0)
            {
                previousState = stack.Peek();
                previousState.Revealed();
            }

            return removedState;
        }

        /// <summary>
        /// Function to push a gamestate to the top of the stack and make it the current gamestate
        /// </summary>
        /// <param name="state">takes an object derived from the gamestate class which should represent the desirable game state</param>
        public void Push(GameState state)
        {
            GameState previousState;
            //Means that there is another state on the stack, the pushed state is therefore 
            //obscuring the state already on the stack.
            if (stack.Count > 0)
            {
                previousState = stack.Peek();
                previousState.Obscuring();
            }
            stack.Push(state);
            state.Entered();
        }

        /// <summary>
        /// Function which enables a peek of the current gamestate
        /// </summary>
        /// <returns>returns the current gamestate as a GameState object</returns>
        public GameState Peek()
        {
            return stack.Peek();
        }

        /// <summary>
        /// Method for running the current GameStates update logic, in order to 
        /// run logic that might be necessary for the gamestate to check condition
        /// in which change of a gamestate might be necessary
        /// </summary>
        public void UpdateGameState(GameTime gameTime)
        {
            if (stack.Count != 0)
            {
                GameState currGameState = Peek();
                currGameState.Update(gameTime);
            }
        }
    }
}
