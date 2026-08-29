using System.Collections.Generic;
using UnityEngine;


public class ShipModels : MonoBehaviour
{
    public List<ShipLevel> shipModels;
    private GameObject currentShip;

    void Start()
    {
        SetShip(101);
    }
    void SetShip(int code)
    {
        int level = code / 100;
        int number = code % 100;
        int levelIndex = level - 1;
        int shipIndex = number - 1;
        GameObject newShip = shipModels[levelIndex].ships[shipIndex];
        foreach (ShipLevel shiplevel in shipModels)
        {
            foreach (GameObject ship in shiplevel.ships)
            {
                ship.SetActive(false);
            }
        }
        newShip.SetActive(true);
        currentShip = newShip;

    }
}


[System.Serializable]
public class ShipLevel
{
    public List<GameObject> ships;
}

