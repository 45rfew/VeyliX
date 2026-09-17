using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShipStats
{
    public string shipName;
    public string description;
    public StatRange speed;
    public StatRange acceleration;
    public StatRange rotationSpeed;
    public int mass;
    public IntStatRange health;
    public IntStatRange healthRegen;
    public StatRange energy;
    public StatRange energyRegen;
    public List<Cannon> cannons = new List<Cannon>();
    public List<int> upgradesTo = new List<int>();
}