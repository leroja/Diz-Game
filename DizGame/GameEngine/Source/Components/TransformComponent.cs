using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameEngine.Source.Components
{
    public class TransformComponent : IComponent
    {
        //Holds data such as position, rotation and scaling.

        /// <summary>
        /// Vector3 representing the position for an object
        /// </summary>
        public Vector3 Position { get; set; }

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
        /// Vector3 which represents the dirrection of an object
        /// </summary>
        public Vector3 Dirrection { get; set; }
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
            this.Position = position;
            this.Scale = scale;
            Dirrection = Vector3.Zero;
            this.QuaternionRotation = Quaternion.Identity;
            this.Orientation = Quaternion.Identity;
            this.RotationMatrix = Matrix.Identity;
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
            this.Scale = scale;
            this.QuaternionRotation = Quaternion.Identity;
            this.Orientation = Quaternion.Identity;
            this.RotationMatrix = rotationMatrix;
        }
    }
}
