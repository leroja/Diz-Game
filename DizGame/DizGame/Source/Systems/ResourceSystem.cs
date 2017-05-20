using GameEngine.Source.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using DizGame.Source.Components;
using GameEngine.Source.Components;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// System to manage the different kinds of resources placed within the game
    /// world. These are available for players to pick up and gain additional resources.
    /// </summary>
    public class ResourceSystem : IUpdate
    {
        private int maxNumberOfResourcesInPlay;
        private int numberOfRemovedResources;
        private int healthAmmoRatio;

        /// <summary>
        /// Basic constructor for the resource system
        /// </summary>
        public ResourceSystem()
        {
            maxNumberOfResourcesInPlay = 25;
            healthAmmoRatio = 3;
        }

        /// <summary>
        /// Alternate constructor for the resorce system if different settings
        /// are desired.
        /// </summary>
        /// <param name="maxNumberOfResourcesInPlay">this number represent the total number of resources that should
        /// be in play at any given time within the game.</param>
        /// <param name="healthAmmoRatio"> this number represent the ratio for which newly created resources are determained. 
        /// For example if the ratio is 5, every 5th resource will be a health resource and the rest ammo resources.</param>
        public ResourceSystem(int maxNumberOfResourcesInPlay, int healthAmmoRatio)
        {
            this.maxNumberOfResourcesInPlay = maxNumberOfResourcesInPlay;
            this.healthAmmoRatio = healthAmmoRatio;
        }

        /// <summary>
        /// Update function to administrate the removal of old resources and
        /// create additional resources untill there is a maximal number of resources in play
        /// </summary>
        /// <param name="gameTime">takes the time since the last update</param>
        public override void Update(GameTime gameTime)
        {
            numberOfRemovedResources = 0;
            ResourceComponent resource;
            List<int> entitylist = ComponentManager.GetAllEntitiesWithComponentType<ResourceComponent>();
            int worldid = ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>().First();
            WorldComponent world = ComponentManager.GetEntityComponent<WorldComponent>(worldid);

            if (world.Day % world.ModulusValue != 1)
            {

                foreach (int entity in entitylist)
                {
                    resource = ComponentManager.GetEntityComponent<ResourceComponent>(entity);
                    resource.duration -= gameTime.ElapsedGameTime;
                    if (resource.duration.Seconds <= 0)
                        RemoveOldResources(entity);

                }
                if (entitylist.Count + numberOfRemovedResources < maxNumberOfResourcesInPlay)
                    AddNewResources(entitylist.Count + numberOfRemovedResources);
            }
        }

        private void AddNewResources(int currentNumberOfResources)
        {
            int keepTrack = 0;

            List<Vector3> positions = GetMapPositions(maxNumberOfResourcesInPlay - currentNumberOfResources);

            while (currentNumberOfResources != maxNumberOfResourcesInPlay)
            {
                if (keepTrack % healthAmmoRatio == 1)
                {
                    EntityFactory.Instance.ResourceFactory.CreateHealthResource(positions.ElementAt(keepTrack));
                    keepTrack++;
                    currentNumberOfResources++;
                }
                else
                {
                    EntityFactory.Instance.ResourceFactory.CreateAmmoResource(positions.ElementAt(keepTrack));
                    keepTrack++;
                    currentNumberOfResources++;
                }
            }
        }

        private void RemoveOldResources(int entity)
        {
            numberOfRemovedResources++;
            ComponentManager.RemoveEntity(entity);
            ComponentManager.RecycleID(entity);
        }

        private List<Vector3> GetMapPositions(int numberToCreate)
        {
            List<Vector3> positions = new List<Vector3>();

            Random r = new Random();
            int mapWidht;
            int mapHeight;
            List<int> heightList = ComponentManager.GetAllEntitiesWithComponentType<HeightmapComponentTexture>();
            HeightmapComponentTexture heigt = ComponentManager.GetEntityComponent<HeightmapComponentTexture>(heightList[0]);
            mapWidht = heigt.Width;
            mapHeight = heigt.Height;
            for (int i = 0; i < numberToCreate; i++)
            {
                var pot = new Vector3(r.Next(mapWidht - 10), 0, r.Next(mapHeight - 10));
                pot.Y = heigt.HeightMapData[(int)pot.X, (int)pot.Z];
                if (pot.X < 10)
                {
                    pot.X = pot.X + 10;
                }
                if (pot.Z < 10)
                {
                    pot.Z = pot.Z - 10;
                }
                pot.Z = -pot.Z;
                positions.Add(pot);
            }
            return positions;
        }
    }
}
