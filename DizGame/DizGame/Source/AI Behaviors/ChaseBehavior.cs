using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;

namespace DizGame.Source.AI_States
{
    /// <summary>
    /// A state that makes the AI Chase either another AI or the Player
    /// </summary>
    public class ChaseBehavior : AiBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AIComp"></param>
        /// <param name="gameTime"></param>
        public override void Update(AIComponent AIComp, GameTime gameTime)
        {
            

            // todo ta reda på vilken rikting som den närmsta fienden är
            // todo förflytta AI:n i den riktningen. 
            // något mer?
        }
    }
}
