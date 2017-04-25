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

        public Vector3 Position { get; set; }

        public Vector3 Rotation { get; set; }

        public Quaternion QuaternionRotation { get; set; }
        public Quaternion Orientation { get; set; }
        public Vector3 Dirrection { get; set; }
        public Vector3 Forward { get; set; }
        public Vector3 Up { get; set; }
        public Vector3 Right { get; set; }

        public Vector3 Scale { get; set; }

        public Matrix ObjectMatrix { get; set; }

        public Matrix RotationMatrix { get; set; }

        public TransformComponent(Vector3 position, Vector3 scale)
        {
            this.Position = position;
            this.Scale = scale;
            Dirrection = Vector3.Zero;
            this.QuaternionRotation = Quaternion.Identity;
            this.Orientation = Quaternion.Identity;
            this.RotationMatrix = Matrix.Identity;
        }

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
