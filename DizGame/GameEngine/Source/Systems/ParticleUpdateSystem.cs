using System;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// Updates particles
    /// </summary>
    public class ParticleUpdateSystem : IUpdate
    {
        /// <summary>
        /// Updates Particles of a certain type
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var a = ComponentManager.GetAllEntitiesWithComponentType<ParticleEmitterComponent>();
            Parallel.ForEach(a, id =>
            {
                var emitter = ComponentManager.GetEntityComponent<ParticleEmitterComponent>(id);

                emitter.EmitterLife -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (emitter.EmitterLife < 0)
                {
                    ComponentManager.RemoveEntity(ComponentManager.GetEntityIDByComponent<ParticleEmitterComponent>(emitter));
                    emitter = null;
                }
                if (emitter != null)
                {
                    AddParticle(id, gameTime);
                }
            });
        }

        /// <summary>
        /// Adds particle to ParticelEmitterComponent
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time"></param>
        public void AddParticle(int id, GameTime time)
        {
            Vector3 pos = new Vector3(1, 40, 1);
            ParticleEmitterComponent emitter = ComponentManager.GetEntityComponent<ParticleEmitterComponent>(id);
            TransformComponent tran = ComponentManager.GetEntityComponent<TransformComponent>(id);

            if (emitter.NumberOfActiveParticles + 4 == emitter.NumberOfParticles * 4)
                return;

            OffsetIndex(emitter);
            emitter.NumberOfActiveParticles += 4;

            float startTime = emitter.LifeTime;
            var pot = SetRandomPos(new Vector3(10, 0, 10), new Vector3(-10, 0, -10));

            for (int i = 0; i < 4; i++)
            {
                emitter.Particles[emitter.StartIndex + i].StartPosition = pos;
                emitter.Particles[emitter.StartIndex + i].Direction = emitter.Direction;
                emitter.Particles[emitter.StartIndex + i].Speed = emitter.Speed;
                emitter.Particles[emitter.StartIndex + i].StartTime = startTime;
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
        /// <param name="emitter"></param>
        void OffsetIndex(ParticleEmitterComponent emitter)
        {
            for (int i = 0; i < emitter.NumberOfActiveParticles; i++)
            {
                emitter.StartIndex++;
                if (emitter.StartIndex == emitter.Particles.Length)
                    emitter.StartIndex = 0;
            }
        }
    }
}