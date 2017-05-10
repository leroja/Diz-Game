using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Managers;
using GameEngine.Source.Components;

namespace DizGame.Source.AI_States
{
    /// <summary>
    /// 
    /// </summary>
    public class EvadeBehavior : AiBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AIComp"></param>
        /// <param name="gameTime"></param>
        public override void Update(AIComponent AIComp, GameTime gameTime)
        {
            

            
            // todo ta reda på vilken rikting som den närmsta fienden är
            // todo vända AI:n i motsat riktning
            // todo förflytta AI:n i den riktningen. 
            // något mer?
        }
    }
}
