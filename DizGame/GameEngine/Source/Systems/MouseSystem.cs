using System;
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
    public class MouseSystem : IUpdate
    {
        public MouseState PrevState { get; set; }
        public MouseState CurState { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            UpdateStates();

            List<int> entities = ComponentManager.GetAllEntitiesWithComponentType<MouseComponent>();

            foreach (var item in entities)
            {
                MouseComponent mouse = ComponentManager.GetEntityComponent<MouseComponent>(item);
                UpdateActionStates(mouse);
                UpdateMousePositions(mouse);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mouseComp"></param>
        private void UpdateMousePositions(MouseComponent mouseComp)
        {
            mouseComp.X = CurState.X;
            mouseComp.Y = CurState.Y;
        }
        
        /// <summary>
        /// updates the previous and current State of the Mouse
        /// </summary>
        private void UpdateStates()
        {
            PrevState = CurState;
            CurState = Mouse.GetState();
        }

        /// <summary>
        /// Updates the states of left-, right- and middle mouse buttons
        /// </summary>
        /// <param name="mouseComponent"></param>
        public void UpdateActionStates(MouseComponent mouseComponent)
        {

            UpdateBtn(mouseComponent, CurState.RightButton, PrevState.RightButton, "RightButton");
            UpdateBtn(mouseComponent, CurState.LeftButton, PrevState.LeftButton, "LeftButton");
            UpdateBtn(mouseComponent, CurState.MiddleButton, PrevState.MiddleButton, "MiddleButton");
        }

        /// <summary>
        /// updates state of a specific mousebutton
        /// </summary>
        /// <param name="mouse"></param>
        /// <param name="curState"></param>
        /// <param name="prevState"></param>
        /// <param name="button"></param>
        private void UpdateBtn(MouseComponent mouse, ButtonState curState, ButtonState prevState, string button)
        {
            
            if (curState == ButtonState.Pressed && prevState == ButtonState.Pressed)
            {
                mouse.MouseActionState[button] = ButtonStates.Hold;
            }
            else if (curState != ButtonState.Pressed && prevState == ButtonState.Pressed)
            {
                mouse.MouseActionState[button] = ButtonStates.Released;
            }
            else if (curState == ButtonState.Pressed && prevState != ButtonState.Pressed)
            {
                mouse.MouseActionState[button] = ButtonStates.Pressed;
            }
            else
            {
                mouse.MouseActionState[button] = ButtonStates.Not_Pressed;
            }
        }
    }
}
