using GameEngine.Source.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using DizGame.Source.Components;
using GameEngine.Source.Components;
using DizGame.Source.Factories;
using System.Threading.Tasks;
using DizGame.Source.Random_Stuff;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// System to manage the different kinds of resources placed within the game world. 
    /// These are available for players to pick up and gain additional resources.
    /// </summary>
    public class ResourceSystem : IUpdate
    {
        private int maxNumberOfResourcesInPlay;
        private int numberOfRemovedResources;
        private int healthAmmoRatio;
        private Border border;

        /// <summary>
        /// Basic constructor for the resource system
        /// </summary>
        public ResourceSystem(Border border)
        {
            maxNumberOfResourcesInPlay = 30;
            healthAmmoRatio = 3;
            this.border = border;
        }

        /// <summary>
        /// Alternate constructor for the resource system if different settings
        /// are desired.
        /// </summary>
        /// <param name="maxNumberOfResourcesInPlay">this number represent the total number of resources that should
        /// be in play at any given time within the game.</param>
        /// <param name="healthAmmoRatio"> this number represent the ratio for which newly created resources are determined. 
        /// For example if the ratio is 5, every 5th resource will be a health resource and the rest ammo resources.</param>
        /// <param name="border"> The bounds in which the resources can spawn </param>
        public ResourceSystem(int maxNumberOfResourcesInPlay, int healthAmmoRatio, Border border)
        {
            if (healthAmmoRatio == 0)
                healthAmmoRatio = 2;
            this.maxNumberOfResourcesInPlay = maxNumberOfResourcesInPlay;
            this.healthAmmoRatio = healthAmmoRatio;
            this.border = border;
        }

        /// <summary>
        /// Update function to administrate the removal of old resources and
        /// create additional resources until there is a maximal number of resources in play
        /// </summary>
        /// <param name="gameTime">takes the time since the last update</param>
        public override void Update(GameTime gameTime)
        {
            numberOfRemovedResources = 0;
            ResourceComponent resource;
            List<int> entitylist = ComponentManager.GetAllEntitiesWithComponentType<ResourceComponent>();
            int worldid = ComponentManager.GetAllEntitiesWithComponentType<WorldComponent>().First();
            WorldComponent world = ComponentManager.GetEntityComponent<WorldComponent>(worldid);

            Parallel.ForEach(entitylist, id =>
            {
                var transComp = ComponentManager.GetEntityComponent<TransformComponent>(id);
                transComp.Rotation += new Vector3(0, 0.7f * (float)gameTime.ElapsedGameTime.TotalSeconds, 0f);
            });

            if (world.Day % world.ModulusValue != 0)
            {
                Parallel.ForEach(entitylist, entity =>
                {
                    resource = ComponentManager.GetEntityComponent<ResourceComponent>(entity);
                    resource.duration -= gameTime.ElapsedGameTime;
                    if (resource.duration.Seconds <= 0)
                        RemoveOldResources(entity);
                });
                if (entitylist.Count + numberOfRemovedResources < maxNumberOfResourcesInPlay)
                    AddNewResources(entitylist.Count + numberOfRemovedResources);
            }
        }

        private void AddNewResources(int currentNumberOfResources)
        {
            var Health = 0;
            foreach (var item in ComponentManager.GetAllEntitiesWithComponentType<ResourceComponent>())
            {
                var rComp = ComponentManager.GetEntityComponent<ResourceComponent>(item);
                if (rComp.thisType == ResourceType.Health)
                {
                    Health++;
                }
            }

            var healthTarget = maxNumberOfResourcesInPlay / healthAmmoRatio;

            int keepTrack = 0;

            List<Vector3> positions = GetMapPositions(maxNumberOfResourcesInPlay - currentNumberOfResources);

            while (currentNumberOfResources != maxNumberOfResourcesInPlay)
            {
                if (Health < healthTarget)
                {
                    EntityFactory.Instance.ResourceFactory.CreateHealthResource(positions.ElementAt(keepTrack));
                    Health++;
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
            List<int> heightList = ComponentManager.GetAllEntitiesWithComponentType<HeightmapComponent>();
            HeightmapComponent heigt = ComponentManager.GetEntityComponent<HeightmapComponent>(heightList[0]);
            for (int i = 0; i < numberToCreate; i++)
            {
                var pot = new Vector3(r.Next((int)border.LowX, (int)border.HighX), 0, r.Next(Math.Abs((int)border.LowZ), Math.Abs((int)border.HighZ)));
                pot.Y = heigt.HeightMapData[(int)pot.X, (int)pot.Z];
                pot.Z = -pot.Z;
                positions.Add(pot);
            }
            return positions;
        }
    }
}