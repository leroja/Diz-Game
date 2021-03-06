﻿using GameEngine.Source.Components;

namespace DizGame.Source.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class SpectatingComponent : IComponent
    {
        /// <summary>
        /// The id of the currently spectated entity
        /// </summary>
        public int SpectatedEntity { get; set; }
        /// <summary>
        /// A time value in seconds that acts as a buffer zone
        /// before the ability to spectate kicks in. 
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SpectatingComponent()
        {
            Time = 5;
        }
    }
}