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

    void Awake()
    {
        BuildShipTree();
        currentShip = ships[101];
        playerEnergy = GetComponent<PlayerEnergy>();
        playerHealth = GetComponent<PlayerHealth>();
        playerShoot = GetComponentInChildren<PlayerShoot>();
        playerLook = GetComponent<PlayerLook>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    public Ship GetCurrentShip()
    {
        return currentShip;
    }
    public void UpgradeTo(int ship)
    {
        currentShip = ships[ship];
        SetCurrentShipCode(ship);
        playerEnergy.NewShip();
        playerHealth.NewShip();
        //playerShoot.ResetCannons();
        UpdateAllStats();
        upgradeButtons.ResetUpgradeCounts();
    }
    public int GetCurrentShipCode()
    {
        return currentShipCode;
    }
    public void SetCurrentShipCode(int code)
    {
        currentShipCode = code;
    }
    public int GetShipTier()
    {
        return Mathf.FloorToInt(currentShipCode / 100);
    }
    public float GetMaxCrystals()
    {
        return Mathf.Pow(GetShipTier(), 2) * 20;
    }
    public void UpgradeHealth(int amount)
    {
        currentShip.health.Upgrade(amount);
        playerHealth.NewShip();
    }
    public void UpgradeHealthRegen(int amount)
    {
        currentShip.healthRegen.Upgrade(amount);
        playerHealth.NewShip();
    }
    public void UpgradeEnergy(int amount)
    {
        currentShip.energy.Upgrade(amount);
        playerEnergy.NewShip();
    }
    public void UpgradeEnergyRegen(int amount)
    {
        currentShip.energyRegen.Upgrade(amount);
        playerEnergy.NewShip();
    }
    public void UpgradeDamage(int amount)
    {
        for (int i = 0; i < currentShip.cannons.Count; i++)
        {
            currentShip.cannons[i].damage.Upgrade(amount);
        }   
    }
    public void UpgradeBulletSpeed(int amount)
    {
        for (int i = 0; i < currentShip.cannons.Count; i++)
        {
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
        Ship ship1 = new Ship();
        ship1.code = 101;
        ship1.name = "poo";
        ship1.health = new StatRange(80, 120);
        ship1.healthRegen = new StatRange(4, 8);
        ship1.energy = new StatRange(50, 80);
        ship1.energyRegen = new StatRange(100, 1005);
        ship1.speed = new StatRange(90/5, 215/5);
        ship1.acceleration = new StatRange(90, 140);
        ship1.rotationSpeed = new StatRange(80*5, 100*5);
        ship1.mass = 100;
        ship1.cannons = new List<Cannon>();
        ship1.cannons.Add(
            new Cannon(
                "Middle Cannon",
                10,
                20, 50,
                30, 40,
                2, 2,
                50
            )
        );
        ship1.cannons.Add(
            new Cannon(
                "Left Cannon",
                1,
                10, 20,
                30, 40,
                4, 4,
                0
            )
        );
        ship1.cannons.Add(
            new Cannon(
                "Right Cannon",
                1,
                10, 20,
                30, 40,
                4, 4,
                0
            )
        );

        Ship ship201 = new Ship();
        ship201.code = 201;
        ship201.name = "Fighter";
        ship201.health = new StatRange(100, 160);
        ship201.healthRegen = new StatRange(5, 10);
        ship201.energy = new StatRange(60, 100);
        ship201.energyRegen = new StatRange(12, 18);
        ship201.speed = new StatRange(20, 32);
        ship201.acceleration = new StatRange(100, 160);
        ship201.rotationSpeed = new StatRange(350, 450);
        ship201.mass = 120;
        ship201.cannons = new List<Cannon>();
        ship201.cannons.Add(
            new Cannon(
                "Main Cannon",
                10,
                50, 60,
                60, 80,
                1, 1,
                60
            )
        );

        Ship ship202 = new Ship();
        ship202.code = 202;
        ship202.name = "Tank";
        ship202.health = new StatRange(150, 250);
        ship202.healthRegen = new StatRange(3, 6);
        ship202.energy = new StatRange(80, 120);
        ship202.energyRegen = new StatRange(8, 12);
        ship202.speed = new StatRange(10, 18);
        ship202.acceleration = new StatRange(70, 100);
        ship202.rotationSpeed = new StatRange(200, 300);
        ship202.mass = 200;

        ship202.cannons = new List<Cannon>();

        // ship202.cannons.Add(
        //     new Cannon(
        //         "Heavy Cannon",
        //         50, 120,
        //         30, 50,
        //         0.5f, 2,
        //         60, 100,
        //         8, 20,2
        //     )
        // );

        Ship ship301 = new Ship();
        ship301.code = 301;
        ship301.name = "Fighter";
        ship301.health = new StatRange(222, 160);
        ship301.healthRegen = new StatRange(5, 10);
        ship301.energy = new StatRange(260, 300);
        ship301.energyRegen = new StatRange(100, 180);
        ship301.speed = new StatRange(20, 32);
        ship301.acceleration = new StatRange(66, 160);
        ship301.rotationSpeed = new StatRange(350, 450);
        ship301.mass = 222;
        ship301.cannons = new List<Cannon>();
        ship301.cannons.Add(
            new Cannon(
                "Main Cannon",
                1,
                250, 300,
                20, 30,
                1, 1,
                200
            )
        );
        //tree path
        ship1.upgrades.Add(ship201);
        ship1.upgrades.Add(ship202);
        ship201.upgrades.Add(ship301);
        ships.Add(ship1.code,ship1);
        ships.Add(ship201.code,ship201);
        ships.Add(ship202.code,ship202);
        ships.Add(ship301.code,ship301);
    }
}