using DizGame.Source.AI_States;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class AIComponent : IComponent
    {
        /// <summary>
        /// The current Behavior/state of the AI Entity
        /// </summary>
        public IAiBehavior CurrentBehaivior { get; set; }
        /// <summary>
        /// In what bounds the AI can move, based from origo
        /// </summary>
        public Rectangle Bounds { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float Hysteria { get; set; }
        /// <summary>
        /// A value in seconds for how long the AI will stick to its choosen direction
        /// </summary>
        public float DirectionDuration { get; set; }
        // todo
        /// <summary>
        /// 
        /// </summary>
        public float DirectionChangeRoation { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AIComponent(float hysteria, Rectangle rec, float dur, float rot)
        {
            this.CurrentBehaivior = new WanderBehavior(Quaternion.Identity);
            this.Hysteria = hysteria;
            this.Bounds = rec;
            this.DirectionDuration = dur;
            this.DirectionChangeRoation = rot;
        }
    }
}
