using GameEngine.Source.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class HealthComponent : IComponent
    {
        /// <summary>
        /// the health of the entity
        /// </summary>
        public float Health { get; set; }

        /// <summary>
        /// Constructor which sets attributes to default values.
        /// </summary>
        public HealthComponent()
        {
            Health = 100;
        }
    }
}
