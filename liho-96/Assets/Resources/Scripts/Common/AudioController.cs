using UnityEngine;

namespace Common
{
    public class AudioController : MonoBehaviour
    {
        public AudioSource soundsSource;
        public AudioSource musicSource;
        public AudioSource textSoundSource;

        private AudioClip _textSoundClip;

        private string _currentSound;
        private string _currentMusic;

        private void Start()
        {
            _textSoundClip = Resources.Load<AudioClip>("Audio/typewriter_key_press");
        }

        public void NewMusic(string musicName)
        {
            if (musicName == null || musicName == _currentMusic)
            {
                return;
            }
            if (musicName == "")
            {
                musicSource.Stop();
                return;
            }

            _currentMusic = musicName;
            
            var clip = Resources.Load<AudioClip>("Audio/" + musicName);
            musicSource.Stop();
            musicSource.clip = clip;
            musicSource.Play();
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
    }
}