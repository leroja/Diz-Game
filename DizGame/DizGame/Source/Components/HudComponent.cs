using GameEngine.Source.Components;

namespace DizGame.Source.Components
{
    /// <summary>
    /// A component for the hud
    /// </summary>
    public class HudComponent : IComponent
    {
        /// <summary>
        /// The entity Id of the entity that has the hud
        /// </summary>
        public int TrackedEntity { get; set; }
    }
}
