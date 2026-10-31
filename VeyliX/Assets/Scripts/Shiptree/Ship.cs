using System.Collections.Generic;
using UnityEngine;

public class Ship
{
    public int code;
    public string name;
    public float zoom;
    public GameObject modelPrefab;

    public IntStatRange health;
    public IntStatRange healthRegen;

    public StatRange energy;
    public StatRange energyRegen;

    public StatRange speed;
    public StatRange acceleration;
    public StatRange rotationSpeed;
    public int mass;

    public List<Cannon> cannons;
    public List<Ship> upgrades;

    public Ship()
    {
        cannons = new List<Cannon>();
        upgrades = new List<Ship>();
    }
}