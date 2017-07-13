using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework.Audio;
using GameEngine.Source.Managers;
using System;

namespace GameEngine.Source.Systems
{
    /// <summary>
    /// A system for playing sound effects
    /// </summary>
    public class SoundEffectSystem : IUpdate
    {
        private List<SoundEffectInstance> activeSoundEffects;

        /// <summary>
        /// Constructor
        /// </summary>
        public SoundEffectSystem()
        {
            activeSoundEffects = new List<SoundEffectInstance>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var soundEffectCompIDs = ComponentManager.GetAllEntitiesWithComponentType<SoundEffectComponent>();

            foreach (var Id in soundEffectCompIDs)
            {
                var soundEffectComponent = ComponentManager.GetEntityComponent<SoundEffectComponent>(Id);

                foreach (var soundEffect in soundEffectComponent.SoundEffectsToBePlayed)
                {
                    activeSoundEffects.Add(AudioManager.Instance.PlaySoundEffect(soundEffect.Item1, soundEffect.Item2, soundEffect.Item3));
                }
                soundEffectComponent.SoundEffectsToBePlayed.Clear();
            }
            

            for (int i = 0; i < activeSoundEffects.Count; i++)
            {
                var activeSound = activeSoundEffects[i];

                if (activeSound.State == SoundState.Stopped)
                {
                    // If the sound has stopped playing, dispose it.
                    activeSound.Dispose();

                    // Remove it from the active list.
                    activeSoundEffects.RemoveAt(i);
                }
            }
        }
    }
}