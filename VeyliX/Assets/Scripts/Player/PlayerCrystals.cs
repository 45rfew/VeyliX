using UnityEngine;
using System;

public class PlayerCrystals : MonoBehaviour
{
    public Action<int> OnplayerCrystalChange;
    private PlayerStats playerStats;
    private int crystalCount = 0;
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }
    public int GetCrystalCount() => crystalCount;

    public void AddCrystals(int value)
    {
        int maxCrystals = playerStats.GetMaxCrystals();
        crystalCount = Math.Min(maxCrystals, crystalCount + value);
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