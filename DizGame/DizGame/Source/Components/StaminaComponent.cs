using GameEngine.Source.Components;
using System;

namespace DizGame.Source.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class StaminaComponent : IComponent
    {
        /// <summary>
        /// The current amount of stamina
        /// </summary>
        public float CurrentStamina { get; set; }
        /// <summary>
        /// The maximum amount of Stamina
        /// </summary>
        public float MaximumStamina { get; set; }
        /// <summary>
        /// The current stamina ratio; CurrentStamina / MaximumStamina
        /// </summary>
        public float StaminaRatio { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float RunThreshold { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsRunning { get; set; }
    }
}