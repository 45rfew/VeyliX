using System;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public Action<float, float> OnEnergyRegen;
    private PlayerStats playerStats;
    private float currentEnergy = 0;
    private float maxEnergy;
    private float energyRegen;
    
    private void Awake() {    
        playerStats = GetComponent<PlayerStats>();
        maxEnergy = playerStats.GetEnergy()[0];
        currentEnergy = maxEnergy;
        energyRegen = playerStats.GetEnergyRegen()[0];
        OnEnergyRegen?.Invoke(currentEnergy, maxEnergy);
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
}
