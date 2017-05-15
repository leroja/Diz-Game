using GameEngine.Source.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    public class MouseComponent : IComponent
    {
        public Dictionary<string, ButtonStates> MouseActionState { get; set; }
        public Dictionary<string, string> MouseActionBinding { get; set; }
        public float Y { get; set; }
        public float X { get; set; }

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
        /// 
        /// 
        /// available keys:
        ///     RightButton, LeftButton, MiddleButton
        /// </summary>
        /// <param name="action"></param>
        /// <param name="button"></param>
        public void AddActionToButton(string action, string button)
        {
            MouseActionBinding.Add(action, button);
        }

        public ButtonStates GetState(string action)
        {
            return MouseActionState[MouseActionBinding[action]];
        }
    }
}
