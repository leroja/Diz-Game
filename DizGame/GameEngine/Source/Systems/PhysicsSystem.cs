using GameEngine.Source.Systems.Abstract_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.Physics.Type;

namespace GameEngine.Source.Systems
{
    public class PhysicsSystem : IUpdate
    {
        private float frameCount = 0;
        private float timeSinceLastUpdate = 0;
        private float updateInterval = 1;
        private float framesPerSecond = 0;

        private RigidBody rigidBody;
        private Particle particle;
        private Projectiles projectile;
        private Ragdoll ragDoll;
        private Soft soft;
        private Static _static;

        public PhysicsSystem()
        {
            rigidBody = new RigidBody();
            particle = new Particle();
            projectile = new Projectiles();
            ragDoll = new Ragdoll();
            soft = new Soft();
            _static = new Static();
        }
        public override void Update(GameTime gameTime)
        {
            CountFPS(gameTime);
            UpdateMaxAcceleration(gameTime);
            UpdateLinearAcceleration(gameTime);
            UpdateLinearVelocity(gameTime);
            UpdateReflection(gameTime);

            UpdatePhysicComponentByType(gameTime);
        }
        private void UpdatePhysicComponentByType(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<PhysicsComponent>())
            {
                PhysicsComponent physic = ComponentManager.GetEntityComponent<PhysicsComponent>(entityID);
                switch(physic.PhysicsType)
                {
                    case Enums.PhysicsType.Static:
                        _static.Update(physic, dt);
                        break;
                    case Enums.PhysicsType.Soft:
                        soft.Update(physic, dt);
                        break;
                    case Enums.PhysicsType.Rigid:
                        rigidBody.Update(physic, dt);
                        break;
                    case Enums.PhysicsType.Ragdoll:
                        ragDoll.Update(physic, dt);
                        break;
                    case Enums.PhysicsType.Projectiles:
                        projectile.Update(physic, dt);
                        break;
                    case Enums.PhysicsType.Particle:
                        particle.Update(physic, dt);
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Updates the objects linear acceleration
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateLinearAcceleration(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<PhysicsComponent>())
            {
                PhysicsComponent physic = ComponentManager.GetEntityComponent<PhysicsComponent>(entityID);
                //physic.Acceleration = (physic.Velocity / dt);
                physic.Acceleration = physic.Forces / physic.Mass;
            }
        }
        /// <summary>
        /// Updates the objects linear velocity
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateLinearVelocity(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<PhysicsComponent>())
            {
                PhysicsComponent physic = ComponentManager.GetEntityComponent<PhysicsComponent>(entityID);
                physic.Velocity += physic.Acceleration * dt; 
            }
        }

        //TODO: UpdateReflection
        /// <summary>
        /// Updates the objects heading depending on collision
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateReflection(GameTime gameTime)
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
        private void UpdateMaxAcceleration(GameTime gameTime)
        {
            foreach(int entityID in ComponentManager.GetAllEntitiesWithComponentType<PhysicsComponent>())
            {
                PhysicsComponent physics = ComponentManager.GetEntityComponent<PhysicsComponent>(entityID);
                Vector3 temp = Vector3.Zero;
                physics.MaxAcceleration = (physics.Forces / physics.Mass) / framesPerSecond;
            }
        }
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
