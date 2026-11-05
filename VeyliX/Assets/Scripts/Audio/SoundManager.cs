
using UnityEngine;

public enum SoundType
{
    Shoot
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;

    [Header("Damage Audio Settings")]
    [SerializeField] private int minDamage = 1;
    [SerializeField] private int maxDamage = 255;

    [Header("Pitch")]
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 1.5f;

    [Header("Volume")]
    [SerializeField] private float minVolume = 0.3f;
    [SerializeField] private float maxVolume = 1f;

    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, int damage = 1)
    {
        if (instance == null) return;
        float t = Mathf.InverseLerp(instance.minDamage,instance.maxDamage,damage);
        float pitch = Mathf.Lerp(instance.minPitch,instance.maxPitch,t);
        float volume = Mathf.Lerp(instance.minVolume,instance.maxVolume,t);
        instance.audioSource.pitch = pitch;
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound],volume);
    }
}
