using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework.Audio;
using GameEngine.Source.Managers;

namespace GameEngine.Source.Systems
{
    // TODO Fix the problem with the 3D sounds, 
    /// <summary>
    /// 
    /// </summary>
    public class _3DSoundSystem : IUpdate
    {
        private List<SoundEffectInstance> activeSoundEffects;

        /// <summary>
        /// Constructor
        /// </summary>
        public _3DSoundSystem()
        {
            activeSoundEffects = new List<SoundEffectInstance>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            var temp = ComponentManager.GetAllEntitiesAndComponentsWithComponentType<_3DAudioListenerComponent>();
            var ListenerID = temp.First().Key;
            var listenerTransComp = ComponentManager.GetEntityComponent<TransformComponent>(ListenerID);
            var listenerPhysComp = ComponentManager.GetEntityComponent<PhysicsComponent>(ListenerID);

            AudioListener listener = new AudioListener()
            {
                Position = listenerTransComp.Position,
                Forward = listenerTransComp.Forward,
                Up = listenerTransComp.Up,
                Velocity = listenerPhysComp.Velocity,
            };

            var soundEffectCompIDs = ComponentManager.GetAllEntitiesWithComponentType<_3DSoundEffectComponent>();

            foreach (var Id in soundEffectCompIDs)
            {
                var soundEffectComponent = ComponentManager.GetEntityComponent<_3DSoundEffectComponent>(Id);
                var transComp = ComponentManager.GetEntityComponent<TransformComponent>(Id);
                var physComp = ComponentManager.GetEntityComponent<PhysicsComponent>(Id);

                AudioEmitter emitter = new AudioEmitter()
                {
                    Position = transComp.Position,
                    Velocity = physComp.Velocity,
                    Up = transComp.Up,
                    Forward = transComp.Forward,
                    DopplerScale = 1,
                };

                foreach (var soundEffect in soundEffectComponent.SoundEffectsToBePlayed)
                {
                    activeSoundEffects.Add(AudioManager.Instance.Play3DSoundEffect(soundEffect.Item1, soundEffect.Item2, emitter, listener));
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