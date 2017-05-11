using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Managers;
using GameEngine.Source.Components;
using GameEngine.Source.Utils;

namespace DizGame.Source.AI_States
{
    /// <summary>
    /// 
    /// </summary>
    public class WanderBehavior : AiBehavior
    {
        private float currentTimeForDir;
        /// <summary>
        /// Constructor
        /// </summary>
        public WanderBehavior()
        {
            currentTimeForDir = 0f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AIComp"></param>
        /// <param name="gameTime"></param>
        public override void Update(AIComponent AIComp, GameTime gameTime)
        {
            var transformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);
            var pos = transformComp.Position;
            
            currentTimeForDir += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentTimeForDir > AIComp.DirectionDuration)
            {
                var rotation = (float)Util.GetRandomNumber(-AIComp.DirectionChangeRoation, AIComp.DirectionChangeRoation);
                rotation /= MathHelper.TwoPi;
                transformComp.Rotation += new Vector3(0, rotation, 0);
                currentTimeForDir = 0f;
                transformComp.Rotation = WrapAngle(transformComp.Rotation);
            }

            var height = GetCurrentHeight(transformComp.Position);
            
            var t = new Vector3(transformComp.Position.X, height, transformComp.Position.Z);
            
            if (t.X >= AIComp.Bounds.Height)
            {
                t -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
            }
            else if (t.X <= 3)
            {
                t -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
            }
            else if (t.Z <= -AIComp.Bounds.Width)
            {
                t -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
            }
            else if (t.Z >= -3)
            {
                t -= transformComp.Forward * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation += new Vector3(0, MathHelper.Pi, 0);
            }
            else
            {
                t += transformComp.Forward * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            transformComp.Position = t;
        }
    }
}
