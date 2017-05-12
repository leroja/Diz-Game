﻿using GameEngine.Source.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Components
{
    /// <summary>
    /// A component for defining a player
    /// </summary>
    public class PlayerComponent : IComponent
    {
        /// <summary>
        /// The ID of the player
        /// </summary>
        public int PlayerID { get; set; }

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="PlayerId"></param>
        public PlayerComponent(int PlayerId)
        {
            this.PlayerID = PlayerID;
        }

        /// <summary>
        /// An enmpty constructor
        /// </summary>
        public PlayerComponent()
        {

        }
    }
}
