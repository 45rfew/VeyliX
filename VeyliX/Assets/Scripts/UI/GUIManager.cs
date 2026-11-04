using System;
using System.Collections;
using System.Reflection.Emit;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    public PlayerHealth health;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI gemText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lvlText;
    public PlayerEnergy playerEnergy;
    public PlayerCrystals crystalPickup;
    public PlayerStats playerStats;
    public GameObject shipTierUpgrade;
    public GameObject shipUpgrades;
    public UpgradeButtons upgradeButtons;
    public GameObject respawnUI;
    public TextMeshProUGUI livesText;
    public Action<bool> OnRespawnTriggered; 
    private int playerScore;
    private RectTransform shipTierUpgradeRect;
    private Vector2 shipTierUpgradePosition;
    private Coroutine shipTierUpgradeAnimation;
    private bool shipTierUpgradeVisible;
    private RectTransform shipUpgradesRect;
    private Vector2 shipUpgradesPosition;
    private Coroutine shipUpgradesAnimation;
    private bool shipUpgradesVisible;
    
    void Awake()
    {
        if (shipTierUpgrade == null)
        {
            return;
        }
        shipTierUpgradeRect = shipTierUpgrade.GetComponent<RectTransform>();
        if (shipTierUpgradeRect == null)
        {
            return;
        }
        shipTierUpgradePosition = shipTierUpgradeRect.anchoredPosition;
        shipTierUpgradeVisible = false;
        shipTierUpgrade.SetActive(false);
        if (shipUpgrades == null)
        {
            return;
        }
        shipUpgradesRect = shipUpgrades.GetComponent<RectTransform>();
        if (shipUpgradesRect == null)
        {
            return;
        }
        shipUpgradesPosition = shipUpgradesRect.anchoredPosition;
        shipUpgradesVisible = false;
        shipUpgrades.SetActive(false);
    }

    void Start()
    {
        health.OnDamage += UpdateHealth;
        health.OnHeal += UpdateHealth;
        health.OnScoreChanged += UpdateScore;
        health.OnShipDestroyed += ShowRespawnUI;
        crystalPickup.OnplayerCrystalChange += UpdateGem;
        playerEnergy.OnEnergyRegen += UpdateEnergy;
        playerStats.ShipChanged += UpdateShipTierUpgradeText;
        health.OnLivesChanged += UpdateLives;
        UpdateEnergy(playerEnergy.GetEnergy(),playerEnergy.GetEnergy());
        UpdateGem(crystalPickup.GetCrystalCount());
        UpdateLives(health.GetLives());
        SetShipTierUpgradeVisible(false);
   }

    void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.ShipChanged -= UpdateShipTierUpgradeText;
        }
    }
    void UpdateLives(int lives)
    {
        livesText.text = $"{lives}";
    }
    void ShowRespawnUI(bool visible)
    {
        if (respawnUI != null)
        {
            respawnUI.SetActive(visible);
        }
    }
    public void TriggerRespawn()
    {
        if (respawnUI != null)
        {
            respawnUI.SetActive(false);
            OnRespawnTriggered?.Invoke(true);
        }
    }

    public void QuitToMainMenu()
    {
        OnRespawnTriggered?.Invoke(false);
        SceneManager.LoadScene("MainMenu");
    }
    void UpdateShipTierUpgradeText(){
        if (shipTierUpgrade == null)
        {
            return;
        }
        Ship ship = playerStats.GetCurrentShip();
        int shipTier = playerStats.GetShipTier();
        if (ship.upgrades.Count == 0)
        {
            SetShipTierUpgradeVisible(false);
        }

        for (int i=0; i<shipTierUpgrade.transform.childCount; i++)
        {
            GameObject button = shipTierUpgrade.transform.GetChild(i).gameObject;
            if (i >= ship.upgrades.Count)
            {
                button.SetActive(false);
                continue;
            }

            button.SetActive(true);
            TMP_Text name = button.transform.Find("Name").GetComponent<TMP_Text>() ?? null;
            TMP_Text level = button.transform.Find("Level").GetComponent<TMP_Text>() ?? null;
            if (name == null || level == null)
            {
                return;
            }
            name.text = $"{ship.upgrades[i].name ?? null} [{9-i*9}]";
            level.text = $"lvl {shipTier+1}";
        }

    }
    void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthText.text = $"Shield: {currentHealth}/{maxHealth}";
        //healthBar.value = currentHealth / maxHealth * 100 <= 0 ? 0.01f : currentHealth / maxHealth * 100; 
        //healthText.text = currentHealth < 0 ? $"0 / {maxHealth}" : $"{currentHealth} / {maxHealth}";
    }
    void UpdateEnergy(float currentEnergy, float maxEnergy)
    {
        energyText.text = $"Energy: {Mathf.Floor(currentEnergy)}/{Mathf.Floor(maxEnergy)}";    
    }
    void UpdateGem(int currentGem)
    { 
        bool canBuyUpgrade = currentGem >= playerStats.UpgradeCrystalCost() && !upgradeButtons.CheckUpgradesMaxed();
        SetShipUpgradesVisible(canBuyUpgrade);
        int maxGem = playerStats.GetMaxCrystals();
        bool canUpgradeShipTier = currentGem >= maxGem && playerStats.GetShipTier() < playerStats.GetMaxShipTier();
        SetShipTierUpgradeVisible(canUpgradeShipTier);
        gemText.text = $"Gems: {currentGem}/{maxGem}";
    }

    void SetShipTierUpgradeVisible(bool visible)
    {
        if (shipTierUpgradeRect == null || shipTierUpgradeVisible == visible)
        {
            return;
        }
        shipTierUpgradeVisible = visible;
        if (shipTierUpgradeAnimation != null)
        {
            StopCoroutine(shipTierUpgradeAnimation);
        }
        if (visible)
        {
            shipTierUpgrade.SetActive(true);
            if (shipTierUpgradeRect.anchoredPosition == shipTierUpgradePosition)
            {
                shipTierUpgradeRect.anchoredPosition = GetHiddenShipTierUpgradePosition();
            }
            shipTierUpgradeAnimation = StartCoroutine(AnimateShipTierUpgrade(shipTierUpgradePosition, false));
            UpdateShipTierUpgradeText();
        }
        else
        {
            shipTierUpgradeAnimation = StartCoroutine(AnimateShipTierUpgrade(GetHiddenShipTierUpgradePosition(), true));
        }
    }

    Vector2 GetHiddenShipTierUpgradePosition()
    {
        RectTransform parentRect = shipTierUpgradeRect.parent as RectTransform;
        if (parentRect == null)
        {
            return shipTierUpgradePosition + Vector2.up * shipTierUpgradeRect.rect.height;
        }

        return shipTierUpgradePosition + Vector2.up * (parentRect.rect.height + shipTierUpgradeRect.rect.height);
    }

    IEnumerator AnimateShipTierUpgrade(Vector2 destination, bool hideWhenFinished)
    {
        Vector2 startPosition = shipTierUpgradeRect.anchoredPosition;
        const float duration = 0.3f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            shipTierUpgradeRect.anchoredPosition = Vector2.Lerp(startPosition, destination, t);
            yield return null;
        }
        shipTierUpgradeRect.anchoredPosition = destination;
        shipTierUpgradeAnimation = null;
        if (hideWhenFinished)
        {
            shipTierUpgrade.SetActive(false);
        }
    }

    void SetShipUpgradesVisible(bool visible)
    {
        if (shipUpgradesRect == null || shipUpgradesVisible == visible)
        {
            return;
        }
        shipUpgradesVisible = visible;
        if (shipUpgradesAnimation != null)
        {
            StopCoroutine(shipUpgradesAnimation);
        }
        if (visible)
        {
            shipUpgrades.SetActive(true);
            if (shipUpgradesRect.anchoredPosition == shipUpgradesPosition)
            {
                shipUpgradesRect.anchoredPosition = GetHiddenShipUpgradesPosition();
            }
            shipUpgradesAnimation = StartCoroutine(AnimateShipUpgrades(shipUpgradesPosition, false));
        }
        else
        {
            shipUpgradesAnimation = StartCoroutine(AnimateShipUpgrades(GetHiddenShipUpgradesPosition(), true));
        }
    }

    Vector2 GetHiddenShipUpgradesPosition()
    {
        RectTransform parentRect = shipUpgradesRect.parent as RectTransform;
        if (parentRect == null)
        {
            return shipUpgradesPosition + Vector2.left * shipUpgradesRect.rect.width;
        }
        return shipUpgradesPosition + Vector2.left * (parentRect.rect.width + shipUpgradesRect.rect.width);
    }
    IEnumerator AnimateShipUpgrades(Vector2 destination, bool hideWhenFinished)
    {
        Vector2 startPosition = shipUpgradesRect.anchoredPosition;
        const float duration = 0.3f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            shipUpgradesRect.anchoredPosition = Vector2.Lerp(startPosition, destination, t);
            yield return null;
        }
        shipUpgradesRect.anchoredPosition = destination;
        shipUpgradesAnimation = null;
        if (hideWhenFinished)
        {
            shipUpgrades.SetActive(false);
        }
    }
    
    void UpdateScore(int score)
    {
        Debug.Log("Score updated by " + score);
        playerScore += score;
        scoreText.text = "Score: " + playerScore;
    }
}