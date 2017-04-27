using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using Microsoft.Xna.Framework;

namespace GameEngine.Source.Components
{
    public class PhysicsComponent : IComponent
    {
        #region Public Constants
        public const float DEFAULT_GRAVITY = 9.80665f;
        public const float DEFAULT_BICEPSFORCE = 500; // in Newton(in kilogram 50kg)
        public const float DEFAULT_LEGFORCE = 3000;  //  in NewTon(in kilogram 3000kg)
        public const float DEFAULT_WALKFORCE = 1000; 
        #endregion Public Constants
        #region Public Configuration
        /// <summary>
        /// Defines mass in kilogram ex 1f = 1kg;
        /// </summary>
        public float Mass { get; set; }
        public Vector3 Acceleration { get; set; }
        /// <summary>
        /// Defines maxacceleration in Meters per second each second 
        /// divided by FPS to give meters per second each frame.
        /// </summary>
        public Vector3 MaxAcceleration { get; set; }
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
        /// Defines the Initial velocity the object started with
        /// eg. used for projectiles (gun) etc.
        /// </summary>
        public Vector3 InitialVelocity { get; set; }
        /// <summary>
        /// Defines the physic objects max velocity
        /// </summary>
        public Vector3 MaxVelocity { get; set; }

        /// <summary>
        /// Maximum force in X,Y,Z in newtones (kilogram meter per second each second).
        /// </summary>
        public Vector3 Forces { get; set; }
        public PhysicsType PhysicsType { get; set; }
        public MaterialType MaterialType { get; set; }
        public DragType DragType { get; set; }
        public GravityType GravityType { get; set; }
        public float ReferenceArea { get; set; }
        public float Gravity { get; set; }
        public float Density { get; set; }
        public float Volume { get; set; }
        public float Bounciness { get; set; }
        public bool IsMoving { get; set; } //TODO: Skall inte ligga här
        public bool IsFalling { get; set; } //TODO: Skall inte ligga här
        #endregion Public Configuration

        public PhysicsComponent()
        {
            Density = 1f;
            Volume = 1f;
            Mass = Density * Volume;
            Bounciness = 1;

            Acceleration = Vector3.Zero;
            MaxVelocity = Vector3.Zero;
            LastAcceleration = Vector3.Zero;
            Velocity = Vector3.Zero;
            InitialVelocity = Vector3.Zero;

            Forces += Vector3.Zero; //DEFAULT_GRAVITY * Mass * Vector3.Down; // Sets the basi forces to an downforce by regular "gravity constant"
            PhysicsType = PhysicsType.Static;
            MaterialType = MaterialType.None;
            DragType = DragType.Default;
            GravityType = GravityType.None;
            ReferenceArea = MathHelper.Pi * 10;

            Gravity = DEFAULT_GRAVITY;
            IsMoving = false;
            IsFalling = false;
        }
    }
}
