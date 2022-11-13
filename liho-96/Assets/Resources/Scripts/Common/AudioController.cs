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
        private string _currentMusic;
        private float _originalMusicVolume;

        private void Start()
        {
            _textSoundClip = Resources.Load<AudioClip>("Audio/typewriter_key_press");
            _originalMusicVolume = musicSource.volume;
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

            if (string.IsNullOrEmpty(_currentMusic))
            {
                PlayNewMusic(musicName);
            }
            else
            {
                StartCoroutine(MusicFadeAndPlayNew(musicName));
            }
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

        private void PlayNewMusic(string musicName)
        {
            musicSource.Stop();
            _currentMusic = musicName;
            musicSource.clip = Resources.Load<AudioClip>("Audio/" + musicName);
            musicSource.volume = 0;
            musicSource.Play();
            StartCoroutine(MusicRise());
        }

        private IEnumerator MusicFadeAndPlayNew(string musicName)
        {
            yield return MusicFade();
            PlayNewMusic(musicName);
            StartCoroutine(MusicRise());
        }
        
        private IEnumerator MusicFade()
        {
            while (musicSource.volume > 0)
            {
                yield return new WaitForSeconds(0.01f);
                musicSource.volume -= volumeChangeSpeed;
                if (musicSource.volume < 0)
                {
                    musicSource.volume = 0;
                }
            }
        }
        
        private IEnumerator MusicRise()
        {
            while (musicSource.volume < _originalMusicVolume)
            {
                yield return new WaitForSeconds(0.01f);
                musicSource.volume += volumeChangeSpeed;
                if (musicSource.volume > _originalMusicVolume)
                {
                    musicSource.volume = _originalMusicVolume;
                }
            }
        }
    }
}