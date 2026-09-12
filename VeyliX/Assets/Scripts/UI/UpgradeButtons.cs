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

    public void GetUpgradeType()
    {
        shipTier = playerStats.GetShipTier();
        string btn = EventSystem.current.currentSelectedGameObject.name;
        if (btn == "Health")
        {
            if (upgradeCounts["Health"] < shipTier)
            {
                playerStats.UpgradeHealth(shipTier);
                upgradeCounts["Health"]++;
            }
        }
        else if (btn == "HealthRegen")
        {
            if (upgradeCounts["HealthRegen"] < shipTier)
            {
                playerStats.UpgradeHealthRegen(shipTier);
                upgradeCounts["HealthRegen"]++;
            }
        }
        else if (btn == "Energy")
        {
            if (upgradeCounts["Energy"] < shipTier)
            {
                playerStats.UpgradeEnergy(shipTier);
                upgradeCounts["Energy"]++;
            }
        }
        else if (btn == "EnergyRegen")
        {
            if (upgradeCounts["EnergyRegen"] < shipTier)
            {
                playerStats.UpgradeEnergyRegen(shipTier);
                upgradeCounts["EnergyRegen"]++;
            }
        }
        else if (btn == "Damage")
        {
            if (upgradeCounts["Damage"] < shipTier)
            {
                playerStats.UpgradeDamage(shipTier);
                upgradeCounts["Damage"]++;
            }
        }
        else if (btn == "BulletSpeed")
        {
            if (upgradeCounts["BulletSpeed"] < shipTier)
            {
                playerStats.UpgradeBulletSpeed(shipTier);
                upgradeCounts["BulletSpeed"]++;
            }
        }
        else if (btn == "Speed")
        {
            if (upgradeCounts["Speed"] < shipTier)
            {
                playerStats.UpgradeSpeed(shipTier);
                upgradeCounts["Speed"]++;
            }
        }
        else if (btn == "Acceleration")
        {
            if (upgradeCounts["Acceleration"] < shipTier)
            {
                playerStats.UpgradeAcceleration(shipTier);
                playerStats.UpgradeRotationSpeed(shipTier);
                upgradeCounts["Acceleration"]++;
            }
        }
        Debug.Log("Upgrade type: " + btn);
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