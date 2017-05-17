using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Enums;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// Component which contains data about the world
    /// </summary>
    public class WorldComponent : IComponent
    {
        #region Public Enum
        /// <summary>
        /// int enum to define the clocks notation 12 or 24h clock.
        /// </summary>
        public enum ClockNotation : int
        {
            /// <summary>
            /// 12hour clock.
            /// </summary>
            HOURS12 = 12,
            /// <summary>
            /// 24hour clock.
            /// </summary>
            HOURS24 = 24,
        }
        #endregion Public Enum

        #region Public Properties
        /// <summary>
        /// A value that could be used for different relevant things that 
        /// needs to be commonly implemented throughout different systems.
        /// </summary>
        public int ModulusValue { get; set; }
        /// <summary>
        /// This is used if Notation is set as 12hour clock,
        /// to change the day when "24 hours has past eg. 2x12"
        /// </summary>
        public bool Noon { get; set; }
        /// <summary>
        /// Is to be set 
        /// </summary>
        public ClockNotation Notation { get; set; }
        /// <summary>
        /// Defines which day it's.
        /// </summary>
        public float Day { get; set; }
        /// <summary>
        /// Defines the current hour.
        /// </summary>
        public float Hour { get; set; }
        /// <summary>
        /// Defines the current minute.
        /// </summary>
        public float Minute { get; set; }
        /// <summary>
        /// Defines the current Second.
        /// </summary>
        public float Second { get; set; }
        /// <summary>
        /// Defines the current millisecond.
        /// </summary>
        public float Millisecond { get; set; }
        /// <summary>
        /// Defines the global world matrix.
        /// </summary>
        public Matrix World { get; set; }
        /// <summary>
        /// Defines the hour by ingame seconds
        /// </summary>
        public int DefineHour { get; set; }
        /// <summary>
        /// Enum used to defines the world current temperatur
        /// </summary>
        public AirTemperature Temperatur { get; set; }
        /// <summary>
        /// Defines the worlds gravity (pull).
        /// </summary>
        public Vector3 Gravity { get; set; }
        /// <summary>
        /// Defines and list of worldfluids as an Dictionary, this might be overflow ->
        /// big chance of removing.
        /// </summary>
        public Dictionary<DensityType, List<Tuple<Vector3, Vector3>>> WorldFluids { get; set; }
        /// <summary>
        /// bool if sun is "on"
        /// </summary>
        public bool IsSunActive { get; set; }
        #endregion Public Properties
        /// <summary>
        /// Basic constructor which sets all the attributes to default value.
        /// </summary>
        /// <param name="world"></param>
        public WorldComponent(Matrix world)
        {
            ModulusValue = 3;
            World = world;
            Noon = true;
            Notation = ClockNotation.HOURS24;
            Hour = 12;
            Minute = 0;
            Second = 0;
            Millisecond = 0;
            Day = 0;
            DefineHour = 1;
            Temperatur = AirTemperature.Plus20;
            Gravity = PhysicsComponent.DEFAULT_GRAVITY * Vector3.Down;
            WorldFluids = new Dictionary<DensityType, List<Tuple<Vector3,Vector3>>>
            {
                { DensityType.Air, new List<Tuple<Vector3,Vector3>>() },
                { DensityType.Water_Heavy, new List<Tuple<Vector3,Vector3>>() },
                { DensityType.Automobile_Oils, new List<Tuple<Vector3,Vector3>>() }
            };
            IsSunActive = true;
        }

    }
}
