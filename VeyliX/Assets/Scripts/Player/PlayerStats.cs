using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int mass = 100;
    private int[] health = { 80, 100 };
    private int[] healthRegen = { 4, 6 };
    private int[] energy = { 50, 70  };
    private int[] energyRegen = { 10, 15 };
    private int[] damage = { 8, 12 };
    private int[] bulletSpeed = { 80, 100 };
    private int[] speed = { 90/4, 120/4 };
    private int[] acceleration = { 60/2, 80/2 };
    private int[] rotationSpeed = { 80*6, 100*6 };

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public int GetMass() => mass;
    public int[] GetHealth() => health;
    public int[] GetHealthRegen() => healthRegen;
    public int[] GetEnergy() => energy;
    public int[] GetEnergyRegen() => energyRegen;
    public int[] GetDamage() => damage;
    public int[] GetBulletSpeed() => bulletSpeed;
    public int[] GetSpeed() => speed;
    public int[] GetAcceleration() => acceleration;
    public int[] GetRotationSpeed() => rotationSpeed;
}
