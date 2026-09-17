using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int currentHealth;
    public Action<int, int> OnDamage;
    public Action<int, int> OnHeal;
    public Action OnDeath;
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
    }
}