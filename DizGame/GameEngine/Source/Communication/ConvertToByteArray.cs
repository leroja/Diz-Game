using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework;

namespace GameEngine.Source.Communication
{
    public static class ConvertToByteArray
    {
        ///// <summary>
        ///// This function converts a value and inserts the bytes in the referenced array.
        ///// </summary>
        ///// <param name="inputArray">The zero based array to insert the bytes into.</param>
        ///// <param name="pos">The start position to begin insertion from.</param>
        ///// <param name="entityId">The entityId to send together with the components.</param>
        ///// <param name="value">The value to convert to bytes.</param>
        /////<returns>The length of the message in the array. The length may be less than the size of the input 
        ///// array.
        ///// </returns>
        //public static int ConvertValue(ref Byte[] inputArray, int pos, int entityId List<IComponent> values)
        //{
        //    //Byte[] copiedValues = BitConverter.GetBytes();

        //    //Array.Copy(copiedValues, 0, inputArray, pos, copiedValues.Length);

        //    //return copiedValues.Length;
        //}


        /// <summary>
        /// This function converts a value and inserts the bytes in the referenced array.
        /// </summary>
        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
        /// <param name="pos">The start position to begin insertion from.</param>
        /// <param name="value">The value to convert to bytes.</param>
        ///<returns>The length of the message in the array. The length may be less than the size of the input 
        /// array.
        /// </returns>
        public static int ConvertValue(ref Byte[] inputArray, int pos, Int32 value)
        {
            Byte[] copiedValues = BitConverter.GetBytes(value);

            Array.Copy(copiedValues, 0, inputArray, pos, copiedValues.Length);

            return copiedValues.Length;
        }


        /// <summary>
        /// This function converts a value and inserts the bytes in the referenced array.
        /// </summary>
        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
        /// <param name="pos">The start position to begin insertion from.</param>
        /// <param name="value">The value to insert.</param>
        ///<returns>The length of the message in the array. The length may be less than the size of the input 
        /// array.
        /// </returns>
        public static int ConvertValue(ref Byte[] inputArray, int pos, Byte value)
        {
            Byte[] copiedValues = BitConverter.GetBytes(value);

            Array.Copy(copiedValues, 0, inputArray, pos, copiedValues.Length);

            return copiedValues.Length;
        }


        /// <summary>
        /// This function converts values of Vector3 type and inserts the bytes in the referenced array.
        /// </summary>
        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
        /// <param name="pos">The start position to begin insertion from.</param>
        /// <param name="values">The List of values to insert.</param>
        ///<returns>The length of the message in the array. The length may be less than the size of the input 
        /// array.
        /// </returns>
        public static int ConvertValue(ref Byte[] inputArray, int pos, List<Vector3> values)
        {
            Byte[] copiedValues;
            int size_float = 0;
            int index = 0;

            foreach (Vector3 vector in values)
            {
                size_float = sizeof(float);
                
                //X
                copiedValues = BitConverter.GetBytes(vector.X);
                
                Array.Copy(copiedValues, 0, inputArray, pos + index++ * size_float, copiedValues.Length);

                //Y
                copiedValues = BitConverter.GetBytes(vector.Y);
                Array.Copy(copiedValues, 0, inputArray, pos + index++ * size_float, copiedValues.Length);
                
                //Z
                copiedValues = BitConverter.GetBytes(vector.Z);
                Array.Copy(copiedValues, 0, inputArray, pos + index++ * size_float, copiedValues.Length);


            }

            return index * size_float;
        }


        /// <summary>
        /// This function converts a quaternion and inserts the bytes in the referenced array.
        /// </summary>
        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
        /// <param name="pos">The start position to begin insertion from.</param>
        /// <param name="quaternion">The rotation to convert.</param>
        ///<returns>The length of the message in the array. The length may be less than the size of the input 
        /// array.
        ///<returns>
        private static int ConvertValue(ref Byte[] inputArray, int pos, Quaternion quaternion)
        {
            //Byte[] copiedValues = BitConverter.GetBytes(quaternion.);

            //Array.Copy(copiedValues, 0, inputArray, pos, copiedValues.Length);

            //return copiedValues.Length;

            return 0;
        }


        /// <summary>
        /// This function converts a float and inserts the bytes in the referenced array.
        /// </summary>
        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
        /// <param name="pos">The start position to begin insertion from.</param>
        /// <param name="value">The value to convert.</param>
        ///<returns>The length of the message in the array. The length may be less than the size of the input 
        /// array.
        ///<returns>
        public static int ConvertValue(ref Byte[] inputArray, int pos, float value)
        {
            Byte[] copiedValues = BitConverter.GetBytes(value);

            Array.Copy(copiedValues, 0, inputArray, pos, copiedValues.Length);

            return copiedValues.Length;
        }
    }
}
