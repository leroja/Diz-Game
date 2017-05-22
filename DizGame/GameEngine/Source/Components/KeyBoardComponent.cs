using GameEngine.Source.Enums;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// A component that contains a number of actions and ther corresponing Key, and the current state of that key.
    /// Used to get input from the keyboard to the game
    /// </summary>
    public class KeyBoardComponent : IComponent
    {
        /// <summary>
        /// A dictionary containing actions and their corresponing key
        /// </summary>
        public Dictionary<string, Keys> KeyBoardActions { get; set; }
        /// <summary>
        /// A dictionary containing action and their current state
        /// </summary>
        public Dictionary<string, ButtonStates> State { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public KeyBoardComponent()
        {
            KeyBoardActions = new Dictionary<string, Keys>();
            State = new Dictionary<string, ButtonStates>();
        }

        /// <summary>
        /// Adds an action and the corresponing key to the dictionary
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
        /// Returns the state of an action
        /// </summary>
        /// <param name="action">
        /// Name of the action to get the state of
        /// </param>
        /// <returns>
        /// The keyboard state of the action
        /// </returns>
        public ButtonStates GetState(string action)
        {
            return State[action];
        }
    }
}
