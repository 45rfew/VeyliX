using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int mass = 100;
    private int[] health = { 80, 100 };
    private int[] healthRegen = { 4, 6 };
    private int[] energy = { 50, 70  };
    private int[] energyRegen = { 10, 15 };
    public int[] damage = { 80, 12 };
    private int[] bulletSpeed = { 80/5, 100/5 };
    private int[] speed = { 90/6, 115/6 };
    private int[] acceleration = { 90, 140 };
    private int[] rotationSpeed = { 60 * 5, 80 * 5 };
    public float fireRate = 2f;
    public float recoil = 15f;

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
