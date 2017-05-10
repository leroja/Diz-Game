using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;

namespace DizGame.Source.AI_States
{
    /// <summary>
    /// A state that makes the AI Chase either another AI or the Player
    /// </summary>
    public class ChaseBehavior : IAiBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ai"></param>
        /// <param name="gameTime"></param>
        public void Update(AIComponent ai, GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
