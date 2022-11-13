using System.Collections;
using UnityEngine;

namespace Common
{
    public class AudioController : MonoBehaviour
    {
        public AudioSource soundsSource;
        public AudioSource musicSource;
        public AudioSource textSoundSource;
        public float volumeChangeSpeed = 0.02f;

        private AudioClip _textSoundClip;
        private string _currentSound;
        private string _currentMusic;
        private float _originalMusicVolume;
        private MusicVolumeState _currentMusicState;

        private void Start()
        {
            _textSoundClip = Resources.Load<AudioClip>("Audio/typewriter_key_press");
            _originalMusicVolume = musicSource.volume;
            _currentMusicState = MusicVolumeState.Stable;
        }

        public void NewMusic(string musicName)
        {
            if (musicName == null || musicName == _currentMusic)
            {
                return;
            }
            if (musicName == "")
            {
                _currentMusic = musicName;
                StartCoroutine(MusicFade());
                return;
            }

            if (!string.IsNullOrEmpty(_currentMusic))
            {
                StartCoroutine(MusicFade());
            }
            
            _currentMusic = musicName;
            
            var clip = Resources.Load<AudioClip>("Audio/" + musicName);
            musicSource.Stop();
            musicSource.clip = clip;
            musicSource.volume = 0;
            musicSource.Play();
            
            StartCoroutine(MusicRise());
        }

        public void NewSound(string soundName)
        {
            if (soundName == null)
            {
                return;
            }
            if (soundName == "")
            {
                soundsSource.Stop();
                return;
            }

            _currentSound = soundName;
            
            var clip = Resources.Load<AudioClip>("Audio/" + soundName);
            soundsSource.Stop();
            soundsSource.clip = clip;
            soundsSource.Play();
        }

        public void PlayTextSound(float pitchRangeMin, float pitchRangeMax)
        {
            textSoundSource.Stop();
            textSoundSource.clip = _textSoundClip;
            textSoundSource.pitch = Random.Range(pitchRangeMin, pitchRangeMax);
            textSoundSource.Play();
        }

        private IEnumerator MusicFade()
        {
            _currentMusicState = MusicVolumeState.Fading;
            while (musicSource.volume > 0)
            {
                yield return new WaitForSeconds(0.01f);
                musicSource.volume -= volumeChangeSpeed;
                if (musicSource.volume < 0)
                {
                    musicSource.volume = 0;
                }
            }
            _currentMusicState = MusicVolumeState.Stable;
        }
        
        private IEnumerator MusicRise()
        {
            yield return new WaitUntil(() => _currentMusicState == MusicVolumeState.Stable);
            _currentMusicState = MusicVolumeState.Rising;
            while (musicSource.volume < _originalMusicVolume)
            {
                yield return new WaitForSeconds(0.01f);
                musicSource.volume += volumeChangeSpeed;
                if (musicSource.volume > _originalMusicVolume)
                {
                    musicSource.volume = _originalMusicVolume;
                }
            }
            _currentMusicState = MusicVolumeState.Stable;
        }
        
        private enum MusicVolumeState
        {
            Stable, Fading, Rising
        }
    }
}