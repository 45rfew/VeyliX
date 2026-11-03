using System;
using System.Collections.Generic;
using UnityEngine;
//hard code each ship zzz
public class PlayerStats : MonoBehaviour
{
    public event Action ShipChanged;

    public UpgradeButtons upgradeButtons;
    private PlayerEnergy playerEnergy;
    private PlayerHealth playerHealth;
    private Dictionary<int, Ship> ships = new Dictionary<int, Ship>();
    private Ship currentShip;
    private int currentShipCode;
    private PlayerShoot playerShoot;
    private PlayerLook playerLook;
    private PlayerMovement playerMovement;
    private int maxShipTier = 6;
    void Awake()
    {
        BuildShipTree();
        currentShip = ships[601];
        playerEnergy = GetComponent<PlayerEnergy>();
        playerHealth = GetComponent<PlayerHealth>();
        playerShoot = GetComponentInChildren<PlayerShoot>();
        playerLook = GetComponent<PlayerLook>();
        playerMovement = GetComponent<PlayerMovement>();
        upgradeButtons.ResetUpgradeUI();
    }
    public Ship GetCurrentShip()
    {
        return currentShip;
    }
    public void UpgradeTo(int ship)
    {
        currentShip = ships[ship];
        SetCurrentShipCode(ship);
        //playerShoot.ResetCannons();
        UpdateAllStats();
        upgradeButtons.ResetUpgradeCounts();
        upgradeButtons.ResetUpgradeUI();
        ShipChanged?.Invoke();
    }
    public int UpgradeCrystalCost()
    {
        return GetShipTier() * 10;
    }
    public int GetCurrentShipCode()
    {
        return currentShipCode;
    }
    public float GetCurrentZoom()
    {
        return GetCurrentShip().zoom;
    }
    public void SetCurrentShipCode(int code)
    {
        currentShipCode = code;
    }
    public int GetShipTier()
    {
        return Mathf.FloorToInt(currentShipCode / 100);
    }
    public int GetMaxShipTier()
    {
        return maxShipTier;
    }
    public int GetMaxCrystals()
    {
        return GetShipTier() * GetShipTier() * 25;
    }
    public void UpgradeHealth(int amount)
    {
        currentShip.health.Upgrade(amount);
        playerHealth.Upgrade();
    }
    public void UpgradeHealthRegen(int amount)
    {
        currentShip.healthRegen.Upgrade(amount);
        playerHealth.Upgrade();
    }
    public void UpgradeEnergy(int amount)
    {
        currentShip.energy.Upgrade(amount);
        playerEnergy.Upgrade();
    }
    public void UpgradeEnergyRegen(int amount)
    {
        currentShip.energyRegen.Upgrade(amount);
        playerEnergy.Upgrade();
    }
    public void UpgradeDamage(int amount)
    {
        for (int i = 0; i < currentShip.cannons.Count; i++){
            currentShip.cannons[i].damage.Upgrade(amount); 
        }
    }
    public void UpgradeBulletSpeed(int amount)
    {
        for (int i = 0; i < currentShip.cannons.Count; i++){
            currentShip.cannons[i].bulletSpeed.Upgrade(amount);
        }
    }
    public void UpgradeSpeed(int amount)
    {
        currentShip.speed.Upgrade(amount);
        playerMovement.UpdateMovementStats();
    }
    public void UpgradeAcceleration(int amount)
    {
        currentShip.acceleration.Upgrade(amount);
        playerMovement.UpdateMovementStats();
    }
    public void UpgradeRotationSpeed(int amount)
    {
        currentShip.rotationSpeed.Upgrade(amount);
        playerLook.UpdateRotationSpeed();
    }
    public void UpdateAllStats()
    {
        playerHealth.NewShip();
        playerEnergy.NewShip();
        playerMovement.UpdateMovementStats();
        playerLook.UpdateRotationSpeed();
    }
    void BuildShipTree()
    {
        Ship ship101 =
            CreateShip(
                code: 101,
                name: "VeyliX",
                zoom: 1f,
                healthMin: 1000,
                healthMax: 120,
                healthRegenMin: 4,
                healthRegenMax: 5,
                energyMin: 600,
                energyMax: 80,
                energyRegenMin: 1000,
                energyRegenMax: 15,
                speedMin: 16f,
                speedMax: 20f,
                accelerationMin: 90f,
                accelerationMax: 110f,
                rotationMin: 400f,
                rotationMax: 600f,
                mass: 1000,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 500, damageMax: 20, bulletSpeedMin: 45, bulletSpeedMax: 52, fireRateMin: 2f, fireRateMax: 2f, recoilValue: 10, error: 0));

