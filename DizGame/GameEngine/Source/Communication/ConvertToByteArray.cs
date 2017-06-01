//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using GameEngine.Source.Components;
//using Microsoft.Xna.Framework;

//namespace GameEngine.Source.Communication
//{
//    /// <summary>
//    /// This class coverts different values to a byte[] array, that can be sent via 
//    /// network functions.
//    /// </summary>
//    public static class ConvertToByteArray
//    {
//        /// <summary>
//        /// This function converts a value and inserts the bytes in the referenced array.
//        /// </summary>
//        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
//        /// <param name="pos">The start position to begin insertion from.</param>
//        /// <param name="value">The value to convert to bytes.</param>
//        ///<returns>The advancement of the pos in the input array where to read the next data '
//        ///type from.
//        /// </returns>
//        public static int ConvertValue(ref Byte[] inputArray, int pos, string value)
//        {
//            Byte[] copiedValues;
//            Byte[] lengthValue;

//            copiedValues = ASCIIEncoding.ASCII.GetBytes(value);
//            lengthValue = BitConverter.GetBytes(copiedValues.Length);

//            Array.Copy(lengthValue, 0, inputArray, pos, lengthValue.Length);
//            Array.Copy(copiedValues, 0, inputArray, pos + lengthValue.Length, copiedValues.Length);

//            return pos + lengthValue.Length +  copiedValues.Length;
//        }


//        /// <summary>
//        /// This function converts a value and inserts the bytes in the referenced array.
//        /// </summary>
//        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
//        /// <param name="pos">The start position to begin insertion from.</param>
//        /// <param name="value">The value to convert to bytes.</param>
//        ///<returns>The advancement of the pos in the input array where to read the next data '
//        ///type from.
//        /// </returns>
//        public static int ConvertValue(ref Byte[] inputArray, int pos, Int32 value)
//        {
//            Byte[] copiedValues = BitConverter.GetBytes(value);

//            Array.Copy(copiedValues, 0, inputArray, pos, copiedValues.Length);

//            return pos + copiedValues.Length;
//        }


//        /// <summary>
//        /// This function converts a value and inserts the bytes in the referenced array.
//        /// </summary>
//        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
//        /// <param name="pos">The start position to begin insertion from.</param>
//        /// <param name="value">The value to insert.</param>
//        ///<returns>The advancement of the pos in the input array where to read the next data '
//        ///type from.
//        /// </returns>
//        public static int ConvertValue(ref Byte[] inputArray, int pos, Byte value)
//        {
//            Byte[] copiedValues = BitConverter.GetBytes(value);

//            Array.Copy(copiedValues, 0, inputArray, pos, copiedValues.Length);

//            return pos + copiedValues.Length;
//        }


//        /// <summary>
//        /// This function converts values of Vector3 type and inserts the bytes in the referenced array.
//        /// </summary>
//        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
//        /// <param name="pos">The start position to begin insertion from.</param>
//        /// <param name="values">The List of values to insert.</param>
//        ///<returns>The advancement of the pos in the input array where to read the next data '
//        ///type from.
//        /// </returns>
//        public static int ConvertValue(ref Byte[] inputArray, int pos, List<Vector3> values)
//        {
//            foreach (Vector3 vector in values)
//                pos = ConvertValue(ref inputArray, pos, vector);

//            return pos;
//        }


//        /// <summary>
//        /// This function converts a quaternion and inserts the bytes in the referenced array.
//        /// </summary>
//        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
//        /// <param name="pos">The start position to begin insertion from.</param>
//        /// <param name="quaternion">The rotation to convert.</param>
//        ///<returns>The advancement of the pos in the input array where to read the next data '
//        ///type from.
//        ///<returns>
//        private static int ConvertValue(ref Byte[] inputArray, int pos, Quaternion quaternion)
//        {
//            pos = ConvertValue(ref inputArray, pos, quaternion.X);
//            pos = ConvertValue(ref inputArray, pos, quaternion.Y);
//            pos = ConvertValue(ref inputArray, pos, quaternion.Z);
//            pos = ConvertValue(ref inputArray, pos, quaternion.W);

//            return pos;
//        }


