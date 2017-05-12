using GameEngine.Source.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    // todo write comments
    /// <summary>
    /// 
    /// </summary>
    public class MouseComponent : IComponent
    {
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, ButtonStates> MouseActionState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> MouseActionBinding { get; set; }
        /// <summary>
        /// The sensitivity
        /// </summary>
        public float MouseSensitivity { get; set; }
        /// <summary>
        /// The X position of the mouse on the screen
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// The Y position of the mouse on the screen
        /// </summary>
        public int Y { get; set; }

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
