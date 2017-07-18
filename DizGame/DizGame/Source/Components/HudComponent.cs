using GameEngine.Source.Components;

namespace DizGame.Source.Components
{
    /// <summary>
    /// A component for the HUD
    /// </summary>
    public class HudComponent : IComponent
    {
        /// <summary>
        /// The entity Id of the entity that has the HUD
        /// </summary>
        public int TrackedEntity { get; set; }
    }
}