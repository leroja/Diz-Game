using GameEngine.Source.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using Microsoft.Xna.Framework.Input;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// 
    /// </summary>
    public class PlayerSystem : IUpdate
    {
        private EntityFactory entFactory;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public PlayerSystem()
        {
            this.entFactory = EntityFactory.Instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var PlayerEntityIds = ComponentManager.GetAllEntitiesWithComponentType<PlayerComponent>();
            var worldTemp = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<WorldComponent>();
            var worldComp = (WorldComponent)worldTemp.Values.First();

            foreach (var playerId in PlayerEntityIds)
            {
                var playerComp = ComponentManager.GetEntityComponent<PlayerComponent>(playerId);
                var mouseComp = ComponentManager.GetEntityComponent<MouseComponent>(playerId);
                var transformComp = ComponentManager.GetEntityComponent<TransformComponent>(playerId);
                var testComp = ComponentManager.GetEntityComponent<TestComponent>(playerId);
                var keyComp = ComponentManager.GetEntityComponent<KeyBoardComponent>(playerId);
                
                var m = UpdateInput(mouseComp);

                transformComp.Rotation += new Vector3(0, m.X, 0) * mouseComp.MouseSensitivity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                transformComp.Rotation = WrapAngle(transformComp.Rotation);

                if (mouseComp.GetState("Fire") == ButtonStates.Pressed && worldComp.Day % 2 == 0 && worldComp.Day != 0)
                {
                    entFactory.CreateBullet("Bullet", transformComp.Position, transformComp.QuaternionRotation, new Vector3(.1f, .1f, .1f), transformComp.Forward, 100, 200, transformComp.Rotation);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mouseComp"></param>
        private Vector2 UpdateInput(MouseComponent mouseComp)
        {
            Rectangle clientBounds = GameOne.Instance.Window.ClientBounds;

            int centerX = clientBounds.Width / 2;
            int centerY = clientBounds.Height / 2;
            float deltaX = centerX - mouseComp.X;
            float deltaY = centerY - mouseComp.Y;

            Mouse.SetPosition(centerX, centerY);

            return new Vector2(deltaX, deltaY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        private Vector3 WrapAngle(Vector3 rotation)
        {
            while (rotation.Y < -MathHelper.Pi)
            {
                rotation.Y += MathHelper.TwoPi;
            }
            while (rotation.Y > MathHelper.Pi)
            {
                rotation.Y -= MathHelper.TwoPi;
            }
            return rotation;
        }
    }
}
