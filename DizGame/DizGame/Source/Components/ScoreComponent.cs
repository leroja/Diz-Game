using GameEngine.Source.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
