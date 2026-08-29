using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    protected float maxHealth = 100f;
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
    public virtual void Heal(float amount)
    {
        if (currentHealth + amount >= maxHealth)
        {
            currentHealth = maxHealth;
        } else
        {
            currentHealth += amount;
        }
        OnHeal?.Invoke(currentHealth,maxHealth);
    }
    protected virtual void Die()
    {
        OnDeath?.Invoke();
    }
}