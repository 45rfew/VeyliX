
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SoundType
{
    Shoot,
    EnemyShoot,
    Explosion,
    Collision,
    Thurst
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    [SerializeField] private int minDamage = 1;
    [SerializeField] private int maxDamage = 255;
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 1.5f;
    [SerializeField] private float minVolume = 0.3f;
    [SerializeField] private float maxVolume = 1f;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioSource enemyAudioSource;
    [SerializeField] private AudioSource thrustingAudioSource;
    [SerializeField] private AudioSource explosionAudioSource;
    [SerializeField] private AudioSource collisionAudioSource;
    private bool isThrusting;
    private static SoundManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (instance == this)
        {
            instance = null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isThrusting = false;
        if (thrustingAudioSource != null)
        {
            thrustingAudioSource.Stop();
        }
    }

    private void Update()
    {
        if (thrustingAudioSource == null)
        {
            return;
        }

        if (isThrusting && !thrustingAudioSource.isPlaying)
        {
            thrustingAudioSource.Play();
        }
        else if (!isThrusting && thrustingAudioSource.isPlaying)
        {
            thrustingAudioSource.Stop();
        }
    }
    public static void PlayShootSound(SoundType sound, int damage = 1, bool isPlayer = true)
    {
        if (instance == null) return;
        float t = Mathf.InverseLerp(instance.minDamage, instance.maxDamage, damage);
        float pitch = Mathf.Lerp(instance.minPitch, instance.maxPitch, t);
        float volume = Mathf.Lerp(instance.minVolume, instance.maxVolume, t);
        AudioSource source = isPlayer ? instance.playerAudioSource : instance.enemyAudioSource;
        if (source == null)
        {
            return;
        }
        if (instance.soundList == null || (int)sound >= instance.soundList.Length || instance.soundList[(int)sound] == null)
        {
            return;
        }

        source.pitch = pitch;
        source.PlayOneShot(instance.soundList[(int)sound], volume);
    }
    public static void ToggleTrustSound(bool thrusting)
    {
        if (instance == null) return;
        instance.isThrusting = thrusting;
    }
    public static void PlayExplosionSound()
    {
        if (instance == null)
        {
            return;
        }
        instance.explosionAudioSource.pitch = Random.Range(0.8f, 1);
        instance.explosionAudioSource.PlayOneShot(instance.explosionAudioSource.clip);
    }
    public static void PlayCollisionSound()
    {
        if (instance == null)
        {
            return;
        }
        //instance.collisionAudioSource.pitch = 1f;
       // instance.collisionAudioSource.PlayOneShot(instance.collisionAudioSource.clip);
    }
}
