using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Utils
{
    /// <summary>
    /// A Class that contains utility functions
    /// </summary>
    public static class Util
    {
        private static Random _random = new Random();

        /// <summary>
        /// Returns a random double between minumim and maximun
        /// </summary>
        /// <param name="minimum"> Minimum Value </param>
        /// <param name="maximum"> Maximun Value </param>
        /// <returns></returns>
        public static double GetRandomNumber(double minimum, double maximum)
        {
            return _random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
