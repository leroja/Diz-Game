using GameEngine.Source.Components.Abstract_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Pop function to enable the removal of a gamestate from the stack
        /// </summary>
        /// <returns></returns>
        public GameState Pop()
        {
            GameState previousState, removedState;
            removedState = stack.Pop();

            //means that there is another state on the stack waiting to be revealed again
            if (stack.Count > 0)
            {
                previousState = stack.Peek();
                previousState.Revealed();
            }

            return removedState;

        }

        public void Push(GameState state)
        {
            GameState previousState;
            //Means that there is another state on the stack, the pushed state is therefore 
            //obscuring the state allready on the stack.
            if(stack.Count > 0)
            {
                previousState = stack.Peek();
                previousState.Obscuring();
            }
            stack.Push(state);
            state.Entered();
        }

        public GameState Peek()
        {
            return stack.Peek();
        }
    }
}
