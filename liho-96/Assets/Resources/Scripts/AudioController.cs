using UnityEngine;

public class AudioController : MonoBehaviour
{

    public AudioSource soundsSource;
    public AudioSource musicSource;

    public void NewMusic(string musicName)
    {
        if (musicName == null) return;
        var clip = Resources.Load<AudioClip>("Audio/" + musicName);
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
    }
    
    public void NewSound(string soundName)
    {
        if (soundName == null) return;
        var clip = Resources.Load<AudioClip>("Audio/" + soundName);
        soundsSource.Stop();
        soundsSource.clip = clip;
        soundsSource.Play();
    }
}