using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class Util
    {
        private static Random _random = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static double GetRandomNumber(double minimum, double maximum)
        {
            return _random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
