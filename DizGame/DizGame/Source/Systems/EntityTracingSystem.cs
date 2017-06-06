using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DizGame.Source.Communication;
using GameEngine.Source.Components;
using GameEngine.Source.Systems;
using Microsoft.Xna.Framework;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// This class is used for tracing new entityId:s that have been added since last update.
    /// </summary>
    public class EntityTracingSystem : IUpdate
    {

        private  List<int> currentEntityIds;

        private List<int> oldEntityIds;

        private static List<int> newEntityIds;


        /// <summary>
        /// Create an entityTracingSystem.
        /// </summary>
        public EntityTracingSystem()
        {
            currentEntityIds = new List<int>();
            newEntityIds = new List<int>();
            oldEntityIds = new List<int>();
        }


        /// <summary>
        /// This function records an initial state of the current entityId:s that the client has right now
        /// - must be called after all entities have been created initially.
        /// </summary>
        public void RecordInitialEntities()
        {
           oldEntityIds = new List<int>(ComponentManager.GetAllCurrentEntityIds());
        }


        /// <summary>
        /// This function needs a ComponentManager that don't reuse old entityIds and also has a reserved range
        /// for the client (e.g 1000000 - 1999999) - otherwise the function will not recognize the created
        /// entitityIds.
        ///This function returns the newly created entities (the clients may have created bullets for example).
        /// Calling this function will place all entities in an old list - so calling this function twice
        /// in a row ( without any new entities created) will return an empty list.
        /// </summary>
        /// <returns>Returns all the new entities that the client has created since last update was called.
        /// </returns>
        public static List<int> GetClientCreatedNewEntities()
        {
            List<int> storeNewEntIds = new List<int>(newEntityIds);

            newEntityIds.Clear();

            storeNewEntIds = storeNewEntIds.Select(x => x).Where(x => x < 9898).ToList(); //9898 should be some range 
                                                            //that the client has reserved constant
                                                            //( e.g. 1000000 - 1999999) read from gamesetting0...

            return storeNewEntIds;
        }


        private void CheckForNewEntities()
        {
            List<int> possibleNewEntIds = ComponentManager.GetAllCurrentEntityIds();

           newEntityIds = possibleNewEntIds.Except(oldEntityIds).ToList();

            oldEntityIds = new List<int>(possibleNewEntIds);
           
        }


        /// <summary>
        /// The update function to be called at periodic interval.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            List<int> clientCreatedEntities;

            CheckForNewEntities();

            clientCreatedEntities = GetClientCreatedNewEntities();

            if (clientCreatedEntities.Count > 0)
                TalkToServer.SendCreatedNewEntities(clientCreatedEntities);
        }
    }
}
