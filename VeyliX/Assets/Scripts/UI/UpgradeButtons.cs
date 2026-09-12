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
            playerStats.UpgradeHealth(3);
            upgradeCounts["Health"]++;
        }
        else if (btn == "HealthRegen")
        {
            playerStats.UpgradeHealthRegen(3);
            upgradeCounts["HealthRegen"]++;
        }
        else if (btn == "Energy")
        {
            playerStats.UpgradeEnergy(3);
            upgradeCounts["Energy"]++;
        }
        else if (btn == "EnergyRegen")
        {
            playerStats.UpgradeEnergyRegen(3);
            upgradeCounts["EnergyRegen"]++;
        }
        else if (btn == "Damage")
        {
            playerStats.UpgradeDamage(3);
            upgradeCounts["Damage"]++;
        }
        else if (btn == "BulletSpeed")
        {
            playerStats.UpgradeBulletSpeed(33);
            upgradeCounts["BulletSpeed"]++;
        }
        else if (btn == "Speed")
        {
            playerStats.UpgradeSpeed(3);
            upgradeCounts["Speed"]++;
        }
        else if (btn == "Acceleration")
        {
            playerStats.UpgradeAcceleration(3);
            playerStats.UpgradeRotationSpeed(3);
            upgradeCounts["Acceleration"]++;
        }
        Debug.Log("Upgrade type: " + btn);
    }

    public void resetUpgradeCounts()
    {
        foreach (var key in upgradeCounts.Keys)
        {
            upgradeCounts[key] = 0;
        }
    }
    
}