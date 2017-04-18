﻿using System;
using GameEngine.Source.Systems.Abstract_classes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;

namespace GameEngine.Source.Systems
{
    public class KeyBoardSystem : IUpdate
    {
        public KeyboardState PrevState { get; set; }
        public KeyboardState CurState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            UpdateStates();

            List<int> entities = ComponentManager.GetAllEntitiesWithComponentType<KeyBoardComponent>();

            if (entities != null)
            {
                foreach (var item in entities)
                {
                    KeyBoardComponent kbc = ComponentManager.GetEntityComponent<KeyBoardComponent>(item);
                    UpdateActionStates(kbc);
                }
            }
        }

        /// <summary>
        /// updates the previous & current State of the keyboard
        /// </summary>
        private void UpdateStates()
        {
            PrevState = CurState;
            CurState = Keyboard.GetState();
        }

        /// <summary>
        /// updates the states of all the Keyboard actions in a keyboard Component
        /// </summary>
        /// <param name="keyboardComp"></param>
        public void UpdateActionStates(KeyBoardComponent keyboardComp)
        {

            foreach (string action in keyboardComp.KeyBoardActions.Keys)
            {
                Keys key = keyboardComp.KeyBoardActions[action];
                bool newState = CurState.IsKeyDown(key);
                bool oldState = PrevState.IsKeyDown(key);

                if (newState && !oldState)
                {
                    keyboardComp.State[action] = ButtonStates.Pressed;
                }
                else if (newState && oldState)
                {
                    keyboardComp.State[action] = ButtonStates.Hold;
                }
                else if (!newState && oldState)
                {
                    keyboardComp.State[action] = ButtonStates.Released;
                }
                else
                {
                    keyboardComp.State[action] = ButtonStates.Not_Pressed;
                }
            }
        }
    }
}