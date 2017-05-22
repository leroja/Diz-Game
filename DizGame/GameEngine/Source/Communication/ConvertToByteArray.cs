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
    /// This class coverts different values to a byte[] array, that can be sent via 
    /// network functions.
    /// </summary>
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
            int len = 0;

            foreach (Vector3 vector in values)
                len += ConvertValue(ref inputArray, pos + len, vector);

            return len;
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
            int len = 0;
            len += ConvertValue(ref inputArray, pos + len, quaternion.X);
            len += ConvertValue(ref inputArray, pos + len, quaternion.Y);
            len += ConvertValue(ref inputArray, pos + len, quaternion.Z);
            len += ConvertValue(ref inputArray, pos + len, quaternion.W);

            return len;
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


        /// <summary>
        /// This function converts a matrix and inserts the bytes in the referenced array.
        /// The values converted are Rotation, Scale and Translation.
        /// </summary>
        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
        /// <param name="pos">The start position to begin insertion from.</param>
        /// <param name="matrix">The matrix to convert.</param>
        ///<returns>The length of the message in the array. The length may be less than the size of the input 
        /// array.
        ///<returns>
        private static int ConvertValue(ref Byte[] inputArray, int pos, Matrix matrix)
        {
            int len = 0;

            len += ConvertValue(ref inputArray, pos + len, matrix.Rotation);
            len += ConvertValue(ref inputArray, pos + len, matrix.Scale);
            len += ConvertValue(ref inputArray, pos + len, matrix.Translation);

            return len;
        }



        /// <summary>
        /// This function converts values of Vector3 type and inserts the bytes in the referenced array.
        /// </summary>
        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
        /// <param name="pos">The start position to begin insertion from.</param>
        /// <param name="value">The value to insert.</param>
        ///<returns>The length of the message in the array. The length may be less than the size of the input 
        /// array.
        /// </returns>
        public static int ConvertValue(ref Byte[] inputArray, int pos, Vector3 value)
        {
            int len = 0;

            len += ConvertValue(ref inputArray, pos + len, value.X);
            len += ConvertValue(ref inputArray, pos + len, value.Y);
            len += ConvertValue(ref inputArray, pos + len, value.Z);

            return len;
        }


        /// <summary>
        /// This function converts value of TransformComponent type and inserts the bytes in the referenced array.
        /// </summary>
        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
        /// <param name="pos">The start position to begin insertion from.</param>
        /// <param name="value">The value to insert.</param>
        ///<returns>The length of the message in the array. The length may be less than the size of the input 
        /// array.
        /// </returns>
        public static int ConvertValue(ref Byte[] inputArray, int pos, TransformComponent value)
        {
            int len = 0;

            len += ConvertValue(ref inputArray, pos + len, value.Dirrection);
            len += ConvertValue(ref inputArray, pos + len, value.Forward);
            len += ConvertValue(ref inputArray, pos + len, value.ID);
            len += ConvertValue(ref inputArray, pos + len, value.ObjectMatrix);
            len += ConvertValue(ref inputArray, pos + len, value.Orientation);
            len += ConvertValue(ref inputArray, pos + len, value.Position);
            len += ConvertValue(ref inputArray, pos + len, value.QuaternionRotation);
            len += ConvertValue(ref inputArray, pos + len, value.Right);
            len += ConvertValue(ref inputArray, pos + len, value.Rotation);
            len += ConvertValue(ref inputArray, pos + len, value.RotationMatrix);
            len += ConvertValue(ref inputArray, pos + len, value.Scale);
            len += ConvertValue(ref inputArray, pos + len, value.Up);

            return len;
        }
    }
}
