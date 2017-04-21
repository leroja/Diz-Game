using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using Microsoft.Xna.Framework;

namespace GameEngine.Source.Components
{
    public class PhysicsComponent : IComponent
    {
        #region Public Constants
        public const float DEFAULT_GRAVITY = 9.80665f;
        #endregion Public Constants
        #region Public Configuration
        /// <summary>
        /// Defines mass in kilogram ex 1f = 1kg;
        /// </summary>
        public float Mass { get; set; }
        public Vector3 Acceleration { get; set; }
        /// <summary>
        /// Defines velocity in meters per second in a given direction
        /// </summary>
        public Vector3 Velocity { get; set; }
        /// <summary>
        /// Defines maxacceleration in Meters per second each second 
        /// divided by FPS to give meters per second each frame.
        /// </summary>
        public Vector3 MaxAcceleration { get; set; }
        /// <summary>
        /// Maximum force in X,Y,Z in newtones (kilogram meter per second each second).
        /// </summary>
        public Vector3 Forces { get; set; }
        public PhysicsType PhysicsType { get; set; }
        public MaterialType MaterialType { get; set; }
        #endregion Public Configuration

        public PhysicsComponent()
        {
            Mass = 1;

            Acceleration = Vector3.Zero;
            Velocity = Vector3.Zero;

            Forces += DEFAULT_GRAVITY * Mass * Vector3.Down; // Sets the basi forces to an downforce by regular "gravity constant"

            PhysicsType = PhysicsType.Static;
            MaterialType = MaterialType.None;
        }
    }
}
