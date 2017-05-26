using GameEngine.Source.Components;

namespace DizGame.Source.Components
{
    /// <summary>
    /// A component that contains the health of entities
    /// </summary>
    public class HealthComponent : IComponent
    {
        /// <summary>
        /// The health of the entity
        /// </summary>
        public float Health { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float MaxHealth = 100;

        /// <summary>
        /// 
        /// </summary>
        public float HealthOnPickup { get; set; }

        /// <summary>
        /// Constructor which sets attributes to default values.
        /// </summary>
        public HealthComponent()
        {
            Health = MaxHealth;
            //defult pickup value
            HealthOnPickup = 20;
        }
    }
}
