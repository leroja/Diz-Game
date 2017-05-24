using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;

namespace GameEngine.Source.Communication
{
    /// <summary>
    /// This class converts values from an Byte[] array back to it's original.
    /// </summary>
    public class ConvertFromByteArray
    {
        //All functions commented here needs to be rewritten to convert the values back to it's original.
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //This function is the beginning of converting - but it does not work as is...
        //public static string  ConvertValue(Byte[] inputArray)
        //{
        //    char character;
        //    int index = 0;
        //    int len = 0;
        //    string copiedValues = "";
        //    int charLen = 2;

        //    for(index=0; index < inputArray.Length; index++)
        //    {
        //        character = BitConverter.ToChar(inputArray, index);
        //        len += charLen;
        //        //Array.Copy(copiedValues, 0, inputArray, pos * index++ * copiedValues.Length, copiedValues.Length);
        //    }
        //    return copiedValues;
        //}


        ///// <summary>
        ///// This function converts values of Int32 from Byte[] array.
        ///// </summary>
        ///// <param name="inputArray">The zero based array to read the bytes from.</param>
        ///// <param name="pos">The start position to begin reading from in the input array.</param>
        /// <param name="value">The converted value.</param>
        /// <returns>The advanced position in the input array where the next data type can be read from.</returns>
        public static int ConvertValue(Byte[] inputArray, int pos, out int value)
        {
            value = BitConverter.ToInt32(inputArray, pos);
            return pos + sizeof(int);
        }


        ///// <summary>
        ///// This function converts values of Byte from Byte[] array.
        ///// </summary>
        ///// <param name="inputArray">The zero based array to read the bytes from.</param>
        ///// <param name="pos">The start position to begin reading from in the input array.</param>
        /// <param name="value">The converted value.</param>
        /// <returns>The advanced position in the input array where the next data type can be read from.</returns>
        public static int ConvertValue(Byte[] inputArray, int pos, out Byte value)
        {
            value = (Byte)BitConverter.ToChar(inputArray, pos);
            return pos + sizeof(Byte) * 2;
        }


        ///// <summary>
        ///// This function converts values of Vector3 type and inserts the bytes in the referenced array.
        ///// </summary>
        ///// <param name="inputArray">The zero based array to insert the bytes into.</param>
        ///// <param name="pos">The start position to begin insertion from.</param>
        ///// <param name="values">The List of values to insert.</param>
        /////<returns>The length of the message in the array. The length may be less than the size of the input 
        ///// array.
        ///// </returns>
        //public static int ConvertValue(ref Byte[] inputArray, int pos, List<Vector3> values)
        //{
        //    int len = 0;

        //    foreach (Vector3 vector in values)
        //        len += ConvertValue(ref inputArray, pos + len, vector);

        //    return len;
        //}


        /// <summary>
        /// This method gets a quaternion from a byte array
        /// </summary>
        /// <param name="inputArray"></param>
        /// <param name="pos"></param>
        /// <returns> A Quaternion that has been picked out from the Byte array at a given position</returns>
        private static int ConvertValue(Byte[] inputArray, int pos, out Quaternion quaternion)
        {
            float value;    
             //om det nu blir +4 i inputarrayen
            Quaternion qua = new Quaternion();
             ConvertValue(inputArray, pos, out value);
            qua.X = value;
            pos += Marshal.SizeOf(value); //så kan man göra för att få ut den riktiga storleken
            ConvertValue(inputArray, pos, out value);
            qua.Y = value;
            pos += Marshal.SizeOf(value);
            ConvertValue(inputArray, pos, out value);
            qua.Z = value;
            pos += Marshal.SizeOf(value);
            ConvertValue(inputArray, pos, out value);
            qua.W = value;
            quaternion = qua;
            //pos += Marshal.SizeOf(value);
            return pos;

        }
        /// <summary>
        /// This method coverts whole byte array to a string
        /// </summary>
        /// <param name="inputArray"></param>
        /// <returns> input array as a string </returns>
        public static string ConvertValueToString(Byte[] inputArray)
        {
            var value = ASCIIEncoding.ASCII.GetString(inputArray);
            return value;
            //Behöver vi en där man anger position i inputArrayen också? :)
            // dvs, vill vi ha ut hela byte arrayen som sträng eller bara en del?
        }


        /// <summary>
        /// This method converts to float
        /// </summary>
        /// <param name="inputArray"></param>
        /// <param name="pos"></param>
        ///  /// <returns> A float at a given position from the Byte array </returns>
        public static int ConvertValue(Byte[] inputArray, int pos, out float value)
        {
            var retValue = BitConverter.ToDouble(inputArray, pos);
            value = (float)retValue;
            pos += Marshal.SizeOf(retValue);
            return pos;
        }

