//*****************************************************************
//The base of this file XNAExtensions.cs is written by Michael Lidgren
//and copied from https://github.com/lidgren/lidgren-network-gen3/blob/master/Lidgren%20XNA%20Extensions/XNAExtensions.cs
//on 2017-05-26.
//****************************************************************

using System;
using System.Collections.Generic;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;

namespace Lidgren.Network.Xna
{
    /// <summary>
    /// 
    /// </summary>
    public static class XNAExtensions
    {
        #region Lidgrens
        /// <summary>
        /// Write a Point
        /// </summary>
        public static void Write(this NetBuffer message, Point value)
        {
            message.Write(value.X);
            message.Write(value.Y);
        }

        /// <summary>
        /// Read a Point
        /// </summary>
        public static Point ReadPoint(this NetBuffer message)
        {
            return new Point(message.ReadInt32(), message.ReadInt32());
        }

        /// <summary>
        /// Write a Single with half precision (16 bits)
        /// </summary>
        public static void WriteHalfPrecision(this NetBuffer message, float value)
        {
            message.Write(new HalfSingle(value).PackedValue);
        }

        /// <summary>
        /// Reads a half precision Single written using WriteHalfPrecision(float)
        /// </summary>
        public static float ReadHalfPrecisionSingle(this NetBuffer message)
        {
            HalfSingle h = new HalfSingle()
            {
                PackedValue = message.ReadUInt16()
            };
            return h.ToSingle();
        }

        /// <summary>
        /// Writes a Vector2
        /// </summary>
        public static void Write(this NetBuffer message, Vector2 vector)
        {
            message.Write(vector.X);
            message.Write(vector.Y);
        }

        /// <summary>
        /// Reads a Vector2
        /// </summary>
        public static Vector2 ReadVector2(this NetBuffer message)
        {
            Vector2 retval;
            retval.X = message.ReadSingle();
            retval.Y = message.ReadSingle();
            return retval;
        }

        /// <summary>
        /// Writes a Vector3
        /// </summary>
        public static void Write(this NetBuffer message, Vector3 vector)
        {
            message.Write(vector.X);
            message.Write(vector.Y);
            message.Write(vector.Z);
        }

        /// <summary>
        /// Writes a Vector3 at half precision
        /// </summary>
        public static void WriteHalfPrecision(this NetBuffer message, Vector3 vector)
        {
            message.Write(new HalfSingle(vector.X).PackedValue);
            message.Write(new HalfSingle(vector.Y).PackedValue);
            message.Write(new HalfSingle(vector.Z).PackedValue);
        }

        /// <summary>
        /// Reads a Vector3
        /// </summary>
        public static Vector3 ReadVector3(this NetBuffer message)
        {
            Vector3 retval;
            retval.X = message.ReadSingle();
            retval.Y = message.ReadSingle();
            retval.Z = message.ReadSingle();
            return retval;
        }

        /// <summary>
        /// Writes a Vector3 at half precision
        /// </summary>
        public static Vector3 ReadHalfPrecisionVector3(this NetBuffer message)
        {
            HalfSingle hx = new HalfSingle()
            {
                PackedValue = message.ReadUInt16()
            };
            HalfSingle hy = new HalfSingle()
            {
                PackedValue = message.ReadUInt16()
            };
            HalfSingle hz = new HalfSingle()
            {
                PackedValue = message.ReadUInt16()
            };
            Vector3 retval;
            retval.X = hx.ToSingle();
            retval.Y = hy.ToSingle();
            retval.Z = hz.ToSingle();
            return retval;
        }

        /// <summary>
        /// Writes a Vector4
        /// </summary>
        public static void Write(this NetBuffer message, Vector4 vector)
        {
            message.Write(vector.X);
            message.Write(vector.Y);
            message.Write(vector.Z);
            message.Write(vector.W);
        }

        /// <summary>
        /// Reads a Vector4
        /// </summary>
        public static Vector4 ReadVector4(this NetBuffer message)
        {
            Vector4 retval;
            retval.X = message.ReadSingle();
            retval.Y = message.ReadSingle();
            retval.Z = message.ReadSingle();
            retval.W = message.ReadSingle();
            return retval;
        }


        /// <summary>
        /// Writes a unit vector (ie. a vector of length 1.0, for example a surface normal) 
        /// using specified number of bits
        /// </summary>
        public static void WriteUnitVector3(this NetBuffer message, Vector3 unitVector, int numberOfBits)
        {
            float x = unitVector.X;
            float y = unitVector.Y;
            float z = unitVector.Z;
            double invPi = 1.0 / Math.PI;
            float phi = (float)(Math.Atan2(x, y) * invPi);
            float theta = (float)(Math.Atan2(z, Math.Sqrt(x * x + y * y)) * (invPi * 2));

            int halfBits = numberOfBits / 2;
            message.WriteSignedSingle(phi, halfBits);
            message.WriteSignedSingle(theta, numberOfBits - halfBits);
        }

