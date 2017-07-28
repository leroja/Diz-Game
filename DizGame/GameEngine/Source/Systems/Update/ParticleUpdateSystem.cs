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

                emitter.CurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                RetireActiveParticles(settings, emitter);
                FreeRetiredParticles(settings, emitter);

                if (emitter.FirstActiveParticle == emitter.FirstFreeParticle)
                    emitter.CurrentTime = 0;

                if (emitter.FirstRetiredParticle == emitter.FirstActiveParticle)
                    emitter.DrawCounter = 0;

            }
            );
        }

        /// <summary>
        /// Free retired particles from particle vector in Emitter
        /// </summary>
        /// <param name="settings">ParticleSettingsComponent for Settings</param>
        /// <param name="emitter">ParticleEmitterComponent For updating particles vector</param>
        private void FreeRetiredParticles(ParticleSettingsComponent settings, ParticleEmitterComponent emitter)
        {
            while (emitter.FirstRetiredParticle != emitter.FirstActiveParticle)
            {
                int age = emitter.DrawCounter - (int)emitter.Particles[emitter.FirstRetiredParticle * 4].Time;
                if (age < 3)
                    break;
                emitter.FirstRetiredParticle++;

                if (emitter.FirstRetiredParticle >= settings.MaxParticles)
                    emitter.FirstRetiredParticle = 0;
            }
        }

        /// <summary>
        /// Retire active particles in particle vector
        /// </summary>
        /// <param name="settings">ParticleSettingsComponent for Settings</param>
        /// <param name="emitter">ParticleEmitterComponent For updating particles vector</param>
        private void RetireActiveParticles(ParticleSettingsComponent settings, ParticleEmitterComponent emitter)
        {
            float particleDuration = (float)settings.Duration.TotalSeconds;

            while (emitter.FirstActiveParticle != emitter.FirstNewParticle)
            {
                float particleAge = emitter.CurrentTime - emitter.Particles[emitter.FirstActiveParticle * 4].Time;

                if (particleAge < particleDuration)
                    break;

                emitter.Particles[emitter.FirstActiveParticle * 4].Time = emitter.DrawCounter;
                emitter.FirstActiveParticle++;

                if (emitter.FirstActiveParticle >= settings.MaxParticles)
                    emitter.FirstActiveParticle = 0;
            }
        }
    }
}