        /// <summary>
        /// This method picks put a vector3 from the byte array at a given position
        /// </summary>
        /// <param name="inputArray"></param>
        /// <param name="pos"></param>
        /// <returns>A vector3</returns>
        private static int ConvertValueToVector3(Byte[] inputArray, int pos, out Vector3 vector)
        {
            float value;
            Vector3 vec = new Vector3();
            ConvertValue(inputArray, pos, out value);
            vec.X = value;
            pos += Marshal.SizeOf(value);
            ConvertValue(inputArray, pos, out value);
            vec.Y = value;            
            pos += Marshal.SizeOf(value);
            ConvertValue(inputArray, pos, out value);
            vec.Z = value;
            pos += Marshal.SizeOf(value);
            vector = vec;
            return pos;
        }



        ///// <summary>
        ///// This function converts a matrix and inserts the bytes in the referenced array.
        ///// The values converted are Rotation, Scale and Translation.
        ///// </summary>
        ///// <param name="inputArray">The zero based array to insert the bytes into.</param>
        ///// <param name="pos">The start position to begin insertion from.</param>
        ///// <param name="matrix">The matrix to convert.</param>
        /////<returns>The length of the message in the array. The length may be less than the size of the input 
        ///// array.
        /////<returns>
        //private static int ConvertValue(ref Byte[] inputArray, int pos, Matrix matrix)
        //{
        //    int len = 0;

        //    len += ConvertValue(ref inputArray, pos + len, matrix.Rotation);
        //    len += ConvertValue(ref inputArray, pos + len, matrix.Scale);
        //    len += ConvertValue(ref inputArray, pos + len, matrix.Translation);

        //    return len;
        //}



        ///// <summary>
        ///// This function converts values of Vector3 type and inserts the bytes in the referenced array.
        ///// </summary>
        ///// <param name="inputArray">The zero based array to insert the bytes into.</param>
        ///// <param name="pos">The start position to begin insertion from.</param>
        ///// <param name="value">The value to insert.</param>
        /////<returns>The length of the message in the array. The length may be less than the size of the input 
        ///// array.
        ///// </returns>
        //public static int ConvertValue(ref Byte[] inputArray, int pos, Vector3 value)
        //{
        //    int len = 0;

        //    len += ConvertValue(ref inputArray, pos + len, value.X);
        //    len += ConvertValue(ref inputArray, pos + len, value.Y);
        //    len += ConvertValue(ref inputArray, pos + len, value.Z);

        //    return len;
        //}


        ///// <summary>
        ///// This function converts value of TransformComponent type and inserts the bytes in the referenced array.
        ///// </summary>
        ///// <param name="inputArray">The zero based array to insert the bytes into.</param>
        ///// <param name="pos">The start position to begin insertion from.</param>
        ///// <param name="value">The value to insert.</param>
        /////<returns>The length of the message in the array. The length may be less than the size of the input 
        ///// array.
        ///// </returns>
        //public static int ConvertValue(ref Byte[] inputArray, int pos, TransformComponent value)
        //{
        //    int len = 0;

        //    //len += ConvertValue(ref inputArray, pos + len, value.Dirrection);
        //    len += ConvertValue(ref inputArray, pos + len, value.Forward);
        //    len += ConvertValue(ref inputArray, pos + len, value.ID);
        //    len += ConvertValue(ref inputArray, pos + len, value.ObjectMatrix);
        //    len += ConvertValue(ref inputArray, pos + len, value.Orientation);
        //    len += ConvertValue(ref inputArray, pos + len, value.Position);
        //    len += ConvertValue(ref inputArray, pos + len, value.QuaternionRotation);
        //    len += ConvertValue(ref inputArray, pos + len, value.Right);
        //    len += ConvertValue(ref inputArray, pos + len, value.Rotation);
        //    //len += ConvertValue(ref inputArray, pos + len, value.RotationMatrix);
        //    len += ConvertValue(ref inputArray, pos + len, value.Scale);
        //    len += ConvertValue(ref inputArray, pos + len, value.Up);

        //    return len;
        //}



        ///// <summary>
        ///// This function converts values of bool from Byte[] array.
        ///// </summary>
        ///// <param name="inputArray">The zero based array to read the bytes from.</param>
        ///// <param name="pos">The start position to begin reading from in the input array.</param>
        /// <param name="value">The converted value.</param>
        /// <returns>The advanced position in the input array where the next data type can be read from.</returns>
        public static int ConvertValue(Byte[] inputArray, int pos, out bool value)
        {
            value = BitConverter.ToBoolean(inputArray, pos);
            return pos + sizeof(bool);
        }

    }
}
