using GameEngine.Source.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Components
{
    public class PlayerComponent : IComponent
    {
        public int PlayerID { get; set; }

        public PlayerComponent()
        {
        }

        public PlayerComponent(int PlayerId)
        {
            this.PlayerID = PlayerID;
        }
    }
}
