using GameEngine.Source.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework;
using GameEngine.Source.Enums;
using GameEngine.Source.Systems.AbstractClasses;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// Class for updating the projectiles physic
    /// </summary>
    public class PhysicsProjectilesSystem : IPhysicsTypeSystem
    {
        /// <summary>
        /// Constructor which sets the PhysicType
        /// </summary>
        /// <param name="physicsSystem"></param>
        public PhysicsProjectilesSystem(IPhysics physicsSystem) : base(physicsSystem)
        {
            PhysicsType = PhysicsType.Projectiles;
        }
        /// <summary>
        /// Updates Acceleration, mass, gravity, force, velocity, position and decaeleration
        /// Using non euler for acceleration
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        public override void Update(PhysicsComponent physic, float dt)
        {
            PhysicsSystem.UpdateMass(physic);
            PhysicsSystem.UpdateGravity(physic, dt);
            PhysicsSystem.UpdateVelocity(physic, dt);


            //TODO: ProjectilesSystem
            PhysicsProjectileComponent projectile = ComponentManager.GetEntityComponent<PhysicsProjectileComponent>(physic.ID);
            if (projectile == null)
                ComponentManager.AddComponentToEntity(physic.ID, new PhysicsProjectileComponent());
            projectile = ComponentManager.GetEntityComponent<PhysicsProjectileComponent>(physic.ID);

            UpdateArcPosition(physic, dt);
            //PhysicsSystem.UpdateDeceleration(physic);
        }
        /// <summary>
        /// Updates the projectiles position in an arc
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        private void UpdateArcPosition(PhysicsComponent physic, float dt)
        {
            PhysicsProjectileComponent projectile = ComponentManager.GetEntityComponent<PhysicsProjectileComponent>(physic.ID);
            projectile.TotalTimePassed += (dt / 4.096f);


            ComponentManager.GetEntityComponent<TransformComponent>(physic.ID).Position
                += physic.Velocity * dt /** projectile.TotalTimePassed * projectile.TotalTimePassed*/;

            //Vector3 pos = ComponentManager.GetEntityComponent<TransformComponent>(physic.ID).Position;

            //ComponentManager.GetEntityComponent<TransformComponent>(physic.ID).Position =
            //    new Vector3(pos.X,
            //    pos.Y - 0.5f * physic.Forces.Y * projectile.TotalTimePassed * projectile.TotalTimePassed, pos.Z);
        }
    }
}
