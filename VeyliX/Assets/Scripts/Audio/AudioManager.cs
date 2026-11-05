
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Volume")]
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;

    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    void Awake()
    {
        // Prevent multiple AudioManagers
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource.loop = true;
        musicSource.volume = musicVolume;

        sfxSource.volume = sfxVolume;
    }

    // Play background music
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        // Don't restart music if it is already playing
        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Play a sound effect
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    // Change volume
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
    }
}
