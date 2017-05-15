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
using GameEngine.Source.Managers;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// A system that updates various things for the players
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
                var keyComp = ComponentManager.GetEntityComponent<KeyBoardComponent>(playerId);
                
                if (keyComp.GetState("Mute") == ButtonStates.Pressed)
                {
                    if (AudioManager.Instance.IsMuted())
                        AudioManager.Instance.GlobalUnMute();
                    else
                        AudioManager.Instance.GlobalMute();
                }
                
                var m = UpdateInput(mouseComp);

                transformComp.Rotation += new Vector3(-m.Y, m.X, 0) * mouseComp.MouseSensitivity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                transformComp.Rotation = WrapAngle(transformComp.Rotation);

                if (mouseComp.GetState("Fire") == ButtonStates.Pressed && worldComp.Day % 2 == 0 && worldComp.Day != 0)
                {
                    entFactory.CreateBullet("Bullet", transformComp.Position, new Vector3(.1f, .1f, .1f), transformComp.Forward, 100, 200, transformComp.Rotation);
                    AudioManager.Instance.PlaySoundEffect("ShotEffect", 1f, 1f);
                }
            }
        }

        /// <summary>
        /// Calculates the mouse delta movement And sets the mouse to be in the middle of the screen
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
        /// A function for keeping the rotation to be between -2PI and +2PI
        /// </summary>
        /// <param name="rotation"> The current rotation </param>
        /// <returns> A rotation between -2PI and +2PI </returns>
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
