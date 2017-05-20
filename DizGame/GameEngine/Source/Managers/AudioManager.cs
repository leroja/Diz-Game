using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace GameEngine.Source.Managers
{
    /// <summary>
    /// A manager that holds songs and soundeffects and manages the playing of them
    /// Also controlls the volume of the sounds that are currently being played
    /// </summary>
    public class AudioManager
    {
        private static AudioManager instance;
        private float prevVol = 1.0f;
        private Dictionary<string, Song> songDic = new Dictionary<string, Song>();
        private Dictionary<string, SoundEffectInstance> soundEffInstDic = new Dictionary<string, SoundEffectInstance>();


        private AudioManager()
        {
        }

        /// <summary>
        /// the instance of the Audio Manager
        /// </summary>
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AudioManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// Mutes the Game
        /// </summary>
        public void GlobalMute()
        {
            MediaPlayer.IsMuted = true;
            prevVol = SoundEffect.MasterVolume;
            SoundEffect.MasterVolume = 0;
        }

        /// <summary>
        /// UnMutes the Game
        /// </summary>
        public void GlobalUnMute()
        {
            MediaPlayer.IsMuted = false;
            SoundEffect.MasterVolume = prevVol;
        }

        /// <summary>
        /// Checks whether the mediaplayer is muted
        /// </summary>
        /// <returns> True if the mediaplayer is muted </returns>
        public bool IsMuted()
        {
            return MediaPlayer.IsMuted;
        }

        /// <summary>
        /// Changes the volume of the mediaplayer
        /// </summary>
        /// <param name="Volume">
        /// MediaPlayer voume, has to be between 0.0 and 1.0
        /// </param>
        public void ChangeSongVolume(float Volume)
        {
            if (Volume <= 1.0 && Volume >= 0.0)
            {
                MediaPlayer.Volume = Volume;
            }
        }

        /// <summary>
        /// Adds the soundEffect to the soundeffect "pool"
        /// </summary>
        /// <param name="effectName"> Name of the soundEffect </param>
        /// <param name="effect"> The soundEffect </param>
        public void AddSoundEffect(string effectName, SoundEffect effect)
        {
            if (effect != null)
            {
                SoundEffectInstance inst = effect.CreateInstance();

                if (!soundEffInstDic.ContainsKey(effectName))
                {
                    soundEffInstDic.Add(effectName, inst);
                }
            }
        }

        /// <summary>
        /// Removes the soundeffect from the dictionary
        /// if there is is a soundeffect with that name
        /// </summary>
        /// <param name="effectName">
        /// The name of the soundEffect
        /// </param>
        public void RemoveSoundEffect(string effectName)
        {
            if (soundEffInstDic.ContainsKey(effectName))
            {
                soundEffInstDic.Remove(effectName);
            }
        }
        /// <summary>
        /// Plays a specific soundeffect
        /// </summary>
        /// <param name="SoundEffect"> name of the soundEffect</param>
        /// <param name="pan"></param>
        /// <param name="pitch"></param>
        public void PlaySoundEffect(string SoundEffect, float pan, float pitch)
        {
            if (soundEffInstDic.ContainsKey(SoundEffect))
            {
                soundEffInstDic[SoundEffect].Pan = pan;
                soundEffInstDic[SoundEffect].Pitch = pitch;
                soundEffInstDic[SoundEffect].Play();
            }
        }

        /// <summary>
        /// Changes the Volume of the soundEffects
        /// </summary>
        /// <param name="volume">
        /// soundEffect Volume has to be between 0.0 and 1.0
        /// </param>
        public void ChangeGlobalSoundEffectVolume(float volume)
        {
            if (volume <= 1.0 && volume >= 0.0)
            {
                prevVol = volume;
                SoundEffect.MasterVolume = volume;
            }
        }

        /// <summary>
        /// Changes the repeat states of the mediaplayer
        /// </summary>
        public void ChangeRepeat()
        {
            MediaPlayer.IsRepeating = !MediaPlayer.IsMuted;
        }

        /// <summary>
        /// Adds the song to the song "pool"
        /// </summary>
        /// <param name="songName">
        /// The name of the song
        /// </param>
        /// <param name="song">
        /// The song itself
        /// </param>
        public void AddSong(string songName, Song song)
        {
            if (song != null)
            {
                if (!songDic.ContainsKey(songName))
                {
                    songDic.Add(songName, song);
                }
            }
        }

        /// <summary>
        /// Removes a specific song
        /// if there is song with that name in the Manager
        /// </summary>
        /// <param name="songName">
        /// Name of the song
        /// </param>
        public void RemoveSong(string songName)
        {
            if (songDic.ContainsKey(songName))
            {
                songDic.Remove(songName);
            }
        }

        /// <summary>
        /// Plays the song if there is one with that name in the manager
        /// </summary>
        /// <param name="name">
        /// Name of the song
        /// </param>
        public void PlaySong(string name)
        {
            if (songDic.ContainsKey(name))
            {
                StopSong();
                MediaPlayer.Play(songDic[name]);
            }
        }

        /// <summary>
        /// Stops playing the song that is currently being played
        /// </summary>
        public void StopSong()
        {
            MediaPlayer.Stop();
        }
    }
}
