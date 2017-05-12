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
        /// 
        /// </summary>
        public float Hysteria { get; set; }

        /// <summary>
        /// A value in seconds for how long the AI will stick to its choosen direction
        /// </summary>
        public float DirectionDuration { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public float DirectionChangeRoation { get; set; }
        
        public float TurningSpeed { get; set; }

        /// <summary>
        /// How often the AI will update its rotation based on the closest enemy
        /// </summary>
        public float UpdateFrequency { get; set; }

        public float TargetRotation { get; set; }

        /// <summary>
        /// The delay between the shoots for when the AI is shooting
        /// Ideally somewhere between 0 and 1 second
        /// </summary>
        public float ShootingCooldown { get; set; }

        public float EvadeDistance { get; set; }

        public float AttackingDistance { get; set; }

        public float ChaseDistance { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AIComponent(float hysteria, Rectangle rec, float dur, float rot, float turSpeed, float updateFreq, float coolDown, float evadeDist, float atkDist) // todo tabort här och lägg till saker i stället när man skapar componenten
        {            
            this.Hysteria = hysteria;
            this.Bounds = rec;
            this.DirectionDuration = dur;
            this.DirectionChangeRoation = rot;
            this.TurningSpeed = turSpeed;
            this.UpdateFrequency = updateFreq;
            this.TargetRotation = 0;
            this.ShootingCooldown = coolDown;
            this.EvadeDistance = evadeDist;
            this.AttackingDistance = atkDist;

            AiBehaviors = new Dictionary<string, AiBehavior>()
            {
                {"Wander", new WanderBehavior() },
                {"Chase", new ChaseBehavior() },
                {"Evade", new EvadeBehavior() },
                {"Attacking", new AttackingBehavior(coolDown) }
            };
            this.CurrentBehaivior = AiBehaviors["Chase"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Behavior"></param>
        /// <returns></returns>
        private AiBehavior GetBehavior(string Behavior)
        {
            return AiBehaviors[Behavior];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Behavior"></param>
        /// <param name="currentRoation"></param>
        public void ChangeBehavior(string Behavior, Vector3 currentRoation)
        {
            var behave = GetBehavior(Behavior);
            CurrentBehaivior = behave;
            CurrentBehaivior.OnEnter(currentRoation);
        }
    }
}
