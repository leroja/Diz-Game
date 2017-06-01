using GameEngine.Source.Enums;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// A component for using the mouse as input
    /// </summary>
    public class MouseComponent : IComponent
    {
        /// <summary>
        /// Stores the states of the actions bound to the specific keys
        /// </summary>
        public Dictionary<string, ButtonStates> MouseActionState { get; set; }
        /// <summary>
        /// Stores wich actions are boudn to wich specific key
        /// </summary>
        public Dictionary<string, string> MouseActionBinding { get; set; }
        /// <summary>
        /// The sensitivity
        /// </summary>
        public float MouseSensitivity { get; set; }
        /// <summary>
        /// The current postion of the mouse
        /// </summary>
        public Vector2 CurrentPosition { get; set; }
        /// <summary>
        /// The previous position of the mouse
        /// </summary>
        public Vector2 PreviousPostion { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MouseComponent()
        {
            MouseActionState = new Dictionary<string, ButtonStates>
            {
                { "LeftButton", ButtonStates.Not_Pressed },
                { "MiddleButton", ButtonStates.Not_Pressed },
                { "RightButton", ButtonStates.Not_Pressed }
            };
            MouseActionBinding = new Dictionary<string, string>();
        }

        /// <summary>
        /// Binds an action to a MouseButton
        /// 
        /// available keys/buttons:
        ///     RightButton, LeftButton, MiddleButton
        /// </summary>
        /// <param name="action"> name of the action </param>
        /// <param name="button"> wich mousebutton the action should be binded to </param>
        public void AddActionToButton(string action, string button)
        {
            MouseActionBinding.Add(action, button);
        }

        /// <summary>
        /// Returns the state of an action
        /// </summary>
        /// <param name="action">
        /// Name of the action to get the state of
        /// </param>
        /// <returns>
        /// The state of the action
        /// </returns>
        public ButtonStates GetState(string action)
        {
            return MouseActionState[MouseActionBinding[action]];
        }
    }
}