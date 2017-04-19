using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using Microsoft.Xna.Framework;

namespace GameEngine.Source.Components
{
    public class PhysicsComponent : IComponent
    {
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
        public Vector3 Heading { get; set; }
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
        public Quaternion Orientation { get; set; }
        #endregion Public Configuration

        public PhysicsComponent()
        {
            Mass = 0;

            Acceleration = Vector3.Zero;
            Velocity = Vector3.Zero;
            Heading = Vector3.Zero;

            PhysicsType = PhysicsType.Static;
            MaterialType = MaterialType.None;

            Orientation = Quaternion.Identity;
        }
    }
}
