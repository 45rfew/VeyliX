using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int currentHealth;
    public Action<int, int> OnDamage;
    public Action<int, int> OnHeal;
    public Action OnDeath;
    public GameObject deathEffectPrefab;
    protected virtual void Start()
    {
        //currentHealth = maxHealth;
    }
    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        OnDamage?.Invoke(currentHealth,maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        OnDeath?.Invoke();
        Renderer renderer = GetComponentInChildren<Renderer>();
        float scale = 1f;
        if (renderer != null)
        {
            scale = renderer.bounds.size.x;
        }
        if (deathEffectPrefab != null)
        {
            GameObject effect = Instantiate(deathEffectPrefab, transform.position, transform.rotation);
            effect.transform.localScale = Vector3.one * scale * 0.8f;
            Destroy(effect, 1.5f);
        }
        SoundManager.PlayExplosionSound();

    }
}