        /// <summary>
        /// Reads a unit vector written using WriteUnitVector3(numberOfBits)
        /// </summary>
        public static Vector3 ReadUnitVector3(this NetBuffer message, int numberOfBits)
        {
            int halfBits = numberOfBits / 2;
            float phi = message.ReadSignedSingle(halfBits) * (float)Math.PI;
            float theta = message.ReadSignedSingle(numberOfBits - halfBits) * (float)(Math.PI * 0.5);

            Vector3 retval;
            retval.X = (float)(Math.Sin(phi) * Math.Cos(theta));
            retval.Y = (float)(Math.Cos(phi) * Math.Cos(theta));
            retval.Z = (float)Math.Sin(theta);

            return retval;
        }

        /// <summary>
        /// Writes a unit quaternion using the specified number of bits per element
        /// for a total of 4 x bitsPerElements bits. Suggested value is 8 to 24 bits.
        /// </summary>
        public static void WriteRotation(this NetBuffer message, Quaternion quaternion, int bitsPerElement)
        {
            if (quaternion.X > 1.0f)
                quaternion.X = 1.0f;
            if (quaternion.Y > 1.0f)
                quaternion.Y = 1.0f;
            if (quaternion.Z > 1.0f)
                quaternion.Z = 1.0f;
            if (quaternion.W > 1.0f)
                quaternion.W = 1.0f;
            if (quaternion.X < -1.0f)
                quaternion.X = -1.0f;
            if (quaternion.Y < -1.0f)
                quaternion.Y = -1.0f;
            if (quaternion.Z < -1.0f)
                quaternion.Z = -1.0f;
            if (quaternion.W < -1.0f)
                quaternion.W = -1.0f;

            message.WriteSignedSingle(quaternion.X, bitsPerElement);
            message.WriteSignedSingle(quaternion.Y, bitsPerElement);
            message.WriteSignedSingle(quaternion.Z, bitsPerElement);
            message.WriteSignedSingle(quaternion.W, bitsPerElement);
        }

        /// <summary>
        /// Reads a unit quaternion written using WriteRotation(... ,bitsPerElement)
        /// </summary>
        public static Quaternion ReadRotation(this NetBuffer message, int bitsPerElement)
        {
            Quaternion retval;
            retval.X = message.ReadSignedSingle(bitsPerElement);
            retval.Y = message.ReadSignedSingle(bitsPerElement);
            retval.Z = message.ReadSignedSingle(bitsPerElement);
            retval.W = message.ReadSignedSingle(bitsPerElement);
            return retval;
        }

        /// <summary>
        /// Writes an orthonormal matrix (rotation, translation but not scaling or projection)
        /// </summary>
        public static void WriteMatrix(this NetBuffer message, ref Matrix matrix)
        {
            Quaternion rot = Quaternion.CreateFromRotationMatrix(matrix);
            WriteRotation(message, rot, 24);
            message.Write(matrix.M41);
            message.Write(matrix.M42);
            message.Write(matrix.M43);
        }

        /// <summary>
        /// Writes an orthonormal matrix (rotation, translation but no scaling or projection)
        /// </summary>
        public static void WriteMatrix(this NetBuffer message, Matrix matrix)
        {
            Quaternion rot = Quaternion.CreateFromRotationMatrix(matrix);
            WriteRotation(message, rot, 24);
            message.Write(matrix.M41);
            message.Write(matrix.M42);
            message.Write(matrix.M43);
        }

        /// <summary>
        /// Reads a matrix written using WriteMatrix()
        /// </summary>
        public static Matrix ReadMatrix(this NetBuffer message)
        {
            Quaternion rot = ReadRotation(message, 24);
            Matrix retval = Matrix.CreateFromQuaternion(rot);
            retval.M41 = message.ReadSingle();
            retval.M42 = message.ReadSingle();
            retval.M43 = message.ReadSingle();
            return retval;
        }

        /// <summary>
        /// Reads a matrix written using WriteMatrix()
        /// </summary>
        public static void ReadMatrix(this NetBuffer message, ref Matrix destination)
        {
            Quaternion rot = ReadRotation(message, 24);
            destination = Matrix.CreateFromQuaternion(rot);
            destination.M41 = message.ReadSingle();
            destination.M42 = message.ReadSingle();
            destination.M43 = message.ReadSingle();
        }

