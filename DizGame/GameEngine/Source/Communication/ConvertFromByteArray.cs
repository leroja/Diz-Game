using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework;

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



        public static void ConvertValue(Byte[] inputArray, int pos, out int value)
        {
            value = BitConverter.ToInt32(inputArray, pos);
        }


        public static void ConvertValue(Byte[] inputArray, int pos, out Byte value)
        {
            value = (Byte)BitConverter.ToChar(inputArray, pos);
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



        //private static void ConvertValue(ref Byte[] inputArray, int pos, out Quaternion quaternion)
        //{
        //    int len = 0;
        //    float r = 8f;

        //    int sizeFloat = 4;


        //    ConvertValue()
        //    Quaternion q = new Quaternion()
        //    len += ConvertValue(ref inputArray, pos + len, quaternion.X);
        //    len += ConvertValue(ref inputArray, pos + len, quaternion.Y);
        //    len += ConvertValue(ref inputArray, pos + len, quaternion.Z);
        //    len += ConvertValue(ref inputArray, pos + len, quaternion.W);

        //}


        ///// <summary>
        ///// This function converts a float and inserts the bytes in the referenced array.
        ///// </summary>
        ///// <param name="inputArray">The zero based array to insert the bytes into.</param>
        ///// <param name="pos">The start position to begin insertion from.</param>
        ///// <param name="value">The value to convert.</param>
        /////<returns>The length of the message in the array. The length may be less than the size of the input 
        ///// array.
        /////<returns>
        //public static int ConvertValue(ref Byte[] inputArray, int pos, float value)
        //{
        //    Byte[] copiedValues = BitConverter.GetBytes(value);

        //    Array.Copy(copiedValues, 0, inputArray, pos, copiedValues.Length);

        //    return copiedValues.Length;
        //}


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
    }
}