        Ship ship201 = CreateShip(
            code: 201,
            name: "Excelsior",
            zoom: 1.05f,
            healthMin: 100,
            healthMax: 120,
            healthRegenMin: 4,
            healthRegenMax: 6,
            energyMin: 80,
            energyMax: 100,
            energyRegenMin: 15,
            energyRegenMax: 22,
            speedMin: 14f,
            speedMax: 18f,
            accelerationMin: 100f,
            accelerationMax: 125f,
            rotationMin: 300f,
            rotationMax: 400f,
            mass: 110,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 10, damageMax: 15, bulletSpeedMin: 30, bulletSpeedMax: 40, fireRateMin: 2f, fireRateMax: 2f, recoilValue: 15, error: 3));
        Ship ship202 = CreateShip(
            code: 202,
            name: "Avalon",
            zoom: 1.05f,
            healthMin: 100,
            healthMax: 110,
            healthRegenMin: 4,
            healthRegenMax: 7,
            energyMin: 60,
            energyMax: 100,
            energyRegenMin: 18,
            energyRegenMax: 24,
            speedMin: 13f,
            speedMax: 17f,
            accelerationMin: 80f,
            accelerationMax: 100f,
            rotationMin: 280f,
            rotationMax: 350f,
            mass: 110,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 15, damageMax: 25, bulletSpeedMin: 38, bulletSpeedMax: 50, fireRateMin: 1f, fireRateMax: 1.2f, recoilValue: 20, error: 0),
            new Cannon(name: "Side Cannon", priority: 1, damageMin: 6, damageMax: 8, bulletSpeedMin: 35, bulletSpeedMax: 40, fireRateMin: 3f, fireRateMax: 3f, recoilValue: 0, error: 2),
            new Cannon(name: "Side Cannon 2", priority: 1, damageMin: 6, damageMax: 8, bulletSpeedMin: 35, bulletSpeedMax: 40, fireRateMin: 3f, fireRateMax: 3f, recoilValue: 0, error: 2));

        Ship ship301 = CreateShip(
            code: 301,
            name: "Atreyu",
            zoom: 1.1f,
            healthMin: 120,
            healthMax: 160,
            healthRegenMin: 4,
            healthRegenMax: 7,
            energyMin: 80,
            energyMax: 110,
            energyRegenMin: 20,
            energyRegenMax: 30,
            speedMin: 11f,
            speedMax: 16f,
            accelerationMin: 70f,
            accelerationMax: 100f,
            rotationMin: 220f,
            rotationMax: 300f,
            mass: 140,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 30, damageMax: 40, bulletSpeedMin: 40, bulletSpeedMax: 50, fireRateMin: 1.2f, fireRateMax: 1.2f, recoilValue: 25, error: 0));
        Ship ship302 = CreateShip(
            code: 302,
            name: "Messiah",
            zoom: 1.1f,
            healthMin: 110,
            healthMax: 140,
            healthRegenMin: 5,
            healthRegenMax: 8,
            energyMin: 70,
            energyMax: 120,
            energyRegenMin: 28,
            energyRegenMax: 36,
            speedMin: 12f,
            speedMax: 18f,
            accelerationMin: 80f,
            accelerationMax: 110f,
            rotationMin: 240f,
            rotationMax: 400f,
            mass: 140,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 20, damageMax: 30, bulletSpeedMin: 38, bulletSpeedMax: 52, fireRateMin: 1.4f, fireRateMax: 1.4f, recoilValue: 0, error: 0),
            new Cannon(name: "Side Cannon", priority: 1, damageMin: 4, damageMax: 7, bulletSpeedMin: 28, bulletSpeedMax: 38, fireRateMin: 2.1f, fireRateMax: 2.1f, recoilValue: 0, error: 6),
            new Cannon(name: "Side Cannon 2", priority: 1, damageMin: 4, damageMax: 7, bulletSpeedMin: 28, bulletSpeedMax: 38, fireRateMin: 2.1f, fireRateMax: 2.1f, recoilValue: 0, error: 6));
        Ship ship303 = CreateShip(
            code: 303,
            name: "Daedalus",
            zoom: 1.1f,
            healthMin: 120,
            healthMax: 160,
            healthRegenMin: 4,
            healthRegenMax: 7,
            energyMin: 100,
            energyMax: 130,
            energyRegenMin: 20,
            energyRegenMax: 32,
            speedMin: 12f,
            speedMax: 16f,
            accelerationMin: 80f,
            accelerationMax: 100f,
            rotationMin: 300f,
            rotationMax: 400f,
            mass: 120,
            new Cannon(name: "Side Cannon", priority: 10, damageMin: 7, damageMax: 10, bulletSpeedMin: 30, bulletSpeedMax: 40, fireRateMin: 4f, fireRateMax: 4f, recoilValue: 0, error: 10),
            new Cannon(name: "Side Cannon 2", priority: 10, damageMin: 7, damageMax: 10, bulletSpeedMin: 30, bulletSpeedMax: 40, fireRateMin: 4f, fireRateMax: 4f, recoilValue: 0, error: 10));
        Ship ship304 = CreateShip(
            code: 304,
            name: "Ransu",
            zoom: 1.1f,
            healthMin: 110,
            healthMax: 150,
            healthRegenMin: 4,
            healthRegenMax: 8,
            energyMin: 80,
            energyMax: 100,
            energyRegenMin: 22,
            energyRegenMax: 30,
            speedMin: 13f,
            speedMax: 15.5f,
            accelerationMin: 80f,
            accelerationMax: 110f,
            rotationMin: 200f,
            rotationMax: 300f,
            mass: 140,
            new Cannon(name: "Main Cannon", priority: 1, damageMin: 5, damageMax: 10, bulletSpeedMin: 34, bulletSpeedMax: 40, fireRateMin: 3f, fireRateMax: 3f, recoilValue: 0, error: 10),
            new Cannon(name: "Side Cannon", priority: 10, damageMin: 20, damageMax: 35, bulletSpeedMin: 40, bulletSpeedMax: 54, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 30, error: 0),
            new Cannon(name: "Side Cannon 2", priority: 10, damageMin: 20, damageMax: 35, bulletSpeedMin: 40, bulletSpeedMax: 54, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 30, error: 0));

        Ship ship401 = CreateShip(
            code: 401,
            name: "Teraser",
            zoom: 1.2f,
            healthMin: 120,
            healthMax: 200,
            healthRegenMin: 5,
            healthRegenMax: 10,
            energyMin: 100,
            energyMax: 120,
            energyRegenMin: 22,
            energyRegenMax: 33,
            speedMin: 10f,
            speedMax: 12f,
            accelerationMin: 70f,
            accelerationMax: 90f,
            rotationMin: 220f,
            rotationMax: 280f,
            mass: 220,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 40, damageMax: 60, bulletSpeedMin: 45, bulletSpeedMax: 60, fireRateMin: 1.5f, fireRateMax: 1.5f, recoilValue: 40, error: 0),
            new Cannon(name: "Side Cannon", priority: 1, damageMin: 4, damageMax: 6, bulletSpeedMin: 45, bulletSpeedMax: 70, fireRateMin: 3f, fireRateMax: 3f, recoilValue: 0, error: 2),
            new Cannon(name: "Side Cannon 2", priority: 1, damageMin: 4, damageMax: 6, bulletSpeedMin: 45, bulletSpeedMax: 70, fireRateMin: 3f, fireRateMax: 3f, recoilValue: 0, error: 2));
        Ship ship402 = CreateShip(
            code: 402,
            name: "Sunder",
            zoom: 1.1f,
            healthMin: 130,
            healthMax: 220,
            healthRegenMin: 6,
            healthRegenMax: 10,
            energyMin: 120,
            energyMax: 140,
            energyRegenMin: 18,
            energyRegenMax: 30,
            speedMin: 12f,
            speedMax: 14f,
            accelerationMin: 100f,
            accelerationMax: 120f,
            rotationMin: 300f,
            rotationMax: 320f,
            mass: 200,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 40, damageMax: 55, bulletSpeedMin: 40, bulletSpeedMax: 50, fireRateMin: 1.8f, fireRateMax: 1.8f, recoilValue: 35, error: 0),
            new Cannon(name: "Main Cannon 2", priority: 10, damageMin: 40, damageMax: 55, bulletSpeedMin: 40, bulletSpeedMax: 50, fireRateMin: 1.8f, fireRateMax: 1.8f, recoilValue: 35, error: 0));
        Ship ship403 = CreateShip(
            code: 403,
            name: "Icarus",
            zoom: 1.1f,
            healthMin: 140,
            healthMax: 200,
            healthRegenMin: 5,
            healthRegenMax: 9,
            energyMin: 100,
            energyMax: 120,
            energyRegenMin: 15,
            energyRegenMax: 26,
            speedMin: 12f,
            speedMax: 15f,
            accelerationMin: 100f,
            accelerationMax: 130f,
            rotationMin: 220f,
            rotationMax: 350f,
            mass: 200,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 10, damageMax: 12, bulletSpeedMin: 34, bulletSpeedMax: 48, fireRateMin: 6f, fireRateMax: 6f, recoilValue: 0, error: 2),
            new Cannon(name: "Main Cannon 2", priority: 10, damageMin: 10, damageMax: 12, bulletSpeedMin: 34, bulletSpeedMax: 48, fireRateMin: 6f, fireRateMax: 6f, recoilValue: 0, error: 2));
        Ship ship404 = CreateShip(
            code: 404,
            name: "Calamity",
            zoom: 1.1f,
            healthMin: 140,
            healthMax: 220,
            healthRegenMin: 7,
            healthRegenMax: 11,
            energyMin: 100,
            energyMax: 140,
            energyRegenMin: 20,
            energyRegenMax: 30,
            speedMin: 12f,
            speedMax: 14f,
            accelerationMin: 90f,
            accelerationMax: 110f,
            rotationMin: 200f,
            rotationMax: 280f,
            mass: 240,
            new Cannon(name: "Left Cannon", priority: 10, damageMin: 20, damageMax: 28, bulletSpeedMin: 28, bulletSpeedMax: 44, fireRateMin: 1.5f, fireRateMax: 1.5f, recoilValue: 20, error: 0),
            new Cannon(name: "Left Cannon 2", priority: 1, damageMin: 4, damageMax: 8, bulletSpeedMin: 30, bulletSpeedMax: 48, fireRateMin: 3f, fireRateMax: 3f, recoilValue: 0, error: 3),
            new Cannon(name: "Left Cannon Inner", priority: 2, damageMin: 5, damageMax: 7, bulletSpeedMin: 42, bulletSpeedMax: 58, fireRateMin: 4f, fireRateMax: 4f, recoilValue: 0, error: 3),
            new Cannon(name: "Right Cannon", priority: 10, damageMin: 20, damageMax: 28, bulletSpeedMin: 28, bulletSpeedMax: 44, fireRateMin: 1.5f, fireRateMax: 1.5f, recoilValue: 20, error: 0),
            new Cannon(name: "Right Cannon 2", priority: 1, damageMin: 4, damageMax: 8, bulletSpeedMin: 30, bulletSpeedMax: 48, fireRateMin: 3f, fireRateMax: 3f, recoilValue: 0, error: 3),
            new Cannon(name: "Right Cannon Inner", priority: 2, damageMin: 5, damageMax: 7, bulletSpeedMin: 42, bulletSpeedMax: 58, fireRateMin: 4f, fireRateMax: 4f, recoilValue: 0, error: 3));

        Ship ship405 = CreateShip(
            code: 405,
            name: "Trishula",
            zoom: 1.1f,
            healthMin: 150,
            healthMax: 240,
            healthRegenMin: 7,
            healthRegenMax: 11,
            energyMin: 110,
            energyMax: 145,
            energyRegenMin: 22,
            energyRegenMax: 35,
            speedMin: 10f,
            speedMax: 13f,
            accelerationMin: 70f,
            accelerationMax: 100f,
            rotationMin: 180f,
            rotationMax: 240f,
            mass: 250,
            new Cannon(name: "Left Cannon", priority: 10, damageMin: 30, damageMax: 40, bulletSpeedMin: 30, bulletSpeedMax: 40, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 0, error: 0),
            new Cannon(name: "Left Cannon Small", priority: 1, damageMin: 6, damageMax: 8, bulletSpeedMin: 28, bulletSpeedMax: 35, fireRateMin: 6f, fireRateMax: 6f, recoilValue: 0, error: 4),
            new Cannon(name: "Right Cannon", priority: 10, damageMin: 30, damageMax: 40, bulletSpeedMin: 30, bulletSpeedMax: 40, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 0, error: 0),
            new Cannon(name: "Right Cannon Small", priority: 1, damageMin: 6, damageMax: 8, bulletSpeedMin: 28, bulletSpeedMax: 35, fireRateMin: 6f, fireRateMax: 6f, recoilValue: 0, error: 4));

        Ship ship501 = CreateShip(
            code: 501,
            name: "Excelion",
            zoom: 1.3f,
            healthMin: 280,
            healthMax: 400,
            healthRegenMin: 10,
            healthRegenMax: 14,
            energyMin: 120,
            energyMax: 160,
            energyRegenMin: 35,
            energyRegenMax: 50,
            speedMin: 10f,
            speedMax: 15f,
            accelerationMin: 85f,
            accelerationMax: 110f,
            rotationMin: 220f,
            rotationMax: 330f,
            mass: 300,
            new Cannon(name: "Middle Cannon", priority: 10, damageMin: 100, damageMax: 120, bulletSpeedMin: 35, bulletSpeedMax: 50, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 25, error: 0),
            new Cannon(name: "Left Cannon", priority: 1, damageMin: 4, damageMax: 6, bulletSpeedMin: 25, bulletSpeedMax: 32, fireRateMin: 1.5f, fireRateMax: 1.5f, recoilValue: 0, error: 5),
            new Cannon(name: "Right Cannon", priority: 1, damageMin: 4, damageMax: 6, bulletSpeedMin: 25, bulletSpeedMax: 32, fireRateMin: 1.5f, fireRateMax: 1.5f, recoilValue: 0, error: 5));
        Ship ship502 = CreateShip(
            code: 502,
            name: "Fergus",
            zoom: 1.3f,
            healthMin: 250,
            healthMax: 420,
            healthRegenMin: 10,
            healthRegenMax: 14,
            energyMin: 130,
            energyMax: 200,
            energyRegenMin: 35,
            energyRegenMax: 45,
            speedMin: 10f,
            speedMax: 14f,
            accelerationMin: 90f,
            accelerationMax: 130f,
            rotationMin: 200f,
            rotationMax: 300f,
            mass: 350,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 50, damageMax: 70, bulletSpeedMin: 35, bulletSpeedMax: 55, fireRateMin: 1.5f, fireRateMax: 1.5f, recoilValue: 0, error: 0),
            new Cannon(name: "Side Cannon", priority: 1, damageMin: 6, damageMax: 7, bulletSpeedMin: 33, bulletSpeedMax: 40, fireRateMin: 2.5f, fireRateMax: 2.5f, recoilValue: 0, error: 1),
            new Cannon(name: "Side Cannon", priority: 1, damageMin: 6, damageMax: 7, bulletSpeedMin: 33, bulletSpeedMax: 40, fireRateMin: 2.5f, fireRateMax: 2.5f, recoilValue: 0, error: 1));
        Ship ship503 = CreateShip(
            code: 503,
            name: "Seyfork",
            zoom: 1.3f,
            healthMin: 220,
            healthMax: 400,
            healthRegenMin: 10,
            healthRegenMax: 14,
            energyMin: 130,
            energyMax: 180,
            energyRegenMin: 25,
            energyRegenMax: 45,
            speedMin: 10f,
            speedMax: 14f,
            accelerationMin: 100f,
            accelerationMax: 120f,
            rotationMin: 200f,
            rotationMax: 320f,
            mass: 300,
            new Cannon(name: "Left Cannon", priority: 10, damageMin: 20, damageMax: 25, bulletSpeedMin: 35, bulletSpeedMax: 48, fireRateMin: 2f, fireRateMax: 2f, recoilValue: 0, error: 3),
            new Cannon(name: "Left Cannon 2", priority: 1, damageMin: 10, damageMax: 12, bulletSpeedMin: 30, bulletSpeedMax: 52, fireRateMin: 4f, fireRateMax: 4f, recoilValue: 0, error: 3),
            new Cannon(name: "Right Cannon", priority: 10, damageMin: 20, damageMax: 25, bulletSpeedMin: 35, bulletSpeedMax: 48, fireRateMin: 2f, fireRateMax: 2f, recoilValue: 0, error: 3),
            new Cannon(name: "Right Cannon 2", priority: 1, damageMin: 10, damageMax: 12, bulletSpeedMin: 30, bulletSpeedMax: 52, fireRateMin: 4f, fireRateMax: 4f, recoilValue: 0, error: 3));
        Ship ship504 = CreateShip(
            code: 504,
            name: "Cyborg",
            zoom: 1.3f,
            healthMin: 240,
            healthMax: 400,
            healthRegenMin: 10,
            healthRegenMax: 16,
            energyMin: 130,
            energyMax: 200,
            energyRegenMin: 30,
            energyRegenMax: 55,
            speedMin: 10f,
            speedMax: 11f,
            accelerationMin: 80f,
            accelerationMax: 100f,
            rotationMin: 160f,
            rotationMax: 200f,
            mass: 400,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 100, damageMax: 150, bulletSpeedMin: 30, bulletSpeedMax: 55, fireRateMin: 2f, fireRateMax: 2f, recoilValue: 20, error: 0),
            new Cannon(name: "Main Cannon 2", priority: 1, damageMin: 10, damageMax: 15, bulletSpeedMin: 20, bulletSpeedMax: 45, fireRateMin: 3f, fireRateMax: 3f, recoilValue: 0, error: 4));
        Ship ship505 = CreateShip(
            code: 505,
            name: "Armityle",
            zoom: 1.3f,
            healthMin: 350,
            healthMax: 500,
            healthRegenMin: 10,
            healthRegenMax: 16,
            energyMin: 100,
            energyMax: 160,
            energyRegenMin: 30,
            energyRegenMax: 40,
            speedMin: 10f,
            speedMax: 13f,
            accelerationMin: 100f,
            accelerationMax: 140f,
            rotationMin: 200f,
            rotationMax: 280f,
            mass: 500,
            new Cannon(name: "Main Cannon", priority: 1, damageMin: 5, damageMax: 7, bulletSpeedMin: 42, bulletSpeedMax: 55, fireRateMin: 8f, fireRateMax: 8f, recoilValue: 0, error: 2),
            new Cannon(name: "Main Cannon 2", priority: 1, damageMin: 5, damageMax: 7, bulletSpeedMin: 42, bulletSpeedMax: 55, fireRateMin: 8f, fireRateMax: 8f, recoilValue: 0, error: 2));
        Ship ship506 = CreateShip(
            code: 506,
            name: "Goliath",
            zoom: 1.4f,
            healthMin: 300,
            healthMax: 350,
            healthRegenMin: 10,
            healthRegenMax: 14,
            energyMin: 100,
            energyMax: 150,
            energyRegenMin: 40,
            energyRegenMax: 65,
            speedMin: 8f,
            speedMax: 12f,
            accelerationMin: 70f,
            accelerationMax: 120f,
            rotationMin: 200f,
            rotationMax: 300f,
            mass: 350,
            new Cannon(name: "Main Cannon", priority: 1, damageMin: 40, damageMax: 60, bulletSpeedMin: 40, bulletSpeedMax: 55, fireRateMin: 2f, fireRateMax: 2f, recoilValue: 50, error: 0),
            new Cannon(name: "Main Cannon", priority: 1, damageMin: 40, damageMax: 60, bulletSpeedMin: 40, bulletSpeedMax: 55, fireRateMin: 2f, fireRateMax: 2f, recoilValue: 50, error: 0));

        Ship ship601 = CreateShip(
            code: 601,
            name: "Epiloguer",
            zoom: 1.7f,
            healthMin: 300,
            healthMax: 400,
            healthRegenMin: 4,
            healthRegenMax: 20,
            energyMin: 300,
            energyMax: 330,
            energyRegenMin: 150,
            energyRegenMax: 200,
            speedMin: 17f,
            speedMax: 22f,
            accelerationMin: 60f,
            accelerationMax: 80f,
            rotationMin: 350f,
            rotationMax: 400f,
            mass: 400,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 250, damageMax: 300, bulletSpeedMin: 50, bulletSpeedMax: 60, fireRateMin: 2f, fireRateMax: 2f, recoilValue: 250, error: 0),
            new Cannon(name: "Main Cannon", priority: 1, damageMin: 25, damageMax: 30, bulletSpeedMin: 30, bulletSpeedMax: 35, fireRateMin: 3f, fireRateMax: 3f, recoilValue: 0, error: 0),
            new Cannon(name: "Main Cannon", priority: 1, damageMin: 25, damageMax: 30, bulletSpeedMin: 30, bulletSpeedMax: 35, fireRateMin: 3f, fireRateMax: 3f, recoilValue: 0, error: 0));

        Ship ship602 = CreateShip(
            code: 602,
            name: "Raigeki",
            zoom: 1.5f,
            healthMin: 460,
            healthMax: 560,
            healthRegenMin: 24,
            healthRegenMax: 36,
            energyMin: 1400,
            energyMax: 2000,
            energyRegenMin: 800,
            energyRegenMax: 1000,
            speedMin: 12f,
            speedMax: 14f,
            accelerationMin: 80f,
            accelerationMax: 100f,
            rotationMin: 160f,
            rotationMax: 200f,
            mass: 420,
            new Cannon(name: "Left Cannon", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Left Cannon 2", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Left Cannon 3", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Left Cannon 4", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Left Cannon 5", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Left Cannon 6", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Left Cannon 7", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Left Cannon 8", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Left Cannon 9", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Left Cannon 10", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Right Cannon", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Right Cannon 2", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Right Cannon 3", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Right Cannon 4", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Right Cannon 5", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Right Cannon 6", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Right Cannon 7", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Right Cannon 8", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Right Cannon 9", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0),
            new Cannon(name: "Right Cannon 10", priority: 10, damageMin: 70, damageMax: 100, bulletSpeedMin: 10, bulletSpeedMax: 10, fireRateMin: 0.3f, fireRateMax: 0.3f, recoilValue: 10, error: 0));

        Ship ship603 = CreateShip(
            code: 603,
            name: "Vuaniha",
            zoom: 1.5f,
            healthMin: 700,
            healthMax: 770,
            healthRegenMin: 20,
            healthRegenMax: 30,
            energyMin: 400,
            energyMax: 620,
            energyRegenMin: 180,
            energyRegenMax: 250,
            speedMin: 12.5f,
            speedMax: 15f,
            accelerationMin: 60f,
            accelerationMax: 80f,
            rotationMin: 160f,
            rotationMax: 200f,
            mass: 500,
            new Cannon(name: "Left Cannon", priority: 10, damageMin: 20, damageMax: 30, bulletSpeedMin: 36, bulletSpeedMax: 45, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 8, error: 3),
            new Cannon(name: "Left Cannon 2", priority: 10, damageMin: 20, damageMax: 30, bulletSpeedMin: 36, bulletSpeedMax: 45, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 8, error: 3),
            new Cannon(name: "Left Cannon 3", priority: 10, damageMin: 20, damageMax: 30, bulletSpeedMin: 36, bulletSpeedMax: 45, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 8, error: 3),
            new Cannon(name: "Left Cannon 4", priority: 10, damageMin: 20, damageMax: 30, bulletSpeedMin: 36, bulletSpeedMax: 45, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 8, error: 3),
            new Cannon(name: "Left Cannon 5", priority: 10, damageMin: 20, damageMax: 30, bulletSpeedMin: 36, bulletSpeedMax: 45, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 8, error: 3),
            new Cannon(name: "Left Cannon 6", priority: 10, damageMin: 20, damageMax: 30, bulletSpeedMin: 36, bulletSpeedMax: 45, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 8, error: 3),
            new Cannon(name: "Left Cannon 7", priority: 10, damageMin: 20, damageMax: 30, bulletSpeedMin: 36, bulletSpeedMax: 45, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 8, error: 3),
            new Cannon(name: "Right Cannon", priority: 10, damageMin: 20, damageMax: 30, bulletSpeedMin: 36, bulletSpeedMax: 45, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 8, error: 3),
            new Cannon(name: "Right Cannon 2", priority: 10, damageMin: 20, damageMax: 30, bulletSpeedMin: 36, bulletSpeedMax: 45, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 8, error: 3),
            new Cannon(name: "Right Cannon 3", priority: 10, damageMin: 20, damageMax: 30, bulletSpeedMin: 36, bulletSpeedMax: 45, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 8, error: 3),
            new Cannon(name: "Right Cannon 4", priority: 10, damageMin: 20, damageMax: 30, bulletSpeedMin: 36, bulletSpeedMax: 45, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 8, error: 3),
            new Cannon(name: "Right Cannon 5", priority: 10, damageMin: 20, damageMax: 30, bulletSpeedMin: 36, bulletSpeedMax: 45, fireRateMin: 1, fireRateMax: 1f, recoilValue: 8, error: 3),
            new Cannon(name: "Right Cannon 6", priority: 10, damageMin: 20, damageMax: 30, bulletSpeedMin: 36, bulletSpeedMax: 45, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 8, error: 3),
            new Cannon(name: "Right Cannon 7", priority: 10, damageMin: 20, damageMax: 30, bulletSpeedMin: 36, bulletSpeedMax: 45, fireRateMin: 1f, fireRateMax: 1f, recoilValue: 8, error: 3));

        Ship ship604 = CreateShip(
            code: 604,
            name: "Tiaramisu",
            zoom: 1.5f,
            healthMin: 800,
            healthMax: 900,
            healthRegenMin: 25,
            healthRegenMax: 30,
            energyMin: 500,
            energyMax: 700,
            energyRegenMin: 240,
            energyRegenMax: 280,
            speedMin: 11f,
            speedMax: 14f,
            accelerationMin: 70f,
            accelerationMax: 90f,
            rotationMin: 120f,
            rotationMax: 200f,
            mass: 700,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 100, damageMax: 140, bulletSpeedMin: 50, bulletSpeedMax: 60, fireRateMin: 2f, fireRateMax: 2f, recoilValue: 25, error: 0),
            new Cannon(name: "Top Cannon", priority: 5, damageMin: 40, damageMax: 50, bulletSpeedMin: 35, bulletSpeedMax: 42, fireRateMin: 4.5f, fireRateMax: 4.5f, recoilValue: 0, error: 3),
            new Cannon(name: "Left Cannon", priority: 1, damageMin: 15, damageMax: 20, bulletSpeedMin: 30, bulletSpeedMax: 35, fireRateMin: 0.5f, fireRateMax: 0.5f, recoilValue: 0, error: 0.5f),
            new Cannon(name: "Right Cannon", priority: 1, damageMin: 15, damageMax: 20, bulletSpeedMin: 30, bulletSpeedMax: 35, fireRateMin: 0.5f, fireRateMax: 0.5f, recoilValue: 0, error: 0.5f),
            new Cannon(name: "Left Cannon 2", priority: 1, damageMin: 15, damageMax: 20, bulletSpeedMin: 30, bulletSpeedMax: 35, fireRateMin: 0.5f, fireRateMax: 0.5f, recoilValue: 0, error: 0.5f),
            new Cannon(name: "Left Cannon 3", priority: 1, damageMin: 15, damageMax: 20, bulletSpeedMin: 30, bulletSpeedMax: 35, fireRateMin: 0.5f, fireRateMax: 0.5f, recoilValue: 0, error: 0.5f),
            new Cannon(name: "Left Cannon 4", priority: 1, damageMin: 15, damageMax: 20, bulletSpeedMin: 30, bulletSpeedMax: 35, fireRateMin: 0.5f, fireRateMax: 0.5f, recoilValue: 0, error: 0.5f),
            new Cannon(name: "Right Cannon 2", priority: 1, damageMin: 15, damageMax: 20, bulletSpeedMin: 30, bulletSpeedMax: 35, fireRateMin: 0.5f, fireRateMax: 0.5f, recoilValue: 0, error: 0.5f),
            new Cannon(name: "Right Cannon 3", priority: 1, damageMin: 15, damageMax: 20, bulletSpeedMin: 30, bulletSpeedMax: 35, fireRateMin: 0.5f, fireRateMax: 0.5f, recoilValue: 0, error: 0.5f),
            new Cannon(name: "Right Cannon 4", priority: 1, damageMin: 15, damageMax: 20, bulletSpeedMin: 30, bulletSpeedMax: 35, fireRateMin: 0.5f, fireRateMax: 0.5f, recoilValue: 0, error: 0.5f));

        Ship ship605 = CreateShip(
            code: 605,
            name: "Tsimtsum",
            zoom: 1.5f,
            healthMin: 700,
            healthMax: 820,
            healthRegenMin: 24,
            healthRegenMax: 32,
            energyMin: 450,
            energyMax: 500,
            energyRegenMin: 120,
            energyRegenMax: 150,
            speedMin: 10f,
            speedMax: 13f,
            accelerationMin: 70f,
            accelerationMax: 95f,
            rotationMin: 180f,
            rotationMax: 240f,
            mass: 700,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 80, damageMax: 100, bulletSpeedMin: 40, bulletSpeedMax: 55, fireRateMin: 2f, fireRateMax: 2f, recoilValue: 50, error: 0));
        Ship ship606 = CreateShip(
            code: 606,
            name: "Pusat",
            zoom: 1.5f,
            healthMin: 800,
            healthMax: 940,
            healthRegenMin: 20,
            healthRegenMax: 26,
            energyMin: 600,
            energyMax: 800,
            energyRegenMin: 160,
            energyRegenMax: 220,
            speedMin: 10f,
            speedMax: 12f,
            accelerationMin: 60f,
            accelerationMax: 80f,
            rotationMin: 120f,
            rotationMax: 180f,
            mass: 720,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 80, damageMax: 120, bulletSpeedMin: 15, bulletSpeedMax: 20, fireRateMin: 1.2f, fireRateMax: 1.2f, recoilValue: 15, error: 0),
            new Cannon(name: "Main Cannon 2", priority: 10, damageMin: 80, damageMax: 120, bulletSpeedMin: 15, bulletSpeedMax: 20, fireRateMin: 1.2f, fireRateMax: 1.2f, recoilValue: 15, error: 0),
            new Cannon(name: "Side Cannon", priority: 2, damageMin: 20, damageMax: 24, bulletSpeedMin: 25, bulletSpeedMax: 30, fireRateMin: 1.6f, fireRateMax: 1.6f, recoilValue: 0, error: 1),
            new Cannon(name: "Side Cannon 2", priority: 2, damageMin: 20, damageMax: 24, bulletSpeedMin: 25, bulletSpeedMax: 30, fireRateMin: 1.6f, fireRateMax: 1.6f, recoilValue: 0, error: 1),
            new Cannon(name: "Back Cannon", priority: 1, damageMin: 16, damageMax: 20, bulletSpeedMin: 30, bulletSpeedMax: 38, fireRateMin: 1.2f, fireRateMax: 1.2f, recoilValue: 0, error: 1),
            new Cannon(name: "Back Cannon 2", priority: 1, damageMin: 16, damageMax: 20, bulletSpeedMin: 30, bulletSpeedMax: 38, fireRateMin: 1.2f, fireRateMax: 1.2f, recoilValue: 0, error: 1),
            new Cannon(name: "Back Cannon 3", priority: 1, damageMin: 16, damageMax: 20, bulletSpeedMin: 30, bulletSpeedMax: 38, fireRateMin: 1.2f, fireRateMax: 1.2f, recoilValue: 0, error: 1),
            new Cannon(name: "Back Cannon 4", priority: 1, damageMin: 16, damageMax: 20, bulletSpeedMin: 30, bulletSpeedMax: 38, fireRateMin: 1.2f, fireRateMax: 1.2f, recoilValue: 0, error: 1),
            new Cannon(name: "Back Cannon 5", priority: 1, damageMin: 16, damageMax: 20, bulletSpeedMin: 30, bulletSpeedMax: 38, fireRateMin: 1.2f, fireRateMax: 1.2f, recoilValue: 0, error: 1),
            new Cannon(name: "Back Cannon 6", priority: 1, damageMin: 16, damageMax: 20, bulletSpeedMin: 30, bulletSpeedMax: 38, fireRateMin: 1.2f, fireRateMax: 1.2f, recoilValue: 0, error: 1),
            new Cannon(name: "Back Cannon 7", priority: 1, damageMin: 16, damageMax: 20, bulletSpeedMin: 30, bulletSpeedMax: 38, fireRateMin: 1.2f, fireRateMax: 1.2f, recoilValue: 0, error: 1),
            new Cannon(name: "Back Cannon 8", priority: 1, damageMin: 16, damageMax: 20, bulletSpeedMin: 30, bulletSpeedMax: 38, fireRateMin: 1.2f, fireRateMax: 1.2f, recoilValue: 0, error: 1));
        
        
        ship101.upgrades.Add(ship201);
        ship101.upgrades.Add(ship202);

        ship201.upgrades.Add(ship301);
        ship201.upgrades.Add(ship302);
        ship202.upgrades.Add(ship303);
        ship202.upgrades.Add(ship304);

        ship301.upgrades.Add(ship401);
        ship301.upgrades.Add(ship402);
        ship302.upgrades.Add(ship402);
        ship302.upgrades.Add(ship403);
        ship303.upgrades.Add(ship403);
        ship303.upgrades.Add(ship404);
        ship304.upgrades.Add(ship404);
        ship304.upgrades.Add(ship405);

        ship401.upgrades.Add(ship501);
        ship401.upgrades.Add(ship502);
        ship402.upgrades.Add(ship502);
        ship402.upgrades.Add(ship503);
        ship403.upgrades.Add(ship503);
        ship403.upgrades.Add(ship504);
        ship404.upgrades.Add(ship504);
        ship404.upgrades.Add(ship505);
        ship405.upgrades.Add(ship505);
        ship405.upgrades.Add(ship506);

        ship501.upgrades.Add(ship601);
        ship501.upgrades.Add(ship603);
        ship502.upgrades.Add(ship602);
        ship502.upgrades.Add(ship603);
        ship503.upgrades.Add(ship603);
        ship503.upgrades.Add(ship602);
        ship504.upgrades.Add(ship603);
        ship504.upgrades.Add(ship604);
        ship505.upgrades.Add(ship604);
        ship505.upgrades.Add(ship605);
        ship506.upgrades.Add(ship605);
        ship506.upgrades.Add(ship606);
    }

    private Ship CreateShip(
        int code,
        string name,
        float zoom,
        int healthMin,
        int healthMax,
        int healthRegenMin,
        int healthRegenMax,
        int energyMin,
        int energyMax,
        int energyRegenMin,
        int energyRegenMax,
        float speedMin,
        float speedMax,
        float accelerationMin,
        float accelerationMax,
        float rotationMin,
        float rotationMax,
        int mass,
        params Cannon[] cannons)
    {
        Ship ship = new Ship
        {
            code = code,
            name = name,
            zoom = zoom,
            health = new IntStatRange(healthMin, healthMax),
            healthRegen = new IntStatRange(healthRegenMin, healthRegenMax),
            energy = new StatRange(energyMin, energyMax),
            energyRegen = new StatRange(energyRegenMin, energyRegenMax),
            speed = new StatRange(speedMin, speedMax),
            acceleration = new StatRange(accelerationMin, accelerationMax),
            rotationSpeed = new StatRange(rotationMin, rotationMax),
            mass = mass,
            cannons = new List<Cannon>(cannons)
        };
        ships.Add(code, ship);
        return ship;
    }
}