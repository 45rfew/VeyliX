using UnityEngine;
using System;

public class PlayerCrystals : MonoBehaviour
{
    public Action<float> OnplayerCrystalChange;
    public PlayerStats playerStats;
    private int crystalCount = 0;

    public int GetCrystalCount() => crystalCount;

    public void AddCrystals(float value)
    {
        float maxCrystals = playerStats.GetMaxCrystals();
        if (crystalCount + Mathf.FloorToInt(value) > maxCrystals)
        {
            crystalCount = Mathf.FloorToInt(maxCrystals);
        } else
        {
            crystalCount += Mathf.FloorToInt(value);
        }
        OnplayerCrystalChange?.Invoke(crystalCount);

        Debug.Log("Player Crystals: " + crystalCount);
    }

    public void RemoveCrystals(int value)
    {
        crystalCount -= value;

        if (crystalCount < 0) crystalCount = 0;

        OnplayerCrystalChange?.Invoke(crystalCount);

        Debug.Log("Player Crystals: " + crystalCount);
    }
}