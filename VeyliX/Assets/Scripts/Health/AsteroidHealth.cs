using System;
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
            GameObject crystal = Instantiate(crysatalPrefab, pos, Quaternion.identity);
            float t = Mathf.InverseLerp(1f, 9f, size);
            float scale = 0.6f + 2f * Mathf.Pow(t, 0.7f);
            crystal.GetComponentInChildren<CrystalPickup>().SetValue(size);
            crystal.transform.localScale = Vector3.one * scale;
        }
        Destroy(gameObject, 0.01f);
    }
}
