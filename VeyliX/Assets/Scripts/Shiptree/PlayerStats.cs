using System.Collections.Generic;
using UnityEngine;
//hard code each ship zzz
public class PlayerStats : MonoBehaviour
{
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
        currentShip = ships[101];
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
                healthMin: 80,
                healthMax: 100,
                healthRegenMin: 4,
                healthRegenMax: 6,
                energyMin: 50,
                energyMax: 70,
                energyRegenMin: 10,
                energyRegenMax: 15,
                speedMin: 45f,
                speedMax: 60f,
                accelerationMin: 60f,
                accelerationMax: 80f,
                rotationMin: 400f,
                rotationMax: 600f,
                mass: 100,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 20, damageMax: 35, bulletSpeedMin: 30, bulletSpeedMax: 40, fireRateMin: 2f, fireRateMax: 2.4f, recoilValue: 20));

        Ship ship201 = CreateShip(
            code: 201,
            name: "Excelsior",
            zoom: 1.1f,
            healthMin: 130,
            healthMax: 160,
            healthRegenMin: 26,
            healthRegenMax: 40,
            energyMin: 60,
            energyMax: 90,
            energyRegenMin: 2,
            energyRegenMax: 4,
            speedMin: 11f,
            speedMax: 13f,
            accelerationMin: 100f,
            accelerationMax: 125f,
            rotationMin: 100f,
            rotationMax: 120f,
            mass: 85,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 35, damageMax: 45, bulletSpeedMin: 40, bulletSpeedMax: 50, fireRateMin: 1.3f, fireRateMax: 1.6f, recoilValue: 15));
        Ship ship202 = CreateShip(
            code: 202,
            name: "Avalon",
            zoom: 1.1f,
            healthMin: 140,
            healthMax: 185,
            healthRegenMin: 20,
            healthRegenMax: 37,
            energyMin: 60,
            energyMax: 90,
            energyRegenMin: 2,
            energyRegenMax: 4,
            speedMin: 11f,
            speedMax: 13f,
            accelerationMin: 80f,
            accelerationMax: 100f,
            rotationMin: 70f,
            rotationMax: 85f,
            mass: 95,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 40, damageMax: 55, bulletSpeedMin: 35, bulletSpeedMax: 45, fireRateMin: 1f, fireRateMax: 1.2f, recoilValue: 20));

        Ship ship301 = CreateShip(
            code: 301,
            name: "Atreyu",
            zoom: 1.2f,
            healthMin: 160,
            healthMax: 195,
            healthRegenMin: 32,
            healthRegenMax: 65,
            energyMin: 70,
            energyMax: 110,
            energyRegenMin: 2,
            energyRegenMax: 4,
            speedMin: 10f,
            speedMax: 12f,
            accelerationMin: 100f,
            accelerationMax: 130f,
            rotationMin: 50f,
            rotationMax: 70f,
            mass: 90,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 45, damageMax: 60, bulletSpeedMin: 40, bulletSpeedMax: 55, fireRateMin: 1.6f, fireRateMax: 1.9f, recoilValue: 15));
        Ship ship302 = CreateShip(
            code: 302,
            name: "Messiah",
            zoom: 1.2f,
            healthMin: 170,
            healthMax: 210,
            healthRegenMin: 28,
            healthRegenMax: 70,
            energyMin: 70,
            energyMax: 100,
            energyRegenMin: 2,
            energyRegenMax: 3,
            speedMin: 10.5f,
            speedMax: 12f,
            accelerationMin: 80f,
            accelerationMax: 100f,
            rotationMin: 60f,
            rotationMax: 80f,
            mass: 110,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 50, damageMax: 65, bulletSpeedMin: 38, bulletSpeedMax: 52, fireRateMin: 1.4f, fireRateMax: 1.7f, recoilValue: 20));
        Ship ship303 = CreateShip(
            code: 303,
            name: "Daedalus",
            zoom: 1.2f,
            healthMin: 180,
            healthMax: 210,
            healthRegenMin: 26,
            healthRegenMax: 53,
            energyMin: 100,
            energyMax: 130,
            energyRegenMin: 2,
            energyRegenMax: 4,
            speedMin: 9f,
            speedMax: 11f,
            accelerationMin: 80f,
            accelerationMax: 100f,
            rotationMin: 60f,
            rotationMax: 75f,
            mass: 140,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 55, damageMax: 70, bulletSpeedMin: 45, bulletSpeedMax: 60, fireRateMin: 1.2f, fireRateMax: 1.5f, recoilValue: 30));
        Ship ship304 = CreateShip(
            code: 304,
            name: "Ransu",
            zoom: 1.2f,
            healthMin: 195,
            healthMax: 230,
            healthRegenMin: 24,
            healthRegenMax: 46,
            energyMin: 80,
            energyMax: 110,
            energyRegenMin: 2,
            energyRegenMax: 3,
            speedMin: 8f,
            speedMax: 10f,
            accelerationMin: 65f,
            accelerationMax: 100f,
            rotationMin: 40f,
            rotationMax: 70f,
            mass: 220,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 60, damageMax: 75, bulletSpeedMin: 38, bulletSpeedMax: 52, fireRateMin: 1f, fireRateMax: 1.3f, recoilValue: 30));

        Ship ship401 = CreateShip(
            code: 401,
            name: "Teraser",
            zoom: 1.4f,
            healthMin: 170,
            healthMax: 230,
            healthRegenMin: 34,
            healthRegenMax: 58,
            energyMin: 100,
            energyMax: 170,
            energyRegenMin: 2,
            energyRegenMax: 4,
            speedMin: 8f,
            speedMax: 10.5f,
            accelerationMin: 100f,
            accelerationMax: 120f,
            rotationMin: 90f,
            rotationMax: 110f,
            mass: 200,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 60, damageMax: 75, bulletSpeedMin: 45, bulletSpeedMax: 60, fireRateMin: 1.2f, fireRateMax: 1.5f, recoilValue: 20));
        Ship ship402 = CreateShip(
            code: 402,
            name: "Sunder",
            zoom: 1.4f,
            healthMin: 220,
            healthMax: 260,
            healthRegenMin: 31,
            healthRegenMax: 52,
            energyMin: 160,
            energyMax: 220,
            energyRegenMin: 1,
            energyRegenMax: 2,
            speedMin: 7f,
            speedMax: 9.5f,
            accelerationMin: 100f,
            accelerationMax: 120f,
            rotationMin: 40f,
            rotationMax: 55f,
            mass: 250,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 70, damageMax: 85, bulletSpeedMin: 40, bulletSpeedMax: 55, fireRateMin: 1f, fireRateMax: 1.2f, recoilValue: 35));
        Ship ship403 = CreateShip(
            code: 403,
            name: "Icarus",
            zoom: 1.4f,
            healthMin: 170,
            healthMax: 200,
            healthRegenMin: 28,
            healthRegenMax: 50,
            energyMin: 100,
            energyMax: 160,
            energyRegenMin: 1,
            energyRegenMax: 3,
            speedMin: 9f,
            speedMax: 11f,
            accelerationMin: 70f,
            accelerationMax: 90f,
            rotationMin: 80f,
            rotationMax: 100f,
            mass: 210,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 62, damageMax: 78, bulletSpeedMin: 50, bulletSpeedMax: 65, fireRateMin: 1.4f, fireRateMax: 1.8f, recoilValue: 15));
        Ship ship404 = CreateShip(
            code: 404,
            name: "Calamity",
            zoom: 1.4f,
            healthMin: 175,
            healthMax: 235,
            healthRegenMin: 22,
            healthRegenMax: 39,
            energyMin: 100,
            energyMax: 200,
            energyRegenMin: 2,
            energyRegenMax: 5,
            speedMin: 8f,
            speedMax: 10f,
            accelerationMin: 90f,
            accelerationMax: 110f,
            rotationMin: 60f,
            rotationMax: 90f,
            mass: 200,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 65, damageMax: 85, bulletSpeedMin: 42, bulletSpeedMax: 58, fireRateMin: 1.1f, fireRateMax: 1.4f, recoilValue: 25));
        Ship ship405 = CreateShip(
            code: 405,
            name: "Trishula",
            zoom: 1.4f,
            healthMin: 240,
            healthMax: 280,
            healthRegenMin: 30,
            healthRegenMax: 47,
            energyMin: 110,
            energyMax: 140,
            energyRegenMin: 2,
            energyRegenMax: 3,
            speedMin: 6.5f,
            speedMax: 9f,
            accelerationMin: 70f,
            accelerationMax: 100f,
            rotationMin: 60f,
            rotationMax: 80f,
            mass: 250,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 75, damageMax: 90, bulletSpeedMin: 40, bulletSpeedMax: 55, fireRateMin: 0.9f, fireRateMax: 1.1f, recoilValue: 35));

        Ship ship501 = CreateShip(
            code: 501,
            name: "Excelion",
            zoom: 1.7f,
            healthMin: 220,
            healthMax: 300,
            healthRegenMin: 37,
            healthRegenMax: 75,
            energyMin: 100,
            energyMax: 170,
            energyRegenMin: 2,
            energyRegenMax: 4,
            speedMin: 7f,
            speedMax: 9.5f,
            accelerationMin: 80f,
            accelerationMax: 110f,
            rotationMin: 50f,
            rotationMax: 70f,
            mass: 200,
            new Cannon(name: "Middle Cannon", priority: 10, damageMin: 65, damageMax: 80, bulletSpeedMin: 45, bulletSpeedMax: 60, fireRateMin: 1.4f, fireRateMax: 1.7f, recoilValue: 25),
            new Cannon(name: "Left Cannon", priority: 1, damageMin: 45, damageMax: 55, bulletSpeedMin: 40, bulletSpeedMax: 55, fireRateMin: 1.8f, fireRateMax: 2.2f, recoilValue: 0),
            new Cannon(name: "Right Cannon", priority: 1, damageMin: 45, damageMax: 55, bulletSpeedMin: 40, bulletSpeedMax: 55, fireRateMin: 1.8f, fireRateMax: 2.2f, recoilValue: 0));
        Ship ship502 = CreateShip(
            code: 502,
            name: "Fergus",
            zoom: 1.7f,
            healthMin: 250,
            healthMax: 320,
            healthRegenMin: 36,
            healthRegenMax: 80,
            energyMin: 175,
            energyMax: 250,
            energyRegenMin: 2,
            energyRegenMax: 4,
            speedMin: 7f,
            speedMax: 8.5f,
            accelerationMin: 100f,
            accelerationMax: 130f,
            rotationMin: 90f,
            rotationMax: 100f,
            mass: 200,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 80, damageMax: 100, bulletSpeedMin: 45, bulletSpeedMax: 60, fireRateMin: 1f, fireRateMax: 1.3f, recoilValue: 25));
        Ship ship503 = CreateShip(
            code: 503,
            name: "Seyfork",
            zoom: 1.7f,
            healthMin: 220,
            healthMax: 300,
            healthRegenMin: 31,
            healthRegenMax: 75,
            energyMin: 100,
            energyMax: 210,
            energyRegenMin: 1,
            energyRegenMax: 4,
            speedMin: 7f,
            speedMax: 9f,
            accelerationMin: 70f,
            accelerationMax: 100f,
            rotationMin: 50f,
            rotationMax: 70f,
            mass: 225,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 70, damageMax: 90, bulletSpeedMin: 48, bulletSpeedMax: 65, fireRateMin: 1.2f, fireRateMax: 1.5f, recoilValue: 20));
        Ship ship504 = CreateShip(
            code: 504,
            name: "Cyborg",
            zoom: 1.7f,
            healthMin: 240,
            healthMax: 320,
            healthRegenMin: 34,
            healthRegenMax: 64,
            energyMin: 100,
            energyMax: 150,
            energyRegenMin: 2,
            energyRegenMax: 4,
            speedMin: 8f,
            speedMax: 10f,
            accelerationMin: 100f,
            accelerationMax: 130f,
            rotationMin: 70f,
            rotationMax: 85f,
            mass: 210,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 75, damageMax: 95, bulletSpeedMin: 48, bulletSpeedMax: 65, fireRateMin: 1.3f, fireRateMax: 1.6f, recoilValue: 20));
        Ship ship505 = CreateShip(
            code: 505,
            name: "Armityle",
            zoom: 1.7f,
            healthMin: 220,
            healthMax: 300,
            healthRegenMin: 31,
            healthRegenMax: 60,
            energyMin: 80,
            energyMax: 120,
            energyRegenMin: 2,
            energyRegenMax: 3,
            speedMin: 8f,
            speedMax: 11f,
            accelerationMin: 100f,
            accelerationMax: 140f,
            rotationMin: 70f,
            rotationMax: 90f,
            mass: 175,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 60, damageMax: 75, bulletSpeedMin: 50, bulletSpeedMax: 68, fireRateMin: 1.5f, fireRateMax: 1.9f, recoilValue: 15));
        Ship ship506 = CreateShip(
            code: 506,
            name: "Goliath",
            zoom: 1.7f,
            healthMin: 300,
            healthMax: 380,
            healthRegenMin: 30,
            healthRegenMax: 63,
            energyMin: 80,
            energyMax: 120,
            energyRegenMin: 1,
            energyRegenMax: 3,
            speedMin: 6f,
            speedMax: 7.5f,
            accelerationMin: 70f,
            accelerationMax: 90f,
            rotationMin: 45f,
            rotationMax: 70f,
            mass: 320,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 65, damageMax: 80, bulletSpeedMin: 38, bulletSpeedMax: 52, fireRateMin: 0.9f, fireRateMax: 1.1f, recoilValue: 40));

        Ship ship601 = CreateShip(
            code: 601,
            name: "Epiloguer",
            zoom: 2f,
            healthMin: 450,
            healthMax: 500,
            healthRegenMin: 41,
            healthRegenMax: 45,
            energyMin: 300,
            energyMax: 330,
            energyRegenMin: 3,
            energyRegenMax: 4,
            speedMin: 7.5f,
            speedMax: 7.6f,
            accelerationMin: 130f,
            accelerationMax: 140f,
            rotationMin: 25f,
            rotationMax: 30f,
            mass: 400,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 130, damageMax: 160, bulletSpeedMin: 55, bulletSpeedMax: 70, fireRateMin: 0.9f, fireRateMax: 1.1f, recoilValue: 25));
        Ship ship602 = CreateShip(
            code: 602,
            name: "Vuaniha",
            zoom: 2f,
            healthMin: 460,
            healthMax: 520,
            healthRegenMin: 35,
            healthRegenMax: 40,
            energyMin: 320,
            energyMax: 350,
            energyRegenMin: 4,
            energyRegenMax: 5,
            speedMin: 7f,
            speedMax: 7.5f,
            accelerationMin: 130f,
            accelerationMax: 140f,
            rotationMin: 50f,
            rotationMax: 55f,
            mass: 420,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 140, damageMax: 175, bulletSpeedMin: 50, bulletSpeedMax: 65, fireRateMin: 0.8f, fireRateMax: 1f, recoilValue: 30));
        Ship ship603 = CreateShip(
            code: 603,
            name: "Raigeki",
            zoom: 2f,
            healthMin: 700,
            healthMax: 770,
            healthRegenMin: 54,
            healthRegenMax: 60,
            energyMin: 850,
            energyMax: 900,
            energyRegenMin: 1,
            energyRegenMax: 2,
            speedMin: 8.5f,
            speedMax: 9f,
            accelerationMin: 50f,
            accelerationMax: 55f,
            rotationMin: 40f,
            rotationMax: 45f,
            mass: 500,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 200, damageMax: 250, bulletSpeedMin: 60, bulletSpeedMax: 80, fireRateMin: 0.7f, fireRateMax: 0.9f, recoilValue: 50));
        Ship ship604 = CreateShip(
            code: 604,
            name: "Tiaramisu",
            zoom: 2f,
            healthMin: 500,
            healthMax: 550,
            healthRegenMin: 29,
            healthRegenMax: 34,
            energyMin: 500,
            energyMax: 550,
            energyRegenMin: 4,
            energyRegenMax: 5,
            speedMin: 8f,
            speedMax: 8.5f,
            accelerationMin: 100f,
            accelerationMax: 110f,
            rotationMin: 20f,
            rotationMax: 25f,
            mass: 400,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 150, damageMax: 190, bulletSpeedMin: 50, bulletSpeedMax: 68, fireRateMin: 0.8f, fireRateMax: 1f, recoilValue: 25));
        Ship ship605 = CreateShip(
            code: 605,
            name: "Tsimtsum",
            zoom: 2f,
            healthMin: 420,
            healthMax: 470,
            healthRegenMin: 19,
            healthRegenMax: 23,
            energyMin: 450,
            energyMax: 500,
            energyRegenMin: 4,
            energyRegenMax: 5,
            speedMin: 8f,
            speedMax: 8.5f,
            accelerationMin: 180f,
            accelerationMax: 190f,
            rotationMin: 40f,
            rotationMax: 45f,
            mass: 340,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 140, damageMax: 180, bulletSpeedMin: 55, bulletSpeedMax: 72, fireRateMin: 0.9f, fireRateMax: 1.1f, recoilValue: 20));
        Ship ship606 = CreateShip(
            code: 606,
            name: "Pusat",
            zoom: 2f,
            healthMin: 300,
            healthMax: 340,
            healthRegenMin: 15,
            healthRegenMax: 18,
            energyMin: 150,
            energyMax: 180,
            energyRegenMin: 2,
            energyRegenMax: 3,
            speedMin: 14.5f,
            speedMax: 15f,
            accelerationMin: 60f,
            accelerationMax: 65f,
            rotationMin: 85f,
            rotationMax: 90f,
            mass: 260,
            new Cannon(name: "Main Cannon", priority: 10, damageMin: 90, damageMax: 120, bulletSpeedMin: 70, bulletSpeedMax: 90, fireRateMin: 1.3f, fireRateMax: 1.6f, recoilValue: 15));

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
        ship303.upgrades.Add(ship404);
        ship303.upgrades.Add(ship405);
        ship304.upgrades.Add(ship405);
        ship304.upgrades.Add(ship401);

        ship401.upgrades.Add(ship501);
        ship401.upgrades.Add(ship502);
        ship402.upgrades.Add(ship502);
        ship402.upgrades.Add(ship503);
        ship403.upgrades.Add(ship503);
        ship403.upgrades.Add(ship504);
        ship404.upgrades.Add(ship505);
        ship404.upgrades.Add(ship506);
        ship405.upgrades.Add(ship506);
        ship405.upgrades.Add(ship501);

        ship501.upgrades.Add(ship601);
        ship501.upgrades.Add(ship602);
        ship502.upgrades.Add(ship602);
        ship502.upgrades.Add(ship603);
        ship503.upgrades.Add(ship603);
        ship503.upgrades.Add(ship604);
        ship504.upgrades.Add(ship604);
        ship504.upgrades.Add(ship605);
        ship505.upgrades.Add(ship605);
        ship505.upgrades.Add(ship606);
        ship506.upgrades.Add(ship606);
        ship506.upgrades.Add(ship601);
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