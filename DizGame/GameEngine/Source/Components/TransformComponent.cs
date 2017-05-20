﻿using Microsoft.Xna.Framework;

namespace GameEngine.Source.Components
{
    /// <summary>
    /// Holds data such as position, rotation and scaling.
    /// </summary>
    public class TransformComponent : IComponent
    {
        /// <summary>
        /// Vector3 representing the position for an object
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// Vector3 representing the prevoius position
        /// </summary>
        public Vector3 PreviousPosition { get; set; }
        /// <summary>
        /// Vector3 which represents the rotation for an object
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// Quaternion rotation to prevent Gimballock
        /// </summary>
        public Quaternion QuaternionRotation;
        /// <summary>
        /// Quaternion oritentation in order to prevent Gimballock
        /// </summary>
        public Quaternion Orientation { get; set; }
        /// <summary>
        /// Vector3 which represents the forward motion of an object
        /// </summary>
        public Vector3 Forward { get; set; }
        /// <summary>
        /// Vector3 which represents an upward motion of an object
        /// </summary>
        public Vector3 Up { get; set; }
        /// <summary>
        /// Vector3 which represents an motion to the right for some object
        /// </summary>
        public Vector3 Right { get; set; }
        /// <summary>
        /// Vector3 which represents a potential scaling of an object
        /// </summary>
        public Vector3 Scale { get; set; }
        /// <summary>
        /// Matrix which is the ModelMatrix for an object
        /// </summary>
        public Matrix ModelMatrix { get; set; }
        /// <summary>
        /// Matrix which is the ObjectMatrix for an object
        /// </summary>
        public Matrix ObjectMatrix { get; set; }
        /// <summary>
        /// Matrix for describing a rotation of an object.
        /// </summary>
        public Matrix RotationMatrix { get; set; }

        /// <summary>
        /// Basic Constructor for a TransformComponent
        /// </summary>
        /// <param name="position">Takes a Vector3 which should represent the position of an object</param>
        /// <param name="scale">The scale of the object which should be represented as a Vector3</param>
        public TransformComponent(Vector3 position, Vector3 scale)
        {
            Rotation = Vector3.Zero;
            this.Position = position;
            this.PreviousPosition = position;
            this.Scale = scale;
            this.QuaternionRotation = Quaternion.Identity;
            this.Orientation = Quaternion.Identity;
            this.RotationMatrix = Matrix.Identity;
            this.ModelMatrix = Matrix.Identity;
        }

        /// <summary>
        /// Alternative Constructor for a TransformComponent
        /// </summary>
        /// <param name="position">A Vector3 that should represent the position of an object</param>
        /// <param name="scale">The scaling of the object which should be represented by a Vector3</param>
        /// <param name="rotationMatrix">A matrix that should describe the desired rotation for the object</param>
        public TransformComponent(Vector3 position, Vector3 scale, Matrix rotationMatrix)
        {
            this.Position = position;
            this.PreviousPosition = position;
            this.Scale = scale;
            this.QuaternionRotation = Quaternion.Identity;
            this.Orientation = Quaternion.Identity;
            this.RotationMatrix = rotationMatrix;
            this.ModelMatrix = Matrix.Identity;
        }
    }
}
