using GameEngine.Source.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Components
{
    public class BulletComponent : IComponent
    {
        public float MaxRange { get; set; }
        public Vector3 StartPos { get; set; }
        public float Damage { get; set; }
    }
}
