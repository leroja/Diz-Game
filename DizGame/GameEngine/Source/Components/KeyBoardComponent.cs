using GameEngine.Source.Components.Interface;
using GameEngine.Source.Enums;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    public class KeyBoardComponent : IComponent
    {
        public Dictionary<String, Keys> KeyBoardActions { get; set; }
        public Dictionary<String, ButtonStates> State { get; set; }

        public KeyBoardComponent()
        {
            KeyBoardActions = new Dictionary<String, Keys>();
            State = new Dictionary<String, ButtonStates>();
        }
    }
}
