using GameEngine.Source.Managers;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Systems
{
    class SoundSystem
    {
        AudioManager _manager;
        public SoundSystem(AudioManager manager)
        {
            _manager = manager;
        }
        /// <summary>
        /// Plays a song
        /// </summary>
        /// <param name="songName"></param>
        private void PlaySong(string songName)
        {

            _manager.PlaySong(songName);
        }

        /// <summary>
        /// Stops the current song
        /// </summary>
        private void StopSong()
        {
            _manager.StopSong();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SoundEffect"></param>
        /// <param name="pan"></param>
        /// <param name="pitch"></param>
        private void PlaySoundEffect(string SoundEffect, float pan, float pitch)
        {
            _manager.PlaySoundEffect(SoundEffect, pan, pitch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mute"></param>
        /// <param name="newVolume"></param>
        private void SetSongVolume(bool mute, float newVolume)
        {
            float currentVolume = MediaPlayer.Volume;
            if (mute == true)
            {
                _manager.GlobalMute();
            }
            
            if (mute == false)
            {
                _manager.GlobalUnMute();
                if(currentVolume < newVolume)
                {
                    _manager.ChangeSongVolume(newVolume);
                }
                else if(currentVolume > newVolume)
                {
                    _manager.ChangeSongVolume(newVolume);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mute"></param>
        /// <param name="newVolume"></param>
        private void SetEffectVolume(bool mute, float newVolume)
        {
            float currentVolume = MediaPlayer.Volume;
            if (mute == true)
            {
                _manager.GlobalMute();
            }

            if (mute == false)
            {
                _manager.GlobalUnMute();
                if (currentVolume < newVolume)
                {
                    _manager.ChangeGlobalSoundEffectVolume(newVolume);
                }
                else if (currentVolume > newVolume)
                {
                    _manager.ChangeGlobalSoundEffectVolume(newVolume);
                }
            }
        }
    }
}
