using System.Collections.Generic;
using UnityEngine;


public class ShipModels : MonoBehaviour
{
    public List<ShipLevel> shipModels;
    private GameObject currentShip;
    private PlayerStats playerStats;
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        SetShip(101);
        playerStats.UpgradeTo(101);
    }
    public void SetShip(int code)
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
        newShip.transform.parent.GetComponent<Rigidbody>().mass = playerStats.GetCurrentShip().mass;
    }
}

[System.Serializable]
public class ShipLevel
{
    public List<GameObject> ships;
}

