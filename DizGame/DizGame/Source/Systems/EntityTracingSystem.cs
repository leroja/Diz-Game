using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using GameEngine.Source.Systems;
using Microsoft.Xna.Framework;

namespace DizGame.Source.Systems
{
    public class EntityTracingSystem : IUpdate
    {
        private List<int> currentEntityIds;

        public EntityTracingSystem()
        {
            
        }

        public void RecordInitialEntities()
        {
            currentEntityIds = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<IComponent>().Keys.ToList();
        }

        /// <summary>
        /// This function returns the newly created entities (the clients may have created bullets for example).
        /// </summary>
        public List<int> GetNewEntities()
        {
            return new List<int>();
        }

        private void CheckForNewEntities()
        {
            List<int> possibleNewEntIds = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<IComponent>().Keys.ToList();

            //compare possible entids with the current ent ids (that the server has initialised at game start? ).
           
        }

        public override void Update(GameTime gameTime)
        {
            CheckForNewEntities();
        }
    }
}
