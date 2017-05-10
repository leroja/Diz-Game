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

        private  List<int> currentEntityIds;

        private static List<int> newEntityIds;

        public EntityTracingSystem()
        {
            currentEntityIds = new List<int>();

            newEntityIds = new List<int>();
        }


        /// <summary>
        /// This funtion records an initial state of the current entityId:s that the client has right now
        /// - must be called after all entities have been created initially.
        /// </summary>
        public void RecordInitialEntities()
        {
            currentEntityIds = ComponentManager.GetAllCurrentEntityIds();
        }


        /// <summary>
        ///This function returns the newly created entities (the clients may have created bullets for example).
        /// Calling this function will place all entities in an old list - so calling this function twice
        /// in a row ( without any new entities created) will return an empty list.
        /// </summary>
        /// <returns>Returns all the new entities since last update was called.</returns>
        public static List<int> GetNewEntities()
        {
            List<int> storeNewEntIds = new List<int>(newEntityIds);

            newEntityIds.Clear();

            return storeNewEntIds;
        }


        private void CheckForNewEntities()
        {
            List<int> possibleNewEntIds = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<IComponent>().Keys.ToList();

           newEntityIds = possibleNewEntIds.Except(currentEntityIds).ToList();
           
        }


        public override void Update(GameTime gameTime)
        {
            CheckForNewEntities();
        }
    }
}
