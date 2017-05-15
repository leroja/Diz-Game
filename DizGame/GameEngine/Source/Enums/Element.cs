using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Enums
{
    /// <summary>
    /// AirTemperatur is used to get and Tuple(float, float)
    /// Speed of sound and density of air
    /// </summary>
    public enum AirTemperature : int
    {
        /// <summary>
        /// Returns -25
        /// </summary>
        Minus25 = -25,
        /// <summary>
        /// Returns -20
        /// </summary>
        Minus20 = -20,
        /// <summary>
        /// Returns -15
        /// </summary>
        Minus15 = -15,
        /// <summary>
        /// Returns -10
        /// </summary>
        Minus10 = -10,
        /// <summary>
        /// Returns -5
        /// </summary>
        Minus5 = -5,
        /// <summary>
        /// Returns 0
        /// </summary>
        Zero = 0,
        /// <summary>
        /// Returns  5
        /// </summary>
        Plus5 = 5,
        /// <summary>
        /// Returns 10
        /// </summary>
        Plus10 = 10,
        /// <summary>
        /// Returns  15
        /// </summary>
        Plus15 = 15,
        /// <summary>
        /// Returns 20
        /// </summary>
        Plus20 = 20,
        /// <summary>
        /// Returns 25
        /// </summary>
        Plus25 = 25,
        /// <summary>
        /// Returns 30
        /// </summary>
        Plus30 = 30,
        /// <summary>
        /// Returns 35
        /// </summary>
        Plus35 = 35
    }
    /// <summary>
    /// Returns an long represending the chosen objects
    /// drag value.
    /// </summary>
    public enum DragType : long
    {
        /// <summary>
        /// Returns  1
        /// </summary>
        Default = (long)1,
        /// <summary>
        /// Returns 0.47
        /// </summary>
        Sphere = (long)0.47,
        /// <summary>
        /// Returns 0.42
        /// </summary>
        Half_Sphere = (long)0.42,
        /// <summary>
        /// Returns 0.50
        /// </summary>
        Cone = (long)0.50,
        /// <summary>
        /// Returns  1.05
        /// </summary>
        Cube = (long)1.05,
        /// <summary>
        /// Returns 0.80
        /// </summary>
        Angle45Cube = (long) 0.80,
        /// <summary>
        /// Returns 0.82
        /// </summary>
        LongCylinder = (long) 0.82,
        /// <summary>
        /// Returns 1.15
        /// </summary>
        ShortCylinder = (long)1.15,
        /// <summary>
        /// Returns 0.04
        /// </summary>
        StreamLinedBody = (long) 0.04,
        /// <summary>
        /// Returns 0.09
        /// </summary>
        StreamLinedHalf_Body = (long) 0.09,
        /// <summary>
        /// Returns 0.9
        /// </summary>
        BiCycleWithCyclist = (long) 0.9,
        /// <summary>
        /// Returns 0.25
        /// </summary>
        LowestCar = (long) 0.25,
        /// <summary>
        /// Returns 0.295
        /// </summary>
        Bullet = (long) 0.295,
        /// <summary>
        /// Returns 1.15
        /// </summary>
        ManUpright = (long) 1.15,
        /// <summary>
        /// Returns 1.05
        /// </summary>
        Skier = (long) 1.05,
        /// <summary>
        /// Returns 1.15
        /// </summary>
        WiresAndCables = (long) 1.15,
        /// <summary>
        /// Returns 1.2
        /// </summary>
        SkiJumper = (long) 1.2,
        /// <summary>
        /// Returns 1.4
        /// </summary>
        EmpireStateBuilding = (long) 1.4,
        /// <summary>
        /// Returns 1.9
        /// </summary>
        EiffelTower = (long) 1.9,
        /// <summary>
        /// Returns 2.1
        /// </summary>
        SmoothBrick = (long) 2.1
    }
    /// <summary>
    /// Returns the materials kg/m^3 
    /// </summary>
    public enum DensityType : long
    {
        /// <summary>
        /// Returns 1.2
        /// </summary>
        Air = (long)1.2,
        /// <summary>
        /// Returns 1010
        /// </summary>
        Beer = (long) 1010,
        /// <summary>
        /// Returns 785.1
        /// </summary>
        Ethanol = (long) 785.1,
        /// <summary>
        /// Returns 910
        /// </summary>
        Automobile_Oils = (long) 910,
        /// <summary>
        /// Returns 599
        /// </summary>
        Butane = (long) 599,
        /// <summary>
        /// Returns 924
        /// </summary>
        Coconut_oil = (long) 924,
        /// <summary>
        /// Returns 1024
        /// </summary>
        Cresol = (long) 1024,
        /// <summary>
        /// Returns 915
        /// </summary>
        Crude_Oil_California = (long) 915,
        /// <summary>
        /// Returns 973
        /// </summary>
        Crude_Oil_Mexican = (long) 973,
        /// <summary>
        /// Returns 873
        /// </summary>
        Crude_Oil_Texas = (long) 873,
        /// <summary>
        /// Returns 885
        /// </summary>
        Diesel_Fuel = (long) 885,
        /// <summary>
        /// Returns 737
        /// </summary>
        Vehicle_Gasoline = (long) 737,
        /// <summary>
        /// Returns 13590
        /// </summary>
        Mercury = (long) 13590,
        /// <summary>
        /// Returns 791
        /// </summary>
        Methanol = (long) 791,
        /// <summary>
        /// Returns 1035
        /// </summary>
        Milk = (long) 1035,
        /// <summary>
        /// Returns 1560
        /// </summary>
        Nitric_Acid = (long) 1560,
        /// <summary>
        /// Returns 860
        /// </summary>
        Olive_Oil = (long) 860,
        /// <summary>
        /// Returns 1140
        /// </summary>
        Oxygen_Liquid = (long) 1140,
        /// <summary>
        /// Returns 800
        /// </summary>
        Paraffin = (long) 800,
        /// <summary>
        /// Returns 493.5
        /// </summary>
        Propane = (long) 493.5,
        /// <summary>
        /// Returns 1839
        /// </summary>
        Sulfuric_Acid_95 = (long) 1839,
        /// <summary>
        /// Returns 1105
        /// </summary>
        Water_Heavy = (long) 1105,
        /// <summary>
        /// Returns 1000
        /// </summary>
        Water_Pure = (long) 1000,
        /// <summary>
        /// Returns 1022
        /// </summary>
        Water_Sea = (long) 1022,
        /// <summary>
        /// Returns 925
        /// </summary>
        Whale_Oil = (long) 925
    }
    /// <summary>
    /// Static class for AirCoefficients
    /// </summary>
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
