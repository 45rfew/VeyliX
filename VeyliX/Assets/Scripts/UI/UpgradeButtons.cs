using System;
using UnityEngine;
using UnityEngine.EventSystems; 
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;


public class UpgradeButtons : MonoBehaviour
{
    public PlayerStats playerStats;
    public PlayerCrystals playerCrystals;
    public GameObject healthButton;
    public GameObject upgradeButtons;
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
            Upgrade("Health", upgradeButtons);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Upgrade("HealthRegen",upgradeButtons);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Upgrade("Energy", upgradeButtons);
        }   
        if (Input.GetKeyDown(KeyCode.Alpha4))
    
            Upgrade("EnergyRegen", upgradeButtons);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            Upgrade("Damage", upgradeButtons);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            Upgrade("BulletSpeed", upgradeButtons);
        if (Input.GetKeyDown(KeyCode.Alpha7))
            Upgrade("Speed", upgradeButtons);
        if (Input.GetKeyDown(KeyCode.Alpha8))
            Upgrade("Acceleration", upgradeButtons);
    }
    public void UpdateUpgradeUI(GameObject button, string upgradeName, int upgradeCount, int shipTier)
    {
        if (button == null)
        {
            Debug.LogError("Button is null!");
            return;
        }
        Slider slider = button.GetComponentInChildren<Slider>();
        if (slider == null)
        {
            Debug.LogError("No Slider found on " + button.name);
            return;
        }
        Button upgradeButton = button.GetComponentInChildren<Button>();
        /*
        Button upgradeButton = null;
        foreach (Button childButton in button.GetComponentsInChildren<Button>(true))
        {
            if (childButton.gameObject.name == upgradeName)
            {
                upgradeButton = childButton;
                break;
            }
        }
        */
        if (upgradeButton == null)
        {
            Debug.LogError($"No Button named {upgradeName} found under {button.name}");
            return;
        }
        if (shipTier <= 0)
        {
            Debug.LogError("Ship Tier is zero or less!");
            return;
        }
        slider.value = (float)upgradeCount / shipTier;

        if (upgradeCount >= shipTier)
        {
            Debug.Log($"Upgrade {upgradeName} has reached max level for ship tier {shipTier}. Disabling button.");
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeButton.interactable = true;
        }
        Debug.Log($"Upgrade: {upgradeName}, Count: {upgradeCount}, Ship Tier: {shipTier}, Slider Value: {slider.value}");
    }
    public void Upgrade(string upgradeName, GameObject upgrades)
    {
        if (!EnoughCrystals())
        {
            return;
        } 
        if (upgradeCounts[upgradeName] >= shipTier){
            return;
        }   
        playerCrystals.RemoveCrystals(shipTier * 10);
        upgradeActions[upgradeName](shipTier);
        upgradeCounts[upgradeName]++;
        for (int i = 0; i < upgrades.transform.childCount; i++)
        {
            GameObject button = upgrades.transform.GetChild(i).gameObject;
            if (button.name == upgradeName)
            {
                UpdateUpgradeUI(button, upgradeName, upgradeCounts[upgradeName], shipTier);
                break;
            }
        }
    }

    public void ResetUpgradeUI()
    {
        GameObject upgrades = upgradeButtons ?? null;
        for (int i = 0; i < upgrades.transform.childCount; i++)
        {
            GameObject upgrade = upgrades.transform.GetChild(i).gameObject;
            Slider slider = upgrade.GetComponentInChildren<Slider>();
            Button upgradeButton = upgrade.GetComponentInChildren<Button>();
            if (slider == null || upgradeButton == null)
            {
                return;
            }
            slider.value = 0;
            upgradeButton.interactable = true;
        }
    }

    public void GetUpgradeType()
    {
        string btn = EventSystem.current.currentSelectedGameObject.name;
        GameObject parentButton = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        Upgrade(btn,parentButton);
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
        return playerCrystals.GetCrystalCount() >= shipTier * 10;
    }
}