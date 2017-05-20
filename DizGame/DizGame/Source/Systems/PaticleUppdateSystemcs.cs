using GameEngine.Source.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using GameEngine.Source.RandomStuff;

namespace DizGame.Source.Systems
{
    /// <summary>
    /// Updates particles
    /// </summary>
    public class PaticleUpdateSystem : IUpdate
    {
        /// <summary>
        /// Updates Particles of a certain type
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var a  = ComponentManager.GetAllEntitiesWithComponentType<ParticleEmiterComponent>();
            foreach (var id in a)
            {
                var emiter = ComponentManager.GetEntityComponent<ParticleEmiterComponent>(id);

                emiter.EmiterLife -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(emiter.EmiterLife < 0)
                {
                    ComponentManager.RemoveEntity(ComponentManager.GetEntityIDByComponent<ParticleEmiterComponent>(emiter));
                    emiter = null;
                }
                if (emiter != null)
                {
                    AddParticle(id,gameTime);
                }
            }
        }

        /// <summary>
        /// Adds particle to ParticelEmitterComponent
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time"></param>
        public void AddParticle(int id, GameTime time)
        {
            Vector3 pos = new Vector3(1, 40, 1);
            ParticleEmiterComponent emiter = ComponentManager.GetEntityComponent<ParticleEmiterComponent>(id);
            TransformComponent tran = ComponentManager.GetEntityComponent<TransformComponent>(id);

            if (emiter.NumberOfActiveParticles + 4 == emiter.NumberOfParticles * 4)
                return;

            OffsetIndex(emiter);
            emiter.NumberOfActiveParticles += 4;

            float startTime = emiter.LifeTime;
            var pot = SetRandomPos(new Vector3(10 , 0, 10), new Vector3(-10, 0, -10));
            
            for (int i = 0; i < 4; i++)
            {
                emiter.Particles[emiter.StartIndex + i].StartPosition = pos;
                emiter.Particles[emiter.StartIndex + i].Direction = emiter.Direction;
                emiter.Particles[emiter.StartIndex + i].Speed = emiter.Speed;
                emiter.Particles[emiter.StartIndex + i].StartTime = startTime;
            }
        }

        /// <summary>
        /// Sets a random position inside of bounds
        /// </summary>
        /// <param name="min"> Min positions of all position X,Y,Z in a vector3</param>
        /// <param name="max"> Max positions of all position X,Y,Z in a vector3</param>
        /// <returns></returns>
        private Vector3 SetRandomPos(Vector3 min, Vector3 max)
        {
            Random r = new Random();
            return new Vector3(
                min.X + (float)r.NextDouble() * (max.X - min.X),
                min.Y + (float)r.NextDouble() * (max.Y - min.Y),
                min.Z + (float)r.NextDouble() * (max.Z - min.Z));
        }

        /// <summary>
        /// Sets index for Particle
        /// </summary>
        /// <param name="emiter"></param>
        void OffsetIndex(ParticleEmiterComponent emiter)
        {
            for (int i = 0; i < emiter.NumberOfActiveParticles; i++)
            {
                emiter.StartIndex++;
                if (emiter.StartIndex == emiter.Particles.Length)
                    emiter.StartIndex = 0;
            }
        }
    }
}
