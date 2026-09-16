using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    public Action<int> OnScoreChanged;
    private int score;
    private PlayerStats playerStats;
    private float regen;
    protected override void Start()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();
        OnDamage += HandleDamage;
        OnDeath += HandleDeath;
        maxHealth = playerStats.GetCurrentShip().health.GetValue();
        currentHealth = maxHealth;
        OnDamage?.Invoke(maxHealth, maxHealth);
        regen = playerStats.GetCurrentShip().healthRegen.GetValue();
    }
    void Update()
    {
        if (currentHealth < maxHealth)
        {
            Heal(regen * Time.deltaTime);
        } 
    }
    void HandleDamage(float currentHealth, float maxHealth)
    {
        float damage = maxHealth - currentHealth;
    }
    public void Heal(float amount)
    {
        currentHealth = Math.Min(currentHealth + amount, maxHealth);
        OnHeal?.Invoke(currentHealth,maxHealth);
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
    public void NewShip()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();
        maxHealth = playerStats.GetCurrentShip().health.GetValue();
        currentHealth = maxHealth;
        regen = playerStats.GetCurrentShip().healthRegen.GetValue();
        OnDamage?.Invoke(maxHealth, maxHealth);
    }
    public void Upgrade()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();
        maxHealth = playerStats.GetCurrentShip().health.GetValue();
        regen = playerStats.GetCurrentShip().healthRegen.GetValue();
        OnDamage?.Invoke(currentHealth, maxHealth);        
    }
    void HandleDeath()
    {
        Debug.Log("player die");
        float[] gemValues = {0, 5, 15, 25, 40, 85, 100};
        float amount = gemValues[playerStats.GetShipTier() - 1];
        Vector3 pos = transform.position;
        CrystalSpawner.Instance.DropCrystals(amount, pos);
        Destroy(gameObject, 0.1f);
    }
}