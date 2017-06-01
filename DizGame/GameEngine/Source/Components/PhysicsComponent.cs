using GameEngine.Source.Enums;
using Microsoft.Xna.Framework;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// PhysicComponent that contains vital attributes 
    /// used for calculation physics eg. Velocity, acceleration etc.
    /// </summary>
    public class PhysicsComponent : IComponent
    {
        #region Public Constants
        /// <summary>
        /// Default gravity (9.80665f)
        /// </summary>
        public const float DEFAULT_GRAVITY = 9.80665f;
        #endregion Public Constants
        #region Public Configuration
        /// <summary>
        /// Acceleration in meter per second squared (m/s^2)
        /// </summary>
        public Vector3 Acceleration
        {
            get;
            set;
        }
        /// <summary>
        /// Used to calculate an higher accuracy of velocity,
        /// position using Euler method.
        /// </summary>
        public Vector3 LastAcceleration { get; set; }
        /// <summary>
        /// Defines velocity in meters per second in a given direction
        /// </summary>
        public Vector3 Velocity { get; set; }
        /// <summary>
        /// Defines the physic objects max velocity.
        /// </summary>
        public Vector3 MaxVelocity { get; set; }
        /// <summary>
        /// Maximum force in X,Y,Z in newtones (kilogram meter per second each second).
        /// 1N = 1kg-m/s^2
        /// </summary>
        public Vector3 Forces { get; set; }
        /// <summary>
        /// Sets the physicComponents PhysicType eg. Static, Rigid Projectile etc.
        /// </summary>
        public PhysicsType PhysicsType { get; set; }
        /// <summary>
        /// Sets the objects material type eg. Skin, metal etc.
        /// </summary>
        public MaterialType MaterialType { get; set; }
        /// <summary>
        /// Sets the objects drag type eg. Cylinder, Man up right etc.
        /// </summary>
        public DragType DragType { get; set; }
        /// <summary>
        /// Sets the objects gravity type eg. World, Self or none.
        /// </summary>
        public GravityType GravityType { get; set; }
        /// <summary>
        /// Defines mass in kilogram ex 1f = 1kg;
        /// </summary>
        public float Mass { get; set; }
        /// <summary>
        /// Invermass 1/M
        /// </summary>
        public float InverseMass { get; set; }
        /// <summary>
        /// ReferenceArea should be set as m^2
        /// </summary>
        public float ReferenceArea { get; set; }
        /// <summary>
        /// This is only used if GravityType is (Self).
        /// </summary>
        public float Gravity { get; set; }
        /// <summary>
        /// Should be used as kg/m^3 (1f = 1kg/m^3)
        /// </summary>
        public float Density { get; set; }
        /// <summary>
        /// Should be used as m^3 (1f = 1m^3)
        /// </summary>
        public float Volume { get; set; }
        /// <summary>
        /// Float that sets the bounciness of the object.
        /// </summary>
        public float Bounciness { get; set; }
        /// <summary>
        /// This is used as an negativ force on the acceleration
        /// </summary>
        public float Friction { get; set; } // TODO: Temp
        /// <summary>
        /// Bool to check if the object is in air.
        /// </summary>
        public bool IsInAir { get; set; }
        /// <summary>
        /// Bool to check if object is moving.
        /// </summary>
        public bool IsMoving { get; set; }
        #endregion Public Configuration

        /// <summary>
        /// Basic constructor which sets all the attributes to default values.
        /// </summary>
        public PhysicsComponent()
        {
            Density = 1f;
            Volume = 1f;
            Mass = Density * Volume;
            Bounciness = 1f;
            Friction = 0.5f;
            if (Mass == 0)
                InverseMass = 0;
            else
                InverseMass = 1 / Mass;

            IsInAir = false;
            IsMoving = false;

            Acceleration = Vector3.Zero;
            MaxVelocity = new Vector3(14, 14, 14);
            LastAcceleration = Vector3.Zero;
            Velocity = Vector3.One;

            Forces += Vector3.Zero; //DEFAULT_GRAVITY * Mass * Vector3.Down; // Sets the basi forces to an downforce by regular "gravity constant"

            PhysicsType = PhysicsType.Static;
            MaterialType = MaterialType.None;
            DragType = DragType.Default;
            GravityType = GravityType.None;
            ReferenceArea = 0.05f;

            Gravity = DEFAULT_GRAVITY;
        }
    }
}
