using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Communication
{
    public static class ConvertToByteArray
    {

        /// <summary>
        /// This function converts a value and inserts the bytes in the referenced array.
        /// </summary>
        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
        /// <param name="pos">The start position to begin insertion from.</param>
        /// <param name="value">The value to convert to bytes.</param>
        public static int ConvertValue(ref Byte[] inputArray, int pos, Int32 value)
        {
            Byte[] copiedValues = BitConverter.GetBytes(value);

            Array.Copy(copiedValues, inputArray, copiedValues.Length);

            return copiedValues.Length;
        }


        /// <summary>
        /// This function converts a value and inserts the bytes in the referenced array.
        /// </summary>
        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
        /// <param name="pos">The start position to begin insertion from.</param>
        /// <param name="value">The value to insert.</param>
        public static int ConvertValue(ref Byte[] inputArray, int pos, Byte value)
        {
            Byte[] copiedValues = BitConverter.GetBytes(value);

            Array.Copy(copiedValues, 0, inputArray, pos, copiedValues.Length);

            return copiedValues.Length;
        }

    }
}
