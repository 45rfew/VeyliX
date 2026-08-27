using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    private PlayerStats playerStats;
    private float currentEnergy;
    private float maxEnergy;
    private float energyRegen;
    
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        maxEnergy = playerStats.GetEnergy()[0];
        currentEnergy = maxEnergy;
        energyRegen = playerStats.GetEnergyRegen()[0];
    }

    void Update()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += energyRegen * Time.deltaTime;
        }
    }
    public void SetEnergy(float value)
    {
        currentEnergy -= value;
    }
    public void SetMaxEnergy(float value)
    {
        maxEnergy = value;
    }
    public void SetEnergyRegen(float value)
    {
        energyRegen = value;
    }
    public float GetEnergy() => currentEnergy;
}
