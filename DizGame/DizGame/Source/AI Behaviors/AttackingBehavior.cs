﻿using System.Linq;
using DizGame.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Managers;
using GameEngine.Source.Components;
using DizGame.Source.Systems;
using DizGame.Source.Factories;
using System;
using GameEngine.Source.Utils;

namespace DizGame.Source.AI_Behaviors
{
    /// <summary>
    /// A Behavior that makes the AI shoot at the closest enemy
    /// </summary>
    public class AttackingBehavior : AiBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AIComp"></param>
        /// <param name="gameTime"></param>
        public override void Update(AIComponent AIComp, GameTime gameTime)
        {
            var worldTemp = ComponentManager.Instance.GetAllEntitiesAndComponentsWithComponentType<WorldComponent>();
            var worldComp = (WorldComponent)worldTemp.Values.First();
            var transformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(AIComp.ID);
            var physComp = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(AIComp.ID);
            physComp.Velocity = Vector3.Zero;
            physComp.Acceleration = Vector3.Zero;

            Time -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            DesiredRotation = GetRotationTo(AIComp, ComponentManager.Instance.GetEntityComponent<TransformComponent>(ClosestEnemy).Position).Y;

            transformComp.Rotation = new Vector3(0, TurnToFace(DesiredRotation, transformComp.Rotation.Y,
                AIComp.TurningSpeed * 4 * (float)gameTime.ElapsedGameTime.TotalSeconds), 0);

            transformComp.Position = new Vector3(transformComp.Position.X, MovingSystem.GetHeight(transformComp.Position), transformComp.Position.Z);

            if (worldComp.Day % worldComp.ModulusValue == 0 && worldComp.Day != 0 && Time < 0)
            {
                if (ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(AIComp.ID).CurrentAmmoInMag > 0 &&
                    DesiredRotation == transformComp.Rotation.Y)
                {
                    var rot = new Vector3(GetRotationForAimingAtEnemy(AIComp), 0, 0);

                    rot += GetSpread();

                    EntityFactory.Instance.CreateBullet("Bullet", transformComp.Position + transformComp.Forward * 7,
                        new Vector3(.1f, .1f, .1f), 1000, 1000, transformComp.Rotation + rot,
                        AIComp.DamagePerShot, AIComp.ID);

                    ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(AIComp.ID).CurrentAmmoInMag--;
                    Time = AIComp.ShootingCooldown;
                    var sound = ComponentManager.Instance.GetEntityComponent<_3DSoundEffectComponent>(AIComp.ID);
                    sound.SoundEffectsToBePlayed.Add(Tuple.Create("ShotEffect", 1f));
                }
            }

            BehaviorStuff(AIComp, transformComp, worldComp);
        }

        /// <summary>
        /// Check whether the AI should change behavior
        /// If it should then the method changes the behavior
        /// </summary>
        /// <param name="AIComp"> The AI component of the AI </param>
        /// <param name="transformComp"> The transform component of the AI </param>
        /// <param name="worldComp"> The world component </param>
        private void BehaviorStuff(AIComponent AIComp, TransformComponent transformComp, WorldComponent worldComp)
        {
            var ammoComp = ComponentManager.Instance.GetEntityComponent<AmmunitionComponent>(AIComp.ID);
            var healthComp = ComponentManager.Instance.GetEntityComponent<HealthComponent>(AIComp.ID);

            if ((ammoComp.CurrentAmmoInMag == 0 && ammoComp.AmmountOfActiveMagazines == 0) || (healthComp.Health / healthComp.MaxHealth) <= 0.5)// the health threshold can maybe be defined in/by the AIComp, to allow different behaviors of the AI
            {
                if (!AIComp.HaveBehavior("Patrol"))
                {
                    AIComp.ChangeBehavior("Evade", transformComp.Rotation);
                }
            }

            if (worldComp.Day % worldComp.ModulusValue == 0 && AIComp.AttackingDistance + AIComp.Hysteria < AIComp.CurrentBehaivior.DistanceToClosestEnemy)
            {
                AIComp.ChangeBehavior("Chase", transformComp.Rotation);
            }

            if (worldComp.Day % worldComp.ModulusValue != 0)
            {
                if (AIComp.HaveBehavior("Patrol"))
                {
                    AIComp.ChangeBehavior("Patrol", transformComp.Rotation);
                }
                else
                {
                    AIComp.ChangeBehavior("Evade", transformComp.Rotation);
                }
            }
        }

        /// <summary>
        /// Returns a new Vector3 with some small random numbers
        /// </summary>
        /// <returns></returns>
        private Vector3 GetSpread()
        {
            float min = -MathHelper.PiOver4 / 20;
            float max = MathHelper.PiOver4 / 20;
            float x = (float)Util.GetRandomNumber(min, max);
            float y = (float)Util.GetRandomNumber(min, max);
            float z = (float)Util.GetRandomNumber(min, max);
            return new Vector3(x, y, z);
        }


        /// <summary>
        /// Override of object.ToString
        /// Returns the name of the behavior
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Attacking";
        }
    }
}