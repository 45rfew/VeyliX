using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private List<Ship> ships = new List<Ship>();

    private Ship currentShip;

    void Awake()
    {
        BuildShipTree();

        currentShip = ships[0];
    }

    void BuildShipTree()
    {
        // =========================
        // SHIP 1
        // =========================

        Ship ship1 = new Ship();

        ship1.name = "Scout";

        ship1.health = new StatRange(80, 120);
        ship1.healthRegen = new StatRange(4, 8);

        ship1.energy = new StatRange(50, 80);
        ship1.energyRegen = new StatRange(10, 15);

        ship1.speed = new StatRange(15, 25);
        ship1.acceleration = new StatRange(90, 140);
        ship1.rotationSpeed = new StatRange(300, 400);
        ship1.mass = new StatRange(80, 100);

        ship1.cannons = new List<Cannon>();

        ship1.cannons.Add(
            new Cannon(
                "Left Cannon",
                10, 20,
                5, 10,
                4, 7,
                80, 100,
                2, 5,
                20
            )
        );

        ship1.cannons.Add(
            new Cannon(
                "Right Cannon",
                10, 20,
                5, 10,
                4, 7,
                80, 100,
                2, 5,
                20
            )
        );


        // =========================
        // SHIP 2A
        // =========================

        Ship ship2A = new Ship();

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
                30, 80,
                20, 35,
                1, 3,
                80, 120,
                5, 15,2
            )
        );

        ship2A.cannons.Add(
            new Cannon(
                "Left Cannon",
                8, 18,
                5, 10,
                5, 8,
                80, 110,
                2, 5,2
            )
        );

        ship2A.cannons.Add(
            new Cannon(
                "Right Cannon",
                8, 18,
                5, 10,
                5, 8,
                80, 110,
                2, 5,2
            )
        );


        // =========================
        // SHIP 2B
        // =========================

        Ship ship2B = new Ship();

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

        ship2B.cannons.Add(
            new Cannon(
                "Heavy Cannon",
                50, 120,
                30, 50,
                0.5f, 2,
                60, 100,
                8, 20,2
            )
        );


        // =========================
        // TREE CONNECTIONS
        // =========================

        ship1.upgrades.Add(ship2A);
        ship1.upgrades.Add(ship2B);

        // Add everything to the list

        ships.Add(ship1);
        ships.Add(ship2A);
        ships.Add(ship2B);
    }
    public Ship GetCurrentShip()
    {
        return currentShip;
    }


    public void UpgradeTo(Ship ship)
    {
        currentShip = ship;
    }
}