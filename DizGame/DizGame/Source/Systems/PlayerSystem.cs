using GameEngine.Source.Systems;
using System.Linq;
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
                var physicComp = ComponentManager.GetEntityComponent<PhysicsComponent>(playerId);

                if (keyComp.GetState("Mute") == ButtonStates.Pressed)
                {
                    if (AudioManager.Instance.IsMuted())
                        AudioManager.Instance.GlobalUnMute();
                    else
                        AudioManager.Instance.GlobalMute();
                }

                var m = UpdateInput(mouseComp);

                transformComp.Rotation += new Vector3(m.Y, m.X, 0) * mouseComp.MouseSensitivity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                transformComp.Rotation = WrapAngle(transformComp.Rotation);

                if (mouseComp.GetState("Fire") == ButtonStates.Pressed && worldComp.Day % worldComp.ModulusValue == 0 && worldComp.Day != 0)
                {
                    entFactory.CreateBullet("Bullet", transformComp.Position + transformComp.Forward * 7, new Vector3(.1f, .1f, .1f), 100, 1000, transformComp.Rotation, 10,playerId);
                    AudioManager.Instance.PlaySoundEffect("ShotEffect", 1f, 1f);
                }
            }
        }

        /// <summary>
        /// Calculates the mouse delta movement And sets the mouse to be in the middle of the screen
        /// </summary>
        /// <param name="mouseComp"></param>
        /// <returns>  </returns>
        private Vector2 UpdateInput(MouseComponent mouseComp)
        {
            if (GameOne.Instance.Window != null)
            {
                Rectangle clientBounds = GameOne.Instance.Window.ClientBounds;

                int centerX = clientBounds.Width / 2;
                int centerY = clientBounds.Height / 2;
                float deltaX = centerX - mouseComp.X;
                float deltaY = centerY - mouseComp.Y;

                Mouse.SetPosition(centerX, centerY);

                return new Vector2(deltaX, deltaY);
            }
            return Vector2.Zero;
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

            while (rotation.X < -MathHelper.PiOver4 * 0.5f)
            {
                rotation.X += MathHelper.PiOver4 * 0.01f;
            }
            while (rotation.X > MathHelper.PiOver4 * 0.5f)
            {
                rotation.X -= MathHelper.PiOver4 * 0.01f;
            }
            return rotation;
        }
    }
}
