using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    public Action<int> OnScoreChanged;
    private int score;
    private PlayerStats playerStats;
    protected override void Start()
    {
//base.Start();
        playerStats = FindAnyObjectByType<PlayerStats>();
        OnDamage += HandleDamage;
        OnDeath += HandleDeath;
        maxHealth = playerStats.GetCurrentShip().health.GetValue();
        currentHealth = maxHealth;
        OnDamage?.Invoke(maxHealth, maxHealth);
    }
    void HandleDamage(float currentHealth, float maxHealth)
    {
        float damage = maxHealth - currentHealth;
    }
    public void SetMaxHealth(float newHealth)
    {
        float damageTaken = maxHealth - currentHealth;
        maxHealth = newHealth;
        currentHealth = maxHealth - damageTaken;
        OnDamage?.Invoke(currentHealth, maxHealth);
    }
    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke(amount);
    }
    void HandleDeath()
    {
        Debug.Log("player die");
        
        Destroy(gameObject, 0.1f);
    }
}