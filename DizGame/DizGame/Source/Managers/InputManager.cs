using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Managers
{
    public class InputManager
    {
        private static InputManager instance;

        private Rectangle bounds;
        private Vector2 center;

        private InputManager()
        {
            this.bounds = GameOne.bounds;
            
            center = new Vector2(bounds.Width / 2, bounds.Height / 2);
        }

        /// <summary>
        /// The instance of the Manager
        /// </summary>
        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }
                return instance;
            }
        }

        public void SetPos()
        {
            Mouse.SetPosition((int)center.X, (int)center.Y);
        }

    }
}
