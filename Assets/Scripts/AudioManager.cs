using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;

    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;

    public AudioSource musicAudio;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("MusicVolume", volume);
    }
    public void SetSFXVolume(float volume)
    {
        sfxMixer.SetFloat("SFXVolume", volume);
    }

    public void SetMusicPitch(float pitch)
    {
        musicAudio.pitch = pitch;
    }

    public void PlaySFX(AudioSource sfxSource, AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    


}
