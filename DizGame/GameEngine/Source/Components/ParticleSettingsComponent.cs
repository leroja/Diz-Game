using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine.Source.RandomStuff;
using System;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// Component for controlling setting of particle effect. this components make it possible to control how the particles from the emitter is printed and updated
    /// </summary>
    public class ParticleSettingsComponent : IComponent
    {
        #region properties
        /// <summary>
        /// texture used on the particle
        /// </summary>
        public Texture2D Texture { get; set; }
        /// <summary>
        /// Max number of particles on emitter used for updating and controlling size of vertex and index buffer
        /// </summary>
        public int MaxParticles { get; set; }
        /// <summary>
        /// Duration of a single particle makes all the particles have a lifetime before disappearing
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Multiplied whit duration so every particle not have the same duration
        /// </summary>
        public float DurationRandomness { get; set; }

        /// <summary>
        /// decides how sensitiv the velocity of the particles
        /// </summary>
        public float EmitterVelocitySensitivity { get; set; }
        /// <summary>
        /// Min value for horizontal velocity
        /// </summary>
        public float MinHorizontalVelocity { get; set; }

        /// <summary>
        /// Max value for Horizontal Velocity
        /// </summary>
        public float MaxHorizontalVelocity { get; set; }
        /// <summary>
        /// Min value for Vertical velocity
        /// </summary>
        public float MinVerticalVelocity { get; set; }

        /// <summary>
        /// max value for vertical velocity
        /// </summary>
        public float MaxVerticalVelocity { get; set; }
        /// <summary>
        /// used to control the moment of particles when updating can be used as gravity or as a wind 
        /// </summary>
        public Vector3 Gravity { get; set; }
        /// <summary>
        /// velocity at en of duration
        /// </summary>
        public float EndVelocity { get; set; }
        /// <summary>
        /// min color value of particle. Default white
        /// </summary>
        public Color MinColor { get; set; }
        /// <summary>
        /// Max value for color. default White
        /// </summary>
        public Color MaxColor { get; set; }

        /// <summary>
        /// Min Rotation of particles
        /// </summary>
        public float MinRotateSpeed { get; set; }
        /// <summary>
        /// Max Rotating speed
        /// </summary>
        public float MaxRotateSpeed { get; set; }
        /// <summary>
        /// min start size of particle
        /// </summary>
        public float MinStartSize { get; set; }
        /// <summary>
        /// max particle Starting size
        /// </summary>
        public float MaxStartSize { get; set; }
        /// <summary>
        /// min particle End size
        /// </summary>
        public float MinEndSize { get; set; }
        /// <summary>
        /// max particle end size
        /// </summary>
        public float MaxEndSize { get; set; }
        /// <summary>
        /// Blendstate
        /// </summary>
        public BlendState BlendState { get; set; }
        #endregion

        /// <summary>
        /// Default constructor sets default values for all setting.
        /// </summary>
        public ParticleSettingsComponent()
        {
            this.MaxParticles = 100;
            this.Duration = TimeSpan.FromSeconds(1);
            this.DurationRandomness = 0;
            this.EmitterVelocitySensitivity = 1;
            this.MinHorizontalVelocity = 0;
            this.MaxHorizontalVelocity = 0;
            this.MinVerticalVelocity = 0;
            this.MaxVerticalVelocity = 0;
            this.Gravity = Vector3.Zero;
            this.MinColor = Color.White;
            this.MaxColor = Color.White;
            this.EndVelocity = 1;
            this.MinRotateSpeed = 0;
            this.MaxRotateSpeed = 0;
            this.MinStartSize = 100;
            this.MaxStartSize = 100;
            this.MinEndSize = 100;
            this.MaxEndSize = 100;

            this.BlendState = BlendState.Additive;
        }
    }
}