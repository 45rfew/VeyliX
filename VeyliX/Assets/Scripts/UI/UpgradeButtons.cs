using System;
using UnityEngine;
using UnityEngine.EventSystems; 
using System.Collections.Generic;

public class UpgradeButtons : MonoBehaviour
{
    public PlayerStats playerStats;
    public PlayerCrystals playerCrystals;
    private int shipTier;
    private Dictionary<string, int> upgradeCounts = new Dictionary<string, int>()
    {
        {"Health", 0 },
        {"HealthRegen", 0 },
        {"Energy", 0 },
        {"EnergyRegen", 0 },
        {"Damage", 0 },
        {"BulletSpeed", 0 },
        {"Speed", 0 },
        {"Acceleration", 0 }
    };
    private Dictionary<string, Action<int>> upgradeActions = new Dictionary<string, Action<int>>();
    void Start()
    {
        upgradeActions = new Dictionary<string, Action<int>>()
        {
            {"Health", playerStats.UpgradeHealth},
            {"HealthRegen", playerStats.UpgradeHealthRegen},
            {"Energy", playerStats.UpgradeEnergy},
            {"EnergyRegen", playerStats.UpgradeEnergyRegen},
            {"Damage", playerStats.UpgradeDamage},
            {"BulletSpeed", playerStats.UpgradeBulletSpeed},
            {"Speed", playerStats.UpgradeSpeed},
            {"Acceleration", playerStats.UpgradeAcceleration },
        };
        upgradeActions["Acceleration"] += playerStats.UpgradeRotationSpeed;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Upgrade("Health");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            Upgrade("HealthRegen");

        if (Input.GetKeyDown(KeyCode.Alpha3))
            Upgrade("Energy");

        if (Input.GetKeyDown(KeyCode.Alpha4))
            Upgrade("EnergyRegen");

        if (Input.GetKeyDown(KeyCode.Alpha5))
            Upgrade("Damage");

        if (Input.GetKeyDown(KeyCode.Alpha6))
            Upgrade("BulletSpeed");

        if (Input.GetKeyDown(KeyCode.Alpha7))
            Upgrade("Speed");

        if (Input.GetKeyDown(KeyCode.Alpha8))
            Upgrade("Acceleration");
    }
    public void Upgrade(string upgradeName)
    {
        if (!EnoughCrystals())
            return;
        if (upgradeCounts[upgradeName] >= shipTier)
            return;
        playerCrystals.RemoveCrystals(shipTier * 10);
        upgradeActions[upgradeName](shipTier);
        upgradeCounts[upgradeName]++;
    }
    public void GetUpgradeType()
    {
        string btn = EventSystem.current.currentSelectedGameObject.name;

        Upgrade(btn);
    }
    public void ResetUpgradeCounts()
    {
        upgradeCounts = new Dictionary<string, int>()
        {
            {"Health", 0 },
            {"HealthRegen", 0 },
            {"Energy", 0 },
            {"EnergyRegen", 0 },
            {"Damage", 0 },
            {"BulletSpeed", 0 },
            {"Speed", 0 },
            {"Acceleration", 0 }
        };
    }
    public bool EnoughCrystals()
    {
        shipTier = playerStats.GetShipTier();
        if (playerCrystals.GetCrystalCount() >= shipTier * 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}