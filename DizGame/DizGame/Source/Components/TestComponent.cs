using GameEngine.Source.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Components
{
    /// <summary>
    /// Laggt till för test styrning av kameran med mus
    /// villl inte sitta och pilla i andra komponenter med risk att paja något
    /// </summary>
    public class TestComponent : IComponent
    {
        public Vector2 SmoothedMouseMovement;
        public float RotationSpeed { get; set; }
        public float AccumHeadingDegrees { get; set; }
        public float AccumPitchDegrees { get; set; }
        public TestComponent()
        {
            SmoothedMouseMovement = new Vector2();
            RotationSpeed = 0.2f;
            AccumHeadingDegrees = 0.0f;
            AccumPitchDegrees = 0.0f;
        }
    }
}
