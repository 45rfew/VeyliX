using System;
using Unity.InferenceEngine;
using Unity.VisualScripting;
using UnityEngine;

public class AsteroidHealth : Health
{
    [SerializeField] private GameObject crysatalPrefab;
    private float size;
    private int[] healthValues =
    {
        20,
        30,
        45,
        65,
        90,
        130,
        250,
        300,
        500
    };
    
    private int[] scoreValues =
    {
        1,2,3,4,5,6,7,8,9
    };
    private Rigidbody rb;
    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        size = rb.transform.parent.localScale.x;
        maxHealth = healthValues[Mathf.FloorToInt(size - 1)];
        currentHealth = maxHealth;
        OnDamage += HandleDamage;
        OnDeath += HandleDeath;
        OnDamage?.Invoke(maxHealth, maxHealth);
        base.Start();
    }

    void HandleDamage(float currentHealth, float maxHealth)
    {
        float damage = maxHealth - currentHealth;
    }
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }
    void HandleDeath()
    {
        if (crysatalPrefab != null)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            DropCrystals(size * UnityEngine.Random.Range(1.0f,1.2f), pos);
            // GameObject crystal = Instantiate(crysatalPrefab, pos, Quaternion.identity);
            // float t = Mathf.InverseLerp(1f, 9f, size);
            // float scale = 0.6f + 2f * Mathf.Pow(t, 0.7f);
            // crystal.GetComponentInChildren<CrystalPickup>().SetValue(size * 10);
            // crystal.transform.localScale = Vector3.one * scale;
        }
        Destroy(gameObject, 0.01f);
    }
    void DropCrystals(float amount, Vector3 pos)
    {
        int rand = UnityEngine.Random.Range(0, 100);
        int chunks = rand < 50 ? 1 : rand > 75 ? 3 : 2;
        if (chunks == 1)
        {
            SpawnCrystals(amount, pos);
        }
        if (chunks == 2)
        {
            float firstAmount = UnityEngine.Random.Range(0, amount);
            float secondAmount = amount - firstAmount;
            SpawnCrystals(firstAmount, pos);
            SpawnCrystals(secondAmount, pos);
        }
        if (chunks == 3)
        {
            float firstAmount = UnityEngine.Random.Range(0, amount);
            float secondAmount = UnityEngine.Random.Range(amount - firstAmount, amount);
            float thirdAmount = amount - firstAmount + secondAmount;
            SpawnCrystals(firstAmount, pos);
            SpawnCrystals(secondAmount, pos);
            SpawnCrystals(thirdAmount, pos);
        }   
    }
    void SpawnCrystals(float amount, Vector3 pos)
    {
        GameObject crystal = Instantiate(crysatalPrefab, pos, Quaternion.identity);
        crystal.GetComponentInChildren<CrystalPickup>().SetValue(amount);
        crystal.transform.localScale = Vector3.one * amount / 5;
        Rigidbody rb = crystal.GetComponent<Rigidbody>();
        float randomX = UnityEngine.Random.Range(-1f, 1f);
        float randomZ = UnityEngine.Random.Range(-1f, 1f);
        Vector3 randomDir = new Vector3(randomX, 0, randomZ).normalized;
        rb.AddForce(randomDir * 4, ForceMode.Impulse);
        Destroy(crystal, 10f);       
    }
}   

