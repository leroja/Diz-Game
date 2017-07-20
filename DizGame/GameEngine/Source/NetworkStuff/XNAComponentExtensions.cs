using GameEngine.Source.Components;
using Lidgren.Network;
using Lidgren.Network.Xna;
using System;

namespace GameEngine.Source.NetworkStuff
{
    /// <summary>
    /// This class extends the Lidgren Network with read and write methods for
    /// Components in used the GameEngine.
    /// </summary>
    public static class XNAComponentExtensions
    {
        /// <summary>
        /// This function writes a Component from the GameEngine to the message.
        /// </summary>
        /// <param name="message">The message used for writing components.</param>
        /// <param name="transform">The component to write into the message.</param>
        public static void WriteTransform(this NetBuffer message, TransformComponent transform)
        {
            message.Write(transform.Forward);
            message.Write(transform.ID);
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
        /// This function reads a GameEngine Component from the message.
        /// </summary>
        /// <param name="message">The message to read a component from.</param>
        /// <returns>The component from the message.</returns>
        public static TransformComponent ReadTransform(this NetBuffer message)
        {

            TransformComponent transform = new TransformComponent()
            {
                Forward = message.ReadVector3(),
                ID = message.ReadInt32(),
                ObjectMatrix = message.ReadMatrix(),
                Orientation = message.ReadRotation(24),
                Position = message.ReadVector3(),
                PreviousPosition = message.ReadVector3(),
                QuaternionRotation = message.ReadRotation(24),
                Right = message.ReadVector3(),
                Rotation = message.ReadVector3(),
                Scale = message.ReadVector3(),
                Up = message.ReadVector3()

            };

            return transform;
        }
    }
}
