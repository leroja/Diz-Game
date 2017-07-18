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
            var a = ComponentManager.GetAllEntitiesWithComponentType<ParticleSettingsComponent>();
            Parallel.ForEach(a, id =>
            {
                var emitter = ComponentManager.GetEntityComponent<ParticleEmitterComponent>(id);
                var settings = ComponentManager.GetEntityComponent<ParticleSettingsComponent>(id);

                if (gameTime == null)
                    throw new ArgumentNullException("gameTime");

                emitter.currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                RetireActiveParticles(settings,emitter);
                FreeRetiredParticles(settings,emitter);

                if (emitter.firstActiveParticle == emitter.firstFreeParticle)
                    emitter.currentTime = 0;

                if (emitter.firstRetiredParticle == emitter.firstActiveParticle)
                    emitter.drawCounter = 0;

            }
            );
        }
        /// <summary>
        /// Fre retired particles from particle vector in Emitter
        /// </summary>
        /// <param name="settings">ParticleSettingsComponent for Settings</param>
        /// <param name="emitter">ParticleEmitterComponent For uppdating particles vector</param>
        private void FreeRetiredParticles(ParticleSettingsComponent settings, ParticleEmitterComponent emitter)
        {
            while (emitter.firstRetiredParticle != emitter.firstActiveParticle)
            {
                
                int age = emitter.drawCounter - (int)emitter.particles[emitter.firstRetiredParticle * 4].Time;
                if (age < 3)
                    break;
                emitter.firstRetiredParticle++;

                if (emitter.firstRetiredParticle >= settings.MaxParticles)
                    emitter.firstRetiredParticle = 0;
            }
        }
        /// <summary>
        /// Retire active particles in particle vector
        /// </summary>
        /// <param name="settings">ParticleSettingsComponent for Settings</param>
        /// <param name="emitter">ParticleEmitterComponent For uppdating particles vector</param>
        private void RetireActiveParticles(ParticleSettingsComponent settings, ParticleEmitterComponent emitter)
        {
            float particleDuration = (float)settings.Duration.TotalSeconds;

            while (emitter.firstActiveParticle != emitter.firstNewParticle)
            {
                float particleAge = emitter.currentTime - emitter.particles[emitter.firstActiveParticle * 4].Time;

                if (particleAge < particleDuration)
                    break;

                emitter.particles[emitter.firstActiveParticle * 4].Time = emitter.drawCounter;
                emitter.firstActiveParticle++;

                if (emitter.firstActiveParticle >= settings.MaxParticles)
                    emitter.firstActiveParticle = 0;
            }
        }
    }
}
