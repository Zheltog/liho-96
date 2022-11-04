using UnityEngine;

public class AudioController : MonoBehaviour
{

    public AudioSource soundsSource;
    public AudioSource musicSource;

    public void NewAudio()
    {
        var nextMusic = GameStateHolder.CurrentFrame.Music;
        var nextSound = GameStateHolder.CurrentFrame.Sound;

        if (nextMusic != null)
        {
            PlayMusic(nextMusic);
        }
        
        if (nextSound != null)
        {
            // TODO
        }
    }

    public void PlayMusic(string clipName)
    {
        var clip = Resources.Load<AudioClip>("Audio/" + clipName);
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
    }
}