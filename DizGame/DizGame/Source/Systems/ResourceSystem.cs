using GameEngine.Source.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DizGame.Source.Components;
using GameEngine.Source.Components;
using static DizGame.Source.Components.ResourceComponent;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// System to manage the different kinds of resources placed within the game
    /// world. These are available for players to pick up and gaign additional resources.
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
            maxNumberOfResourcesInPlay = 50;
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

            if(world.Day % world.ModulusValue != 0)
            {

                foreach (int entity in entitylist)
                {
                    resource = ComponentManager.GetEntityComponent<ResourceComponent>(entity);
                    resource.durration -= gameTime.ElapsedGameTime;
                    if (resource.durration.Seconds <= 0)
                        RemoveOldResources(entity);

                }
                if (entitylist.Count + numberOfRemovedResources < 50)
                    AddNewResources(entitylist.Count + numberOfRemovedResources);
            }

            
        }

        private void AddNewResources(int currentNumberOfResources)
        {
            int keepTrack = 0;
            Vector3 scale;
            ResourceComponent resource;
            List<Vector3> positions = GetMapPositions(maxNumberOfResourcesInPlay - currentNumberOfResources);

            while (currentNumberOfResources != 50)
            {
                int newEntityId = ComponentManager.CreateID();
                if(keepTrack % healthAmmoRatio == 0)
                {
                    resource = new ResourceComponent(ResourceType.Health);
                    //adjust the scales differently for the models if needed
                    scale = new Vector3(1, 1, 1);
                    //Also create the model component for the entity + check visabillity
                }
                else
                {
                    resource = new ResourceComponent(ResourceType.Ammo);
                    //adjust the scales differently for the models if needed
                    scale = new Vector3(1, 1, 1);
                    //Also create the model component for the entity + check visabillity
                }
                    

                TransformComponent cmp = new TransformComponent(positions.ElementAt(keepTrack), scale);
                //Make the model rotate a little so that it's more viable to the player
                cmp.Rotation = new Vector3(0.1f, 0, 0);

                List<IComponent> resourceCompList = new List<IComponent>
                {
                    resource,
                    cmp,
                    // add the model here
                };

                ComponentManager.AddAllComponents(newEntityId, resourceCompList);
                keepTrack++;
                
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
