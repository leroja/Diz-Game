using GameEngine.Source.Systems;
using System.Linq;
using Microsoft.Xna.Framework;
using DizGame.Source.Components;
using GameEngine.Source.Components;
using GameEngine.Source.Enums;
using Microsoft.Xna.Framework.Input;
using GameEngine.Source.Managers;
using DizGame.Source.Factories;
using System;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// A system that updates various things for the players
    /// </summary>
    public class PlayerSystem : IUpdate
    {
        private Rectangle clientBounds;
        private Vector2 center;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlayerSystem(Rectangle windowBounds)
        {
            this.clientBounds = windowBounds;
            center = new Vector2(clientBounds.Width / 2, clientBounds.Height / 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var PlayerEntityIds = ComponentManager.GetAllEntitiesWithComponentType<PlayerComponent>();
            
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

                if (mouseComp.GetState("Fire") == ButtonStates.Pressed && CanFire())
                {
                    if (ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(playerId).CurrentAmmoInMag > 0)
                    {
                        EntityFactory.Instance.CreateBullet("Bullet", transformComp.Position + transformComp.Forward * 17, new Vector3(.1f, .1f, .1f), 1000, 1000, transformComp.Rotation, 10, playerId);
                        ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(playerId).CurrentAmmoInMag--;
                        //var sound = ComponentManager.GetEntityComponent<SoundEffectComponent>(playerId);
                        //sound.SoundEffectsToBePlayed.Add(Tuple.Create("ShotEffect", 0f, 1f));

                        var sound = ComponentManager.Instance.GetEntityComponent<_3DSoundEffectComponent>(playerId);
                        sound.SoundEffectsToBePlayed.Add(Tuple.Create("ShotEffect", 0.5f));
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CanFire()
        {
            var worldTemp = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<WorldComponent>();
            var worldComp = (WorldComponent)worldTemp.Values.First();
            return worldComp.Day % worldComp.ModulusValue == 0 && worldComp.Day != 0;
        }

        /// <summary>
        /// Calculates the mouse delta movement And sets the mouse to be in the middle of the screen
        /// </summary>
        /// <param name="mouseComp"></param>
        /// <returns>  </returns>
        private Vector2 UpdateInput(MouseComponent mouseComp)
        {

            var r = center - mouseComp.CurrentPosition;
            //var r = mouseComp.PreviousPostion - mouseComp.CurrentPosition;

            Mouse.SetPosition((int)center.X, (int)center.Y);

            return r;
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

            while (rotation.X < -MathHelper.PiOver4)
            {
                rotation.X += MathHelper.PiOver4 * 0.01f;
            }
            while (rotation.X > MathHelper.PiOver4)
            {
                rotation.X -= MathHelper.PiOver4 * 0.01f;
            }
            return rotation;
        }
    }
}