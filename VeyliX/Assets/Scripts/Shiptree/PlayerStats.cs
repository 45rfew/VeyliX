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
        Ship ship1 = new Ship();
        ship1.code = 101;
        ship1.name = "poo";
        ship1.health = new IntStatRange(2000, 120);
        ship1.healthRegen = new IntStatRange(4, 8);
        ship1.energy = new StatRange(100, 180);
        ship1.energyRegen = new StatRange(10, 1005);
        ship1.speed = new StatRange(45/5, 215/5);
        ship1.acceleration = new StatRange(90, 140);
        ship1.rotationSpeed = new StatRange(80*5, 100*5);
        ship1.mass = 800;
        ship1.cannons = new List<Cannon>();
        ship1.cannons.Add(
            new Cannon(
                "Middle Cannon",
                10,
                20, 50,
                30, 40,
                2, 2,
                20
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
        ship201.health = new IntStatRange(100, 160);
        ship201.healthRegen = new IntStatRange(5, 10);
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
        ship202.health = new IntStatRange(150, 250);
        ship202.healthRegen = new IntStatRange(3, 6);
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
        ship301.health = new IntStatRange(222, 160);
        ship301.healthRegen = new IntStatRange(5, 10);
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

        Ship ship401 = new Ship();
        ship401.code = 401;
        ship401.name = "pooa";
        ship401.health = new IntStatRange(2000, 120);
        ship401.healthRegen = new IntStatRange(4, 8);
        ship401.energy = new StatRange(100, 180);
        ship401.energyRegen = new StatRange(10, 1005);
        ship401.speed = new StatRange(45/5, 215/5);
        ship401.acceleration = new StatRange(90, 140);
        ship401.rotationSpeed = new StatRange(80*5, 100*5);
        ship401.mass = 800;
        ship401.cannons = new List<Cannon>();
        ship401.cannons.Add(
            new Cannon(
                "Middle Cannon",
                10,
                20, 50,
                30, 40,
                2, 2,
                50
            )
        );
        ship401.cannons.Add(
            new Cannon(
                "Left Cannon",
                1,
                10, 20,
                30, 40,
                4, 4,
                0
            )
        );
        ship401.cannons.Add(
            new Cannon(
                "Right Cannon",
                1,
                10, 20,
                30, 40,
                4, 4,
                0
            )
        );

        Ship ship501 = new Ship();
        ship501.code = 501;
        ship501.name = "paoo";
        ship501.health = new IntStatRange(2000, 120);
        ship501.healthRegen = new IntStatRange(4, 8);
        ship501.energy = new StatRange(100, 180);
        ship501.energyRegen = new StatRange(10, 1005);
        ship501.speed = new StatRange(45/5, 215/5);
        ship501.acceleration = new StatRange(90, 140);
        ship501.rotationSpeed = new StatRange(80*5, 100*5);
        ship501.mass = 800;
        ship501.cannons = new List<Cannon>();
        ship501.cannons.Add(
            new Cannon(
                "Middle Cannon",
                10,
                20, 50,
                30, 40,
                2, 2,
                50
            )
        );
        ship501.cannons.Add(
            new Cannon(
                "Left Cannon",
                1,
                10, 20,
                30, 40,
                4, 4,
                0
            )
        );
        ship501.cannons.Add(
            new Cannon(
                "Right Cannon",
                1,
                10, 20,
                30, 40,
                4, 4,
                0
            )
        );

        Ship ship601 = new Ship();
        ship601.code = 601;
        ship601.name = "apaoo";
        ship601.health = new IntStatRange(2000, 120);
        ship601.healthRegen = new IntStatRange(4, 8);
        ship601.energy = new StatRange(100, 180);
        ship601.energyRegen = new StatRange(10, 1005);
        ship601.speed = new StatRange(45/5, 215/5);
        ship601.acceleration = new StatRange(90, 140);
        ship601.rotationSpeed = new StatRange(80*5, 100*5);
        ship601.mass = 800;
        ship601.cannons = new List<Cannon>();
        ship601.cannons.Add(
            new Cannon(
                "Middle Cannon",
                10,
                20, 50,
                30, 40,
                2, 2,
                50
            )
        );
        ship601.cannons.Add(
            new Cannon(
                "Left Cannon",
                1,
                10, 20,
                30, 40,
                4, 4,
                0
            )
        );
        ship601.cannons.Add(
            new Cannon(
                "Right Cannon",
                1,
                10, 20,
                30, 40,
                4, 4,
                0
            )
        );

        //tree path
        ship1.upgrades.Add(ship201);
        ship1.upgrades.Add(ship202);

        ship201.upgrades.Add(ship301);
        ship201.upgrades.Add(ship301);



        ship301.upgrades.Add(ship401);
        ship401.upgrades.Add(ship501);
        ship501.upgrades.Add(ship601);
        


        ships.Add(ship1.code, ship1);
        
        ships.Add(ship201.code,ship201);
        ships.Add(ship202.code, ship202);

        ships.Add(ship301.code, ship301);

        ships.Add(ship401.code, ship401);

        ships.Add(ship501.code, ship501);

        ships.Add(ship601.code, ship601);
        

    }
}