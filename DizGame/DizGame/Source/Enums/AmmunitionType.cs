using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Enums
{
    /// <summary>
    /// Enum for different types of ammunition
    /// </summary>
    public enum AmmunitionType : int
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// Twenty Two Long
        /// </summary>
        Twenty_Two_LR,
        /// <summary>
        /// Pistol .380 (short 9mm)
        /// </summary>
        Pistol_380,
        /// <summary>
        /// Pistol 9mm
        /// </summary>
        Pistol_9mm,
        /// <summary>
        /// Pistol .45
        /// </summary>
        Pistol_45,
        /// <summary>
        /// AK 47 7.62x39mm
        /// </summary>
        AK_47,
        /// <summary>
        /// Shotgun - 12 gauge
        /// </summary>
        Gauge_12,
        /// <summary>
        /// RPG - rocket
        /// </summary>
        RPG,
    }
    /// <summary>
    /// Static class for magazine
    /// </summary>
    public static class Magazine
    {
        /// <summary>
        /// Static function that gets the size of the magazine using
        /// AmmunitionType as parameter.
        /// </summary>
        /// <param name="ammo"></param>
        /// <returns></returns>
        public static int GetSize(AmmunitionType ammo)
        {
            switch(ammo)
            {
                case AmmunitionType.AK_47:
                    return 30;
                case AmmunitionType.Gauge_12:
                    return 8;
                case AmmunitionType.Pistol_380:
                    return 15;
                case AmmunitionType.Pistol_45:
                    return 10;
                case AmmunitionType.Pistol_9mm:
                    return 13;
                case AmmunitionType.RPG:
                    return 1;
                case AmmunitionType.Twenty_Two_LR:
                    return 25;
                default:
                    return 0;
            }
        }
    }
}
