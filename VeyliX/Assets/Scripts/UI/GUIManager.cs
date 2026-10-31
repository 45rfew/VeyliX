using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public PlayerHealth health;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI gemText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lvlText;
    public PlayerEnergy playerEnergy;
    //public Slider healthBar;
    public PlayerCrystals crystalPickup;
    public PlayerStats playerStats;
    public GameObject shipTierUpgrade;
    public GameObject shipUpgrades;
    public UpgradeButtons upgradeButtons;
    private int playerScore;
    
    void Start()
    {
        health.OnDamage += UpdateHealth;
        health.OnHeal += UpdateHealth;
        health.OnScoreChanged += UpdateScore;
        crystalPickup.OnplayerCrystalChange += UpdateGem;
        playerEnergy.OnEnergyRegen += UpdateEnergy;
        UpdateEnergy(playerEnergy.GetEnergy(),playerEnergy.GetEnergy());
    }

    void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthText.text = $"Shield: {currentHealth}/{maxHealth}";
        //healthBar.value = currentHealth / maxHealth * 100 <= 0 ? 0.01f : currentHealth / maxHealth * 100; 
        //healthText.text = currentHealth < 0 ? $"0 / {maxHealth}" : $"{currentHealth} / {maxHealth}";
    }
    void UpdateEnergy(float currentEnergy, float maxEnergy)
    {
        energyText.text = $"Energy: {Mathf.Floor(currentEnergy)}/{Mathf.Floor(maxEnergy)}";    
    }
    void UpdateGem(int currentGem)
    { 
        if (currentGem >= playerStats.UpgradeCrystalCost())
        {
            if (!upgradeButtons.CheckUpgradesMaxed())
            {
                shipUpgrades.SetActive(true);
            }
        } else
        {
            shipUpgrades.SetActive(false);           
        }
        int maxGem = playerStats.GetMaxCrystals();
        if (currentGem >= maxGem)
        {
            shipTierUpgrade.SetActive(true);
        } else
        {
            shipTierUpgrade.SetActive(false);
        }
        gemText.text = $"Gems: {currentGem}/{maxGem}";    
    }
    
    void UpdateScore(int score)
    {
        Debug.Log("Score updated by " + score);
        playerScore += score;
        scoreText.text = "Score: " + playerScore;
    }
}