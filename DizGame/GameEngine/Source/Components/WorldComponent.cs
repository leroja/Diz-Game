using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Enums;

namespace GameEngine.Source.Components
{
    public class WorldComponent : IComponent
    {
        #region Public Constants
        public const int HOURS24 = 24;
        public const int HOURS12 = 12;
        #endregion Public Constants

        #region Public Properties
        public bool Noon { get; set; }
        public int Notation { get; set; }
        public float Day { get; set; }
        public float Hour { get; set; }
        public float Minute { get; set; }
        public float Second { get; set; }
        public float Millisecond { get; set; }
        public Matrix World { get; set; }
        /// <summary>
        /// Defines the hour by ingame seconds
        /// </summary>
        public int DefineHour { get; set; }
        public AirTemperatur Temperatur { get; set; }
        public Vector3 Gravity { get; set; }

        #endregion Public Properties

        public WorldComponent(Matrix world)
        {
            this.World = world;
            Noon = true;
            Notation = HOURS12;
            Hour = 12;
            Minute = 0;
            Second = 0;
            Millisecond = 0;
            DefineHour = 20;
            Temperatur = AirTemperatur.Plus20;
            Gravity = PhysicsComponent.DEFAULT_GRAVITY * Vector3.Down;
        }

    }
}
