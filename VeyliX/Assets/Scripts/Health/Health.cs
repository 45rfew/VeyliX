using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float currentHealth;
    public Action<float,float> OnDamage;
    public Action<float,float> OnHeal;
    public Action OnDeath;
    protected virtual void Start()
    {
        //currentHealth = maxHealth;
    }
    public virtual void TakeDamage(float amount)
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
    }
}