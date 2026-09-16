using System;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public Action<float, float> OnEnergyRegen;
    private PlayerStats playerStats;
    private float currentEnergy = 0;
    private float maxEnergy;
    private float energyRegen;
    
    private void Start() {
        NewShip();
    }

    void Update()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += energyRegen * Time.deltaTime;
            OnEnergyRegen?.Invoke(currentEnergy,maxEnergy);
        }
    }
    public void SetEnergy(float value)
    {
        currentEnergy -= value;
        OnEnergyRegen?.Invoke(currentEnergy,maxEnergy);
    }
    public void SetMaxEnergy(float value)
    {
        maxEnergy = value;
        OnEnergyRegen?.Invoke(currentEnergy,maxEnergy);
    }
    public void SetEnergyRegen(float value)
    {
        energyRegen = value;
    }
    public float GetEnergy() => currentEnergy;
    public void NewShip()
    {
        playerStats = GetComponent<PlayerStats>();
        Ship ship = playerStats.GetCurrentShip();
        maxEnergy = ship.energy.GetValue();
        currentEnergy = maxEnergy;
        energyRegen = ship.energyRegen.GetValue();
        OnEnergyRegen?.Invoke(currentEnergy, maxEnergy);
    }
    public void Upgrade()
    {
        playerStats = GetComponent<PlayerStats>();
        Ship ship = playerStats.GetCurrentShip();
        maxEnergy = ship.energy.GetValue();
        energyRegen = ship.energyRegen.GetValue();
        OnEnergyRegen?.Invoke(currentEnergy, maxEnergy);
    }
}
