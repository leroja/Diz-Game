using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems.Abstract_classes
{
    public abstract class IRender : ISystem
    {
        public abstract void Draw(GameTime gameTime);

    }
}
