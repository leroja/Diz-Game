using GameEngine.Source.Components;
using GameEngine.Source.Utils;
using System;

namespace DizGame.Source.Components
{
    /// <summary>
    /// enum that describes what kind of resource the entity
    /// should represent. Other resource types could be added 
    /// if needed.
    /// </summary>
    public enum ResourceType : int
    {
        /// <summary>
        /// Health is the resource type for which the health component should be updated for 
        /// the player when picked up.
        /// </summary>
        Health,
        /// <summary>
        /// Ammo is the resource type for which the ammunition component for the player
        /// should be manipulated when picked up.
        /// </summary>
        Ammo
    };

    /// <summary>
    /// Class to describe a resource within the game that can be picked up by
    /// players and are placed randomly within the world. Also contains information
    /// regarding which kind of resource the entity represents.
    /// </summary>
    public class ResourceComponent : IComponent
    {
        #region Properties

        /// <summary>
        /// The type of resource
        /// </summary>
        public ResourceType thisType;
        /// <summary>
        /// Duration is a timespan which describes the amount of time 
        /// the component is placed within the world, which also could be 
        /// a unlimited amount of time. This time is described in seconds.
        /// </summary>
        public TimeSpan duration;
        #endregion

        /// <summary>
        /// Basic constructor for the ResourceComponent class
        /// </summary>
        /// <param name="type"> takes the type of the resource as a parameter, 
        /// in order to know the difference between different resources</param>
        public ResourceComponent(ResourceType type)
        {
            thisType = type;
            duration = TimeSpan.FromSeconds(Util.GetRandomNumber(25, 35));
        }

        /// <summary>
        /// Alternative constructor for the ResourceComponent
        /// </summary>
        /// <param name="type">Takes the type of the kind of resource, in order to 
        /// tell the difference between resources</param>
        /// <param name="duration">Duration should represent the time for which the 
        /// resource should remain in play if not picked up by a player.</param>
        public ResourceComponent(ResourceType type, TimeSpan duration)
        {
            thisType = type;
            this.duration = duration;
        }
    }
}