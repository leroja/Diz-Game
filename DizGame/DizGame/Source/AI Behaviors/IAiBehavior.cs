using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DizGame.Source.AI_States
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAiBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AIComp"></param>
        /// <param name="gameTime"></param>
        void Update(AIComponent AIComp, GameTime gameTime);


    }
}
