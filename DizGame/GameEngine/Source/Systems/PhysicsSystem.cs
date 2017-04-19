using GameEngine.Source.Systems.Abstract_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;

namespace GameEngine.Source.Systems
{
    public class PhysicsSystem : IUpdate
    {
        private float frameCount = 0;
        private float timeSinceLastUpdate = 0;
        private float updateInterval = 1;
        private float framesPerSecond = 0;
        public override void Update(GameTime gameTime)
        {
            CountFPS(gameTime);
            UpdateMaxAcceleration(gameTime);
            UpdateAcceleration(gameTime);
            UpdateVelocity(gameTime);
            UpdateReflection(gameTime);


        }
        /// <summary>
        /// Updates the objects acceleration
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateAcceleration(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<PhysicsComponent>())
            {
                PhysicsComponent physic = ComponentManager.GetEntityComponent<PhysicsComponent>(entityID);
                physic.Acceleration = (physic.Velocity / dt);
            }
        }
        /// <summary>
        /// Updates the objects velocity
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateVelocity(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (int entityID in ComponentManager.GetAllEntitiesWithComponentType<PhysicsComponent>())
            {
                PhysicsComponent physic = ComponentManager.GetEntityComponent<PhysicsComponent>(entityID);
                physic.Velocity += physic.Velocity + (physic.Acceleration * dt); 
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
                temp.X = (physics.Forces.X / physics.Mass) / framesPerSecond;
                temp.Y = (physics.Forces.Y / physics.Mass) / framesPerSecond;
                temp.Z = (physics.Forces.Z / physics.Mass) / framesPerSecond;
                physics.MaxAcceleration = temp;
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
