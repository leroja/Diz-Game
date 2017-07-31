using GameEngine.Source.Systems.Interfaces;
using GameEngine.Source.Components;
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
        /// Updates Acceleration, mass, gravity, force, velocity, position and deceleration
        /// Using non Euler for acceleration
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
                += physic.Velocity * dt /* * projectile.TotalTimePassed * projectile.TotalTimePassed*/;

        }
    }
}
