using GameEngine.Source.Enums;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// A component that contains a number of actions and their corresponding Key, and the current state of that key.
    /// Used to get input from the keyboard to the game
    /// </summary>
    public class KeyBoardComponent : IComponent
    {
        /// <summary>
        /// A dictionary containing actions and their corresponding key
        /// </summary>
        internal Dictionary<string, Keys> KeyBoardActions { get; set; }
        /// <summary>
        /// A dictionary containing action and their current state
        /// </summary>
        internal Dictionary<string, ButtonStates> State { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public KeyBoardComponent()
        {
            KeyBoardActions = new Dictionary<string, Keys>();
            State = new Dictionary<string, ButtonStates>();
        }

        /// <summary>
        /// Adds an action and the corresponding key to the dictionary
        /// </summary>
        /// <param name="action">
        /// The name of the action
        /// </param>
        /// <param name="Key">
        /// The Key that the action should belong to
        /// </param>
        public void AddActionAndKey(string action, Keys Key)
        {
            KeyBoardActions.Add(action, Key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="state"></param>
        public void SetState(string action, ButtonStates state)
        {
            State[action] = state;
        }

        /// <summary>
        /// Returns the state of an action
        /// </summary>
        /// <param name="action">
        /// Name of the action to get the state of
        /// </param>
        /// <returns>
        /// The keyboard state of the action
        /// </returns>
        internal ButtonStates GetState(string action)
        {
            return State[action];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool IsPressed(string action)
        {
            return GetState(action) == ButtonStates.Pressed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool IsHold(string action)
        {
            return GetState(action) == ButtonStates.Hold;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool IsReleased(string action)
        {
            return GetState(action) == ButtonStates.Released;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool IsNotPressed(string action)
        {
            return GetState(action) == ButtonStates.Not_Pressed;
        }
    }
}