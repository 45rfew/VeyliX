using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    public Action<int> OnScoreChanged;
    private int score;
    private PlayerStats playerStats;
    private PlayerCrystals playerCrystals;
    private int regen;
    private float regenAccumulator;
    protected override void Start()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();
        playerCrystals = FindAnyObjectByType<PlayerCrystals>();
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
            regenAccumulator += regen * Time.deltaTime;
            int healing = Mathf.FloorToInt(regenAccumulator);
            if (healing > 0)
            {
                regenAccumulator -= healing;
                Heal(healing);
            }
        }
    }
    public override void TakeDamage(int amount)
    {
        if (amount <= currentHealth)
        {
            currentHealth -= amount;
        }
        else
        {
            int overflow = amount - currentHealth;

            currentHealth = 0;

            int crystals = playerCrystals.GetCrystalCount();

            if (crystals >= overflow)
            {
                playerCrystals.RemoveCrystals(overflow);

                CrystalSpawner.Instance.DropCrystals(
                    overflow,
                    transform.position
                );
            }
            else
            {
                playerCrystals.RemoveCrystals(crystals);

                CrystalSpawner.Instance.DropCrystals(
                    crystals,
                    transform.position
                );

                Die();
            }
        }

        OnDamage?.Invoke(currentHealth, maxHealth);
    }
    void HandleDamage(int currentHealth, int maxHealth)
    {
        int damage = maxHealth - currentHealth;
    }
    public void Heal(int amount)
    {
        currentHealth = Math.Min(currentHealth + amount, maxHealth);
        OnHeal?.Invoke(currentHealth,maxHealth);
    }
    public void SetMaxHealth(int newHealth)
    {
        int damageTaken = maxHealth - currentHealth;
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
        int[] gemValues = {0, 5, 15, 25, 40, 85, 100};
        int amount = gemValues[playerStats.GetShipTier() - 1];
        Vector3 pos = transform.position;
        CrystalSpawner.Instance.DropCrystals(amount, pos);
        Destroy(gameObject, 0.1f);
    }
}