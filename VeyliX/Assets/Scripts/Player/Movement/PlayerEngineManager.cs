using UnityEngine;

public class PlayerEngineManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] flamePrefabs;
    void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            foreach (ParticleSystem flame in flamePrefabs ?? System.Array.Empty<ParticleSystem>())
            {
                if (flame != null)
                {
                    flame.Play();
                }
                SoundManager.ToggleTrustSound(true);
            }
        }
        else
        {
            foreach (ParticleSystem flame in flamePrefabs ?? System.Array.Empty<ParticleSystem>())
            {
                if (flame != null)
                {
                    flame.Stop(false, ParticleSystemStopBehavior.StopEmitting);
                }
                SoundManager.ToggleTrustSound(false);
            }
        }
    }
}