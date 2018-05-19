using GameEngine.Source.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.Builders
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityBuilder
    {
        private int entityID;

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityBuilder()
        {
            entityID = ComponentManager.Instance.CreateID();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Build()
        {
            // add components to compman
        }
    }
}