//        /// <summary>
//        /// This function converts a float and inserts the bytes in the referenced array.
//        /// </summary>
//        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
//        /// <param name="pos">The start position to begin insertion from.</param>
//        /// <param name="value">The value to convert.</param>
//        ///<returns>The advancement of the pos in the input array where to read the next data '
//        ///type from.
//        ///<returns>
//        public static int ConvertValue(ref Byte[] inputArray, int pos, float value)
//        {
//            Byte[] copiedValues = BitConverter.GetBytes(value);

//            Array.Copy(copiedValues, 0, inputArray, pos, copiedValues.Length);

//            return pos + copiedValues.Length;
//        }


//        /// <summary>
//        /// This function converts a matrix and inserts the bytes in the referenced array.
//        /// The values converted are Rotation, Scale and Translation.
//        /// </summary>
//        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
//        /// <param name="pos">The start position to begin insertion from.</param>
//        /// <param name="matrix">The matrix to convert.</param>
//        ///<returns>The advancement of the pos in the input array where to read the next data '
//        ///type from.
//        ///<returns>
//        private static int ConvertValue(ref Byte[] inputArray, int pos, Matrix matrix)
//        {
//            pos = ConvertValue(ref inputArray, pos, matrix.Rotation);
//            pos = ConvertValue(ref inputArray, pos, matrix.Scale);
//            pos = ConvertValue(ref inputArray, pos, matrix.Translation);

//            return pos;
//        }



//        /// <summary>
//        /// This function converts values of Vector3 type and inserts the bytes in the referenced array.
//        /// </summary>
//        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
//        /// <param name="pos">The start position to begin insertion from.</param>
//        /// <param name="value">The value to insert.</param>
//        ///<returns>The advancement of the pos in the input array where to read the next data '
//        ///type from.
//        /// </returns>
//        public static int ConvertValue(ref Byte[] inputArray, int pos, Vector3 value)
//        {
//            pos = ConvertValue(ref inputArray, pos, value.X);
//            pos = ConvertValue(ref inputArray, pos, value.Y);
//            pos = ConvertValue(ref inputArray, pos, value.Z);

//            return pos;
//        }


//        /// <summary>
//        /// This function converts value of TransformComponent type and inserts the bytes in the referenced array.
//        /// </summary>
//        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
//        /// <param name="pos">The start position to begin insertion from.</param>
//        /// <param name="value">The value to insert.</param>
//        ///<returns>The advancement of the pos in the input array where to read the next data '
//        ///type from.
//        /// </returns>
//        public static int ConvertValue(ref Byte[] inputArray, int pos, TransformComponent value)
//        {
           
//            pos = ConvertValue(ref inputArray, pos, value.Forward);
//            pos = ConvertValue(ref inputArray, pos, value.ID);
//            pos = ConvertValue(ref inputArray, pos, value.ObjectMatrix);
//            pos = ConvertValue(ref inputArray, pos, value.Orientation);
//            pos = ConvertValue(ref inputArray, pos, value.Position);
//            pos = ConvertValue(ref inputArray, pos, value.QuaternionRotation);
//            pos = ConvertValue(ref inputArray, pos, value.Right);
//            pos = ConvertValue(ref inputArray, pos, value.Rotation);
//            pos = ConvertValue(ref inputArray, pos, value.Scale);
//            pos = ConvertValue(ref inputArray, pos, value.Up);

//            return pos;
//        }


//        /// <summary>
//        /// This function converts value of bool type and inserts the bytes in the referenced array.
//        /// </summary>
//        /// <param name="inputArray">The zero based array to insert the bytes into.</param>
//        /// <param name="pos">The start position to begin insertion from.</param>
//        /// <param name="value">The value to insert.</param>
//        ///<returns>The advancement of the pos in the input array where to read the next data '
//        ///type from.
//        /// </returns>
//        public static int ConvertValue(ref Byte[] inputArray, int pos, bool value)
//        {
//            Byte[] copiedValues = BitConverter.GetBytes(value);

//            Array.Copy(copiedValues, 0, inputArray, pos, copiedValues.Length);

//            return pos + copiedValues.Length;
//        }
//    }
//}
