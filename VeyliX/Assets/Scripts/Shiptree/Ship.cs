using System.Collections.Generic;
using UnityEngine;

public class Ship
{
    public string name;

    public GameObject modelPrefab;

    public StatRange health;
    public StatRange healthRegen;

    public StatRange energy;
    public StatRange energyRegen;

    public StatRange speed;
    public StatRange acceleration;
    public StatRange rotationSpeed;
    public StatRange mass;

    public List<Cannon> cannons;
    public List<Ship> upgrades;

    public Ship()
    {
        cannons = new List<Cannon>();
        upgrades = new List<Ship>();
    }
}