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
        private Dictionary<string, AiBehavior> AiBehaviors;


        /// <summary>
        /// The current Behavior/state of the AI Entity
        /// </summary>
        public AiBehavior CurrentBehaivior { get; set; }
        /// <summary>
        /// In what bounds the AI can move, based from origo
        /// </summary>
        public Rectangle Bounds { get; set; }
        /// <summary>
        /// todo
        /// </summary>
        public float Hysteria { get; set; }
        /// <summary>
        /// A value in seconds for how long the AI will stick to its choosen direction
        /// </summary>
        public float DirectionDuration { get; set; }     
        /// <summary>
        /// todo
        /// </summary>
        public float DirectionChangeRoation { get; set; }
        /// <summary>
        /// How fast the AI will turn each update
        /// </summary>
        public float TurningSpeed { get; set; }
        /// <summary>
        /// How often the AI will update its rotation based on the closest enemy in seconds
        /// </summary>
        public float UpdateFrequency { get; set; }      
        /// <summary>
        /// The delay between the shoots for when the AI is shooting
        /// Ideally somewhere between 0 and 1 second
        /// </summary>
        public float ShootingCooldown { get; set; }
        /// <summary>
        /// todo
        /// </summary>
        public float EvadeDistance { get; set; }
        /// <summary>
        /// from how far the AI will start shooting
        /// </summary>
        public float AttackingDistance { get; set; }
        

        /// <summary>
        /// Constructor
        /// </summary>
        public AIComponent(Rectangle boudnRec, float shootingCoolDown)
        {            
            this.Hysteria = 50;
            this.DirectionDuration = 2f;
            this.DirectionChangeRoation = MathHelper.Pi;
            this.TurningSpeed = 0.8f;
            this.UpdateFrequency = 1f;
            this.ShootingCooldown = shootingCoolDown;
            this.EvadeDistance = 50;
            this.AttackingDistance = 25;

            AiBehaviors = new Dictionary<string, AiBehavior>()
            {
                {"Wander", new WanderBehavior() },
                {"Chase", new ChaseBehavior() },
                {"Evade", new EvadeBehavior() },
                {"Attacking", new AttackingBehavior(ShootingCooldown) }
            };
            this.CurrentBehaivior = AiBehaviors["Attacking"];
        }


        private AiBehavior GetBehavior(string Behavior)
        {
            return AiBehaviors[Behavior];
        }

        /// <summary>
        /// Changes the behavior to the named behavior
        /// </summary>
        /// <param name="Behavior"> Name of behavior </param>
        /// <param name="currentRoation"> The current rotation of the AI </param>
        public void ChangeBehavior(string Behavior, Vector3 currentRoation)
        {
            var behave = GetBehavior(Behavior);
            CurrentBehaivior = behave;
            CurrentBehaivior.OnEnter(currentRoation);
        }
    }
}
