using DizGame.Source.AI_Behaviors;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DizGame.Source.Components
{
    // todo ta bort / flytta en del properties till bättre ställen
    /// <summary>
    /// A component for the AI:s
    /// </summary>
    public class AIComponent : IComponent
    {
        private Dictionary<string, AiBehavior> AiBehaviors;

        #region properties
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
        /// A value in seconds for how long the AI will stick to its chosen direction
        /// </summary>
        public float DirectionDuration { get; set; }
        /// <summary>
        /// In what range the new rotation can be. e.g. -PI --- +PI
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
        /// How far from the other AIs/players the AI want to be at a minimum
        /// </summary>
        public float EvadeDistance { get; set; }
        /// <summary>
        /// From how far the AI will start shooting
        /// </summary>
        public float AttackingDistance { get; set; }
        /// <summary>
        /// In what distance the enemy have to be for the AI to chase it
        /// </summary>
        public float ChaseDistance { get; set; }
        /// <summary>
        /// How much damage the AI does per shot
        /// </summary>
        public float DamagePerShot { get; set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="boundRec"></param>
        /// <param name="shootingCoolDown"></param>
        /// <param name="waypoints"> A list of waypoints for the patrolling AI. If the AI not patrolling this can be null </param>
        public AIComponent(Rectangle boundRec, float shootingCoolDown, List<Vector2> waypoints)
        {
            // default values
            this.Bounds = boundRec;
            this.Hysteria = 50;
            this.DirectionDuration = 2f;
            this.DirectionChangeRoation = MathHelper.Pi;
            this.TurningSpeed = 0.8f;
            this.UpdateFrequency = 1f;
            this.ShootingCooldown = shootingCoolDown;
            this.EvadeDistance = 50;
            this.AttackingDistance = 25;
            this.DamagePerShot = 5;

            // the available behaviors of an AI
            AiBehaviors = new Dictionary<string, AiBehavior>()
            {
                { "Wander", new WanderBehavior() },
                { "Chase", new ChaseBehavior() },
                { "Evade", new EvadeBehavior() },
                { "Attacking", new AttackingBehavior() },
                { "Hoarding", new HoardingBehavior() },
            };
            if (waypoints != null)
            {
                AiBehaviors.Add("Patrol", new PatrollingBehavior(waypoints));
                this.CurrentBehaivior = AiBehaviors["Patrol"]; // starting behavior
            }
            else
            {
                this.CurrentBehaivior = AiBehaviors["Wander"]; // starting behavior
            }
        }

        /// <summary>
        /// Checks if an AI has an behavior
        /// </summary>
        /// <param name="behavior"> The name of the behavior </param>
        /// <returns> true if the AI have an instance of the specified behavior </returns>
        public bool HaveBehavior(string behavior)
        {
            return AiBehaviors.ContainsKey(behavior);
        }

        /// <summary>
        /// Changes the behavior to the named behavior
        /// </summary>
        /// <param name="Behavior"> Name of behavior </param>
        /// <param name="currentRoation"> The current rotation of the AI </param>
        public void ChangeBehavior(string Behavior, Vector3 currentRoation)
        {
            var behave = AiBehaviors[Behavior];
            CurrentBehaivior = behave;
            CurrentBehaivior.DesiredRotation = currentRoation.Y;
            CurrentBehaivior.CurrentTimeForRotation = 0;
            CurrentBehaivior.Time = ShootingCooldown;
        }
    }
}