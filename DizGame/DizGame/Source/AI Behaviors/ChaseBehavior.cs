using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Managers;
using Microsoft.Xna.Framework.Input;

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
            var transformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);
            var pos = transformComp.Position;
            
            var height = GetCurrentHeight(pos);
            var t = new Vector3(transformComp.Position.X, height, transformComp.Position.Z) + transformComp.Forward * 5 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            var rotation = GetRotationToClosestEnenmy(AIComp);
            
            transformComp.Rotation = rotation;
            
            transformComp.Position = t;
        }
    }
}