        /// <summary>
        /// Writes a bounding sphere
        /// </summary>
        public static void Write(this NetBuffer message, BoundingSphere bounds)
        {
            message.Write(bounds.Center.X);
            message.Write(bounds.Center.Y);
            message.Write(bounds.Center.Z);
            message.Write(bounds.Radius);
        }

        /// <summary>
        /// Reads a bounding sphere written using Write(message, BoundingSphere)
        /// </summary>
        public static BoundingSphere ReadBoundingSphere(this NetBuffer message)
        {
            BoundingSphere retval;
            retval.Center.X = message.ReadSingle();
            retval.Center.Y = message.ReadSingle();
            retval.Center.Z = message.ReadSingle();
            retval.Radius = message.ReadSingle();
            return retval;
        }

        #endregion



        /*******************************************************************/
        /* Custom methods for reading and writing GameEngine related stuff */
        /*******************************************************************/

        //TODO: Not sure we need to send everything, because we can calculate some things when we receive them from the other stuff
        /// <summary>
        /// This function writes a TransformComponent from the GameEngine to the message.
        /// </summary>
        /// <param name="message">The message used for writing components.</param>
        /// <param name="transform">The TransformComponent to write into the message.</param>
        public static void WriteTransform(this NetBuffer message, TransformComponent transform)
        {
            message.Write(transform.Forward);
            //message.Write(transform.ID);
            message.WriteMatrix(transform.ObjectMatrix);
            message.WriteRotation(transform.Orientation, 24);
            message.Write(transform.Position);
            message.Write(transform.PreviousPosition);
            message.WriteRotation(transform.QuaternionRotation, 24);
            message.Write(transform.Right);
            message.Write(transform.Rotation);
            message.Write(transform.Scale);
            message.Write(transform.Up);
        }

        /// <summary>
        /// This function reads a TransformComponent from the message.
        /// </summary>
        /// <param name="message">The message to read a component from.</param>
        /// <returns>The TransformComponent from the message.</returns>
        public static TransformComponent ReadTransform(this NetBuffer message)
        {
            return new TransformComponent()
            {
                Forward = message.ReadVector3(),
                //ID = message.ReadInt32(),
                ObjectMatrix = message.ReadMatrix(),
                Orientation = message.ReadRotation(24),
                Position = message.ReadVector3(),
                PreviousPosition = message.ReadVector3(),
                QuaternionRotation = message.ReadRotation(24),
                Right = message.ReadVector3(),
                Rotation = message.ReadVector3(),
                Scale = message.ReadVector3(),
                Up = message.ReadVector3(),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="physics"></param>
        public static void WritePhysics(this NetBuffer message, PhysicsComponent physics)
        {
            message.Write(physics.Acceleration);
            message.Write(physics.Bounciness);
            message.Write(physics.Density);
            message.Write((long)physics.DragType);
            message.Write(physics.Forces);
            message.Write(physics.Friction);
            message.Write(physics.Gravity);
            message.Write((int)physics.GravityType);
            message.Write(physics.InverseMass);
            message.Write(physics.IsInAir);
            message.Write(physics.IsMoving);
            message.Write(physics.LastAcceleration);
            message.Write(physics.Mass);
            message.Write((int)physics.MaterialType);
            message.Write(physics.MaxVelocity);
            message.Write((int)physics.PhysicsType);
            message.Write(physics.ReferenceArea);
            message.Write(physics.Velocity);
            message.Write(physics.Volume);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static PhysicsComponent ReadPhysics(this NetBuffer message)
        {
            return new PhysicsComponent()
            {
                Acceleration = message.ReadVector3(),
                Bounciness = message.ReadFloat(),
                Density = message.ReadFloat(),
                DragType = (DragType)message.ReadInt64(),
                Forces = message.ReadVector3(),
                Friction = message.ReadFloat(),
                Gravity = message.ReadFloat(),
                GravityType = (GravityType)message.ReadInt32(),
                InverseMass = message.ReadFloat(),
                IsInAir = message.ReadBoolean(),
                IsMoving = message.ReadBoolean(),
                LastAcceleration = message.ReadVector3(),
                Mass = message.ReadFloat(),
                MaterialType = (MaterialType)message.ReadInt32(),
                MaxVelocity = message.ReadVector3(),
                PhysicsType = (PhysicsType)message.ReadInt32(),
                ReferenceArea = message.ReadFloat(),
                Velocity = message.ReadVector3(),
                Volume = message.ReadFloat(),
            };
        }
    }
}