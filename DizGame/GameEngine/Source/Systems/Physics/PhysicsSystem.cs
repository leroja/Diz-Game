using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Systems;

namespace GameEngine.Source.Systems
{
    public class PhysicsSystem : IUpdate
    {
        private float frameCount = 0;
        private float timeSinceLastUpdate = 0;
        private float updateInterval = 1;
        private float framesPerSecond = 0;

        private PhysicsRigidBodySystem rigidBody;
        private PhysicsParticleSystem particle;
        private PhysicsProjectilesSystem projectile;
        private PhysicsRagdollSystem ragDoll;
        private PhysicsSoftSystem soft;
        private PhysicsStaticSystem _static;

        public PhysicsSystem()
        {
            rigidBody = new PhysicsRigidBodySystem();
            particle = new PhysicsParticleSystem();
            projectile = new PhysicsProjectilesSystem();
            ragDoll = new PhysicsRagdollSystem();
            soft = new PhysicsSoftSystem();
            _static = new PhysicsStaticSystem();
        }
        /// <summary>
        /// Updates all the necessary part for the physicsystem
        /// using one loop
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            CountFPS(gameTime);
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<PhysicsComponent>())
            {
                PhysicsComponent physic = ComponentManager.GetEntityComponent<PhysicsComponent>(entityID);

                UpdatePosition(entityID, dt);

                UpdateMaxAcceleration(physic);
                UpdateLinearAcceleration(physic, dt);
                UpdateLinearVelocity(physic, dt);
                UpdateReflection(physic, dt);

                UpdatePhysicComponentByType(entityID, dt);
            }
        }
        /// <summary>
        /// Updates the object position using its velocity * dt
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="dt"></param>
        private void UpdatePosition(int entityID, float dt)
        {
            ComponentManager.GetEntityComponent<TransformComponent>(entityID).Position
                += ComponentManager.GetEntityComponent<PhysicsComponent>(entityID).Velocity * dt;
        }
        /// <summary>
        /// Udates the corresponding object by physictype
        /// </summary>
        /// <param name="entityID"></param>
        /// <param name="dt"></param>
        private void UpdatePhysicComponentByType(int entityID, float dt)
        {
                PhysicsComponent physic = ComponentManager.GetEntityComponent<PhysicsComponent>(entityID);
                switch(physic.PhysicsType)
                {
                    case Enums.PhysicsType.Static:
                        _static.Update(entityID, dt);
                        break;
                    case Enums.PhysicsType.Soft:
                        soft.Update(entityID, dt);
                        break;
                    case Enums.PhysicsType.Rigid:
                        rigidBody.Update(entityID, dt);
                        break;
                    case Enums.PhysicsType.Ragdoll:
                        ragDoll.Update(entityID, dt);
                        break;
                    case Enums.PhysicsType.Projectiles:
                        projectile.Update(entityID, dt);
                        break;
                    case Enums.PhysicsType.Particle:
                        particle.Update(entityID, dt);
                        break;
                    default:
                        break;
            }
        }
        /// <summary>
        /// Updates the objects linear acceleration
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        private void UpdateLinearAcceleration(PhysicsComponent physic, float dt)
        {
            //physic.Acceleration = (physic.Velocity / dt);
            physic.Acceleration = physic.Forces / physic.Mass;
        }
        /// <summary>
        /// Updates the objects linear velocity
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        private void UpdateLinearVelocity(PhysicsComponent physic, float dt)
        {
            physic.Velocity += physic.Acceleration * dt;
        }

        //TODO: UpdateReflection
        /// <summary>
        /// Updates the objects heading depending on collision
        /// </summary>
        /// <param name="physic"></param>
        /// <param name="dt"></param>
        private void UpdateReflection(PhysicsComponent physic, float dt)
        {
            // ratioa = Mb / (Ma + Mb)                  (Mass)
            // ratiob = Ma / (Ma + Mb)                  (Mass)
            // Vr = Va - Vb                             (Va,Vb = Velocity)
            // I = (1+e)*N*(Vr • N) / (1/Ma + 1/Mb)     (e = coefficients) (N = surfaceNormal) (Vr = Velocity) (Ma,Mb = Mass)
            // Va - = I * 1/Ma                          (Velo = I / mass)          
            // Vb + = I * 1/Mb                          (Velo = I / mass)
        }
        /// <summary>
        /// Updates maxacceleration in Meters per second each second 
        /// divided by FPS to give meters per second each frame.
        /// </summary>
        /// <param name="physic"></param>
        private void UpdateMaxAcceleration(PhysicsComponent physic)
        {
                physic.MaxAcceleration = (physic.Forces / physic.Mass) / framesPerSecond;
        }
        /// <summary>
        /// Counts the frames per second
        /// </summary>
        /// <param name="gameTime"></param>
        private void CountFPS(GameTime gameTime)
        {
            frameCount++;
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastUpdate += elapsed;
            if (timeSinceLastUpdate > updateInterval)
            {
                framesPerSecond = frameCount / timeSinceLastUpdate;
                frameCount = 0;
                timeSinceLastUpdate -= updateInterval;
            }
        }
    }
}
