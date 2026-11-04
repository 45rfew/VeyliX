using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    public Action<int> OnScoreChanged;
    public Action<bool> OnShipDestroyed;
    public Action<int> OnLivesChanged;
    private int score;
    private PlayerStats playerStats;
    private PlayerCrystals playerCrystals;
    private GUIManager guiManager;
    private int regen;
    private float regenAccumulator;
    private int maxlives = 3;
    private int lives;
    private bool isDead = false;
    protected override void Start()
    {
        lives = maxlives;
        playerStats = FindAnyObjectByType<PlayerStats>();
        playerCrystals = FindAnyObjectByType<PlayerCrystals>();
        guiManager = FindAnyObjectByType<GUIManager>();
        OnDamage += HandleDamage;
        OnDeath += HandleDeath;
        guiManager.OnRespawnTriggered += HandleRespawn;
        maxHealth = playerStats.GetCurrentShip().health.GetValue();
        currentHealth = maxHealth;
        OnDamage?.Invoke(maxHealth, maxHealth);
        OnLivesChanged?.Invoke(lives);
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
    void HandleRespawn(bool respawn)
    {
        if (respawn)
        {
            isDead = false;
            NewShip();
            OnLivesChanged?.Invoke(lives);
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
                CrystalSpawner.Instance.DropCrystals(overflow, transform.position);
            }
            else
            {
                playerCrystals.RemoveCrystals(crystals);
                CrystalSpawner.Instance.DropCrystals(crystals, transform.position);
                if (!isDead)
                {
                    isDead = true;
                    Die();
                }
            }
        }
        OnDamage?.Invoke(currentHealth, maxHealth);
    }
    public void SetLives(int newLives)
    {
        lives = newLives;
        OnLivesChanged?.Invoke(lives);
    }
    public void ResetLives()
    {
        lives = maxlives;
        OnLivesChanged?.Invoke(lives);
    }
    public int GetLives()
    {
        return lives;
    }
    public float GetHealth()
    {
        return currentHealth;
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
        gameObject.SetActive(true);
        playerStats = FindAnyObjectByType<PlayerStats>();
        maxHealth = playerStats.GetCurrentShip().health.GetValue();
        currentHealth = maxHealth;
        regen = playerStats.GetCurrentShip().healthRegen.GetValue();
        OnDamage?.Invoke(maxHealth, maxHealth);
        //lives = maxlives;
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
        if (lives > 0)
        {
            lives--;
            OnLivesChanged?.Invoke(lives);
            OnShipDestroyed?.Invoke(true);
            Debug.Log("Respawn player, lives left: " + lives);
        } 
        else
        {
            OnShipDestroyed?.Invoke(false);
            SceneManager.LoadScene("GameOver");
        }
        gameObject.SetActive(false);
        //Destroy(gameObject, 0.1f);
    }
}