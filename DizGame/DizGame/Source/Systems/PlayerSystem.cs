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
    public class PlayerSystem : IUpdate
    {
        private EntityFactory entFactory;
        
        public PlayerSystem(EntityFactory entFac)
        {
            this.entFactory = entFac;
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
                
                // Temporary
                var rot = transformComp.Rotation;
                rot.X = 0;
                rot.Y = 0;
                rot.Z = 0;

                if (keyComp.GetState("RotateY") == ButtonStates.Hold)
                {
                    rot.Y += 0.001f;
                }
                if (keyComp.GetState("RotateNegY") == ButtonStates.Hold )
                {
                    rot.Y -= 0.001f;
                }
                transformComp.Rotation = rot;
                // /T

                if (mouseComp.GetState("Fire") == ButtonStates.Pressed/* && worldComp.Day % 3 == 0 && worldComp.Day != 0*/)
                {
                    entFactory.CreateBullet("Bullet", transformComp.Position, transformComp.QuaternionRotation, new Vector3(.1f, .1f, .1f), transformComp.Forward, 100, 100);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mouseComp"></param>
        /// <param name="testComp"></param>
        private void UpdateInput(ref MouseComponent mouseComp, ref TestComponent testComp)
        {
            Rectangle clientBounds = GameOne.Instance.Window.ClientBounds;

            int centerX = clientBounds.Width / 2;
            int centerY = clientBounds.Height / 2;
            float deltaX = centerX - mouseComp.X;
            float deltaY = centerY - mouseComp.Y;

            Mouse.SetPosition(centerX, centerY);
            
            testComp.SmoothedMouseMovement.X = deltaX;
            testComp.SmoothedMouseMovement.Y = deltaY;
        }
    }
}
