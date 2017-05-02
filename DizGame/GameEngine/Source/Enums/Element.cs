using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Enums
{
    /// <summary>
    /// AirTemperatur is used to get and Tuple<float,float>
    /// Speed of sound and density of air
    /// </summary>
    public enum AirTemperature : int
    {
        Minus25 = -25,
        Minus20 = -20,
        Minus15 = -15,
        Minus10 = -10,
        Minus5 = -5,
        Zero = 0,
        Plus5 = 5,
        Plus10 = 10,
        Plus15 = 15,
        Plus20 = 20,
        Plus25 = 25,
        Plus30 = 30,
        Plus35 = 35
    }
    /// <summary>
    /// Returns an long represending the chosen objects
    /// drag value.
    /// </summary>
    public enum DragType : long
    {
        Default = (long)1,
        Sphere = (long)0.47,
        Half_Sphere = (long)0.42,
        Cone = (long)0.50,
        Cube = (long)1.05,
        Angle45Cube = (long) 0.80,
        LongCylinder = (long) 0.82,
        ShortCylinder = (long)1.15,
        StreamLinedBody = (long) 0.04,
        StreamLinedHalf_Body = (long) 0.09,
        BiCycleWithCyclist = (long) 0.9,
        LowestCar = (long) 0.25,
        Bullet = (long) 0.295,
        ManUpright = (long) 1.15,
        Skier = (long) 1.05,
        WiresAndCables = (long) 1.15,
        SkiJumper = (long) 1.2,
        EmpireStateBuilding = (long) 1.4,
        EiffelTower = (long) 1.9,
        SmoothBrick = (long) 2.1
    }
    /// <summary>
    /// Returns the materials kg/m^3 
    /// </summary>
    public enum DensityType : long
    {
        Air = (long)1.2,
        Beer = (long) 1010,
        Ethanol = (long) 785.1,
        Automobile_Oils = (long) 910,
        Butane = (long) 599,
        Coconut_oil = (long) 924,
        Cresol = (long) 1024,
        Crude_Oil_California = (long) 915,
        Crude_Oil_Mexican = (long) 973,
        Crude_Oil_Texas = (long) 873,
        Diesel_Fuel = (long) 885,
        Vehicle_Gasoline = (long) 737,
        Mercury = (long) 13590,
        Methanol = (long) 791,
        Milk = (long) 1035,
        Nitric_Acid = (long) 1560,
        Olive_Oil = (long) 860,
        Oxygen_Liquid = (long) 1140,
        Paraffin = (long) 800,
        Propane = (long) 493.5,
        Sulfuric_Acid_95 = (long) 1839,
        Water_Heavy = (long) 1105,
        Water_Pure = (long) 1000,
        Water_Sea = (long) 1022,
        Whale_Oil = (long) 925
    }
    public static class AirCoefficients
    {
        /// <summary>
        /// Returns an tuple representing Tuple(SpeedOfSound(c in m*s^-1), DensityOfAir(p in kg*m^-3))
        /// kg/m^3
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Tuple<float, float> GetAirSoundSpeedAndDensity(AirTemperature type)
        {
            switch(type)
            {
                case AirTemperature.Minus25:
                    return new Tuple<float, float>(315.5f, 1.423f);
                case AirTemperature.Minus20:
                    return new Tuple<float, float>(318.9f,1.395f);
                case AirTemperature.Minus15:
                    return new Tuple<float, float>(322.1f, 1.368f);
                case AirTemperature.Minus10:
                    return new Tuple<float, float>(325.2f, 1.342f);
                case AirTemperature.Minus5:
                    return new Tuple<float, float>(328.3f, 1.317f);
                case AirTemperature.Zero:
                    return new Tuple<float, float>(331.3f, 1.292f);
                case AirTemperature.Plus5:
                    return new Tuple<float, float>(334.3f, 1.269f);
                case AirTemperature.Plus10:
                    return new Tuple<float, float>(337.3f, 1.247f);
                case AirTemperature.Plus15:
                    return new Tuple<float, float>(340.3f, 1.225f);
                case AirTemperature.Plus20:
                    return new Tuple<float, float>(343.2f, 1.204f);
                case AirTemperature.Plus25:
                    return new Tuple<float, float>(346.1f, 1.184f);
                case AirTemperature.Plus30:
                    return new Tuple<float, float>(349.0f, 1.164f);
                case AirTemperature.Plus35:
                    return new Tuple<float, float>(351.9f, 1.146f);
                default:
                    return new Tuple<float, float>(331.3f, 1.292f);
            }
        }

    }
}
