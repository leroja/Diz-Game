using GameEngine.Source.Components;

namespace DizGame.Source.Components
{
    /// <summary>
    /// Component for updating score
    /// </summary>
    public class ScoreComponent : IComponent
    {
        /// <summary>
        /// Leaderboard position
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// The players score
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// Name of the entity the scorecomponent is on
        /// </summary>
        public string NameOfScorer { get; set; }
        /// <summary>
        /// Number of Kills the Entity has
        /// </summary>
        public int Kills { get; set; }
        /// <summary>
        /// How many times the entity has hit the enemies
        /// </summary>
        public int Hits { get; set; }
    }
}
