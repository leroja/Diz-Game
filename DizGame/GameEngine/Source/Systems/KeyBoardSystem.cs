using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// A system the updates the states of keybord components
    /// </summary>
    public class KeyBoardSystem : IUpdate
    {
        /// <summary>
        /// The previous state of the keyboard
        /// </summary>
        public KeyboardState PrevState { get; set; }
        /// <summary>
        /// The current state of the Keyboard
        /// </summary>
        public KeyboardState CurState { get; set; }

        /// <summary>
        /// Updates the action states of all the keyboard components
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            UpdateStates();

            List<int> entities = ComponentManager.GetAllEntitiesWithComponentType<KeyBoardComponent>();

            Parallel.ForEach(entities, item =>
            {
                KeyBoardComponent kbc = ComponentManager.GetEntityComponent<KeyBoardComponent>(item);
                UpdateActionStates(kbc);
            });
        }

        /// <summary>
        /// updates the previous and current State of the keyboard
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
        private void UpdateActionStates(KeyBoardComponent keyboardComp)
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