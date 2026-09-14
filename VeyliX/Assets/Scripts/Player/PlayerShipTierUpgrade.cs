using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerShipTierUpgrade : MonoBehaviour
{
    [SerializeField] private Button upgrade1;
    [SerializeField] private Button upgrade2;

    private PlayerStats playerStats;
    private PlayerCrystals playerCrystals;
    public ShipModels shipModels;
    void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerCrystals = GetComponent<PlayerCrystals>();
        upgrade1.onClick.AddListener(() => Upgrade(0));
        upgrade2.onClick.AddListener(() => Upgrade(1));
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
            Upgrade(0);
        if (Input.GetKeyDown(KeyCode.RightBracket))
            Upgrade(1);
    }
    private void Upgrade(int i)
    {
        if (playerCrystals.GetCrystalCount() >= playerStats.GetMaxCrystals())
        {
            Ship ship = playerStats.GetCurrentShip();
            playerStats.UpgradeTo(ship.upgrades[i].code);
            shipModels.SetShip(ship.upgrades[i].code);
            playerCrystals.RemoveCrystals((int)playerStats.GetMaxCrystals());
            playerCrystals.OnplayerCrystalChange?.Invoke(playerCrystals.GetCrystalCount());
        }
    }
}
