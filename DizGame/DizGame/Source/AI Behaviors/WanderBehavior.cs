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
    public class WanderBehavior : IAiBehavior
    {
        private Random _random;
        private Vector3 wanderDir;
        private float currentTimeForDir;
        /// <summary>
        /// Constructor
        /// </summary>
        public WanderBehavior(Quaternion orienation)
        {
            _random = new Random();
            wanderDir = new Vector3();
            currentTimeForDir = 0f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ai"></param>
        /// <param name="gameTime"></param>
        public void Update(AIComponent ai, GameTime gameTime)
        {
            //var heightmapId = ComponentManager.Instance.GetAllEntitiesWithComponentType<HeightmapComponentTexture>()[0];
            //var heightmapComp = ComponentManager.Instance.GetEntityComponent<HeightmapComponentTexture>(heightmapId);
            var transformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(ai.ID);
            var pos = transformComp.Position;

            transformComp.Rotation = Vector3.Zero;
            currentTimeForDir += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentTimeForDir > ai.DirectionDuration)
            {
                var rotation = (float)Util.GetRandomNumber(-ai.DirectionChangeRoation, ai.DirectionChangeRoation);
                //var rotation = (float)_random.Next(-ai.DirectionChangeRoation, ai.DirectionChangeRoation);
                Console.WriteLine(rotation);
                //rotation /= 360f;
                rotation /= MathHelper.TwoPi;
                Console.WriteLine(rotation);
                transformComp.Rotation = new Vector3(0, rotation, 0);
                currentTimeForDir = 0f;
            }

            var height = GetCurrentHeight(transformComp.Position);
            
            var t = new Vector3(transformComp.Position.X, height, transformComp.Position.Z);



            t += transformComp.Forward * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (t.X >= ai.Bounds.Height)
            {
                t -= transformComp.Right * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //transformComp.Rotation = new Vector3(0, 180, 0);
                transformComp.Rotation = new Vector3(0, MathHelper.Pi, 0);
            }
            if (t.X <= 3)
            {
                t -= transformComp.Right * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //transformComp.Rotation = new Vector3(0, 180, 0);
                transformComp.Rotation = new Vector3(0, MathHelper.Pi, 0);
            }
            //else
            //{
            //    t += transformComp.Right * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //}
            if (t.Z <= -ai.Bounds.Width)
            {
                t -= transformComp.Forward * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //transformComp.Rotation = new Vector3(0, 180, 0);
                transformComp.Rotation = new Vector3(0, MathHelper.Pi, 0);
            }
            if (t.Z >= -3)
            {
                t -= transformComp.Forward * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //transformComp.Rotation = new Vector3(0, 180, 0);
                transformComp.Rotation = new Vector3(0, MathHelper.Pi, 0);
            }
            //else
            //{
            //    t += transformComp.Forward * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //}
            //t += transformComp.Forward * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            transformComp.Position = t;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">
        /// The current Position of the AI
        /// </param>
        /// <returns></returns>
        private float GetCurrentHeight(Vector3 position)
        {
            List<int> temp = ComponentManager.Instance.GetAllEntitiesWithComponentType<HeightmapComponentTexture>();
            if (temp.Count != 0)
            {
                HeightmapComponentTexture hmap = ComponentManager.Instance.GetEntityComponent<HeightmapComponentTexture>(temp.First());

                int roundX = (int)Math.Round(position.X); int roundY = (int)Math.Round(position.Z);
                if (roundX >= hmap.Width - 1 || roundY >= hmap.Height - 1)
                {
                    return 0;
                }
                if (roundY <= 0 && roundX >= 0)
                    return hmap.HeightMapData[roundX, -roundY];
            }
            return 0;
        }



    }
}
