using System.Collections.Generic;
using UnityEngine;
//hard code each ship zzz
public class PlayerStats : MonoBehaviour
{
    //private List<Ship> ships = new List<Ship>();
    private Dictionary<int, Ship> ships = new Dictionary<int, Ship>();
    private Ship currentShip;
    private int currentShipCode;
    private PlayerShoot playerShoot;
    void Awake()
    {
        BuildShipTree();
        currentShip = ships[101];
        playerShoot = GetComponentInChildren<PlayerShoot>();
    }
    void BuildShipTree()
    {
        Ship ship1 = new Ship();
        ship1.code = 101;
        ship1.name = "poo";
        ship1.health = new StatRange(80, 120);
        ship1.healthRegen = new StatRange(4, 8);
        ship1.energy = new StatRange(50, 80);
        ship1.energyRegen = new StatRange(10, 15);
        ship1.speed = new StatRange(90/5, 115/5);
        ship1.acceleration = new StatRange(90, 140);
        ship1.rotationSpeed = new StatRange(80*5, 100*5);
        ship1.mass = new StatRange(100, 100);
        ship1.cannons = new List<Cannon>();
        ship1.cannons.Add(
            new Cannon(
                "Middle Cannon",
                10,
                20, 30,
                60, 80,
                2, 2,
                20
            )
        );
        ship1.cannons.Add(
            new Cannon(
                "Left Cannon",
                1,
                10, 20,
                60, 80,
                4, 4,
                0
            )
        );
        ship1.cannons.Add(
            new Cannon(
                "Right Cannon",
                1,
                10, 20,
                80, 100,
                4, 4,
                0
            )
        );

        Ship ship2A = new Ship();
        ship2A.code = 201;
        ship2A.name = "Fighter";
        ship2A.health = new StatRange(100, 160);
        ship2A.healthRegen = new StatRange(5, 10);
        ship2A.energy = new StatRange(60, 100);
        ship2A.energyRegen = new StatRange(12, 18);
        ship2A.speed = new StatRange(20, 32);
        ship2A.acceleration = new StatRange(100, 160);
        ship2A.rotationSpeed = new StatRange(350, 450);
        ship2A.mass = new StatRange(90, 110);
        ship2A.cannons = new List<Cannon>();
        ship2A.cannons.Add(
            new Cannon(
                "Main Cannon",
                10,
                50, 60,
                60, 80,
                1, 1,
                60
            )
        );

        Ship ship2B = new Ship();
        ship2B.code = 202;
        ship2B.name = "Tank";

        ship2B.health = new StatRange(150, 250);
        ship2B.healthRegen = new StatRange(3, 6);

        ship2B.energy = new StatRange(80, 120);
        ship2B.energyRegen = new StatRange(8, 12);

        ship2B.speed = new StatRange(10, 18);
        ship2B.acceleration = new StatRange(70, 100);
        ship2B.rotationSpeed = new StatRange(200, 300);
        ship2B.mass = new StatRange(150, 200);

        ship2B.cannons = new List<Cannon>();

        // ship2B.cannons.Add(
        //     new Cannon(
        //         "Heavy Cannon",
        //         50, 120,
        //         30, 50,
        //         0.5f, 2,
        //         60, 100,
        //         8, 20,2
        //     )
        // );


        //tree path
        ship1.upgrades.Add(ship2A);
        ship1.upgrades.Add(ship2B);

        ships.Add(ship1.code,ship1);
        ships.Add(ship2A.code,ship2A);
        ships.Add(ship2B.code,ship2B);
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
    }
    public int GetCurrentShipCode()
    {
        return currentShipCode;
    }
    public void SetCurrentShipCode(int code){
        currentShipCode = code;
    }
}