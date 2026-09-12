using System;
using UnityEngine;
using UnityEngine.EventSystems; 
using System.Collections.Generic;

public class UpgradeButtons : MonoBehaviour
{
    public PlayerStats playerStats;
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
        {
            upgradeActions["Health"](shipTier);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            upgradeActions["HealthRegen"](shipTier);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            upgradeActions["Energy"](shipTier);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            upgradeActions["EnergyRegen"](shipTier);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            upgradeActions["Damage"](shipTier);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            upgradeActions["BulletSpeed"](shipTier);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            upgradeActions["Speed"](shipTier);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            upgradeActions["Acceleration"](shipTier);
        }
    }
    public void GetUpgradeType()
    {
        shipTier = playerStats.GetShipTier();

        string btn = EventSystem.current.currentSelectedGameObject.name;

        if (upgradeCounts[btn] < shipTier)
        {
            upgradeActions[btn](shipTier);
            upgradeCounts[btn]++;
        }
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
}