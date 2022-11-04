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
            PlaySound(nextSound);
        }
    }

    public void PlayMusic(string clipName)
    {
        var clip = Resources.Load<AudioClip>("Audio/" + clipName);
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
    }
    
    public void PlaySound(string clipName)
    {
        var clip = Resources.Load<AudioClip>("Audio/" + clipName);
        soundsSource.Stop();
        soundsSource.clip = clip;
        soundsSource.Play();
    }
}