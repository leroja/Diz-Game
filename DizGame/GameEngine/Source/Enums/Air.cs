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
    public enum AirTemperatur : int
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
    public static class AirCoefficients
    {
        /// <summary>
        /// Returns an tuple representing Tuple(SpeedOfSound(c in m*s^-1), DensityOfAir(p in kg*m^-3))
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Tuple<float, float> GetAirSoundSpeedAndDensity(AirTemperatur type)
        {
            switch(type)
            {
                case AirTemperatur.Minus25:
                    return new Tuple<float, float>(315.5f, 1.423f);
                case AirTemperatur.Minus20:
                    return new Tuple<float, float>(318.9f,1.395f);
                case AirTemperatur.Minus15:
                    return new Tuple<float, float>(322.1f, 1.368f);
                case AirTemperatur.Minus10:
                    return new Tuple<float, float>(325.2f, 1.342f);
                case AirTemperatur.Minus5:
                    return new Tuple<float, float>(328.3f, 1.317f);
                case AirTemperatur.Zero:
                    return new Tuple<float, float>(331.3f, 1.292f);
                case AirTemperatur.Plus5:
                    return new Tuple<float, float>(334.3f, 1.269f);
                case AirTemperatur.Plus10:
                    return new Tuple<float, float>(337.3f, 1.247f);
                case AirTemperatur.Plus15:
                    return new Tuple<float, float>(340.3f, 1.225f);
                case AirTemperatur.Plus20:
                    return new Tuple<float, float>(343.2f, 1.204f);
                case AirTemperatur.Plus25:
                    return new Tuple<float, float>(346.1f, 1.184f);
                case AirTemperatur.Plus30:
                    return new Tuple<float, float>(349.0f, 1.164f);
                case AirTemperatur.Plus35:
                    return new Tuple<float, float>(351.9f, 1.146f);
                default:
                    return new Tuple<float, float>(331.3f, 1.292f);
            }
        }

    }
}
