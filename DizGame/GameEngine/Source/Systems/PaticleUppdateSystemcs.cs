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
    class ParticleUpdateSystem : IUpdate
    {
        /// <summary>
        /// Uppdates Particles of a serten type
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

            if (emiter.numberOfActiveParticles + 4 == emiter.nParticles * 4)
                return;

            offsetIndex(emiter);
            emiter.numberOfActiveParticles += 4;

            float startTime = emiter.lifeTime;
            var pot = SetRandomPos(new Vector3(10 , 0, 10), new Vector3(-10, 0, -10));
            


            for (int i = 0; i < 4; i++)
            {
                emiter.particle[emiter.StartIndex + i].startPosition = pos;
                emiter.particle[emiter.StartIndex + i].direction = emiter.Direction;
                emiter.particle[emiter.StartIndex + i].speed = emiter.speed;
                emiter.particle[emiter.StartIndex + i].startTime = startTime;
            }
        }
        /// <summary>
        /// setts random position inside of bounds
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
        void offsetIndex(ParticleEmiterComponent emiter)
        {
            for (int i = 0; i < emiter.numberOfActiveParticles; i++)
            {
                emiter.StartIndex++;
                if (emiter.StartIndex == emiter.particle.Length)
                    emiter.StartIndex = 0;
            }
        }

    }
}
