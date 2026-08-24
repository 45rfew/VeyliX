using UnityEngine;

public class AsteroidHealth : Health
{
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
        size = rb.transform.localScale.x;
        Debug.Log("Size: " + size);
        maxHealth = healthValues[Mathf.FloorToInt(size - 1)];
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
        Destroy(gameObject, 0.1f);
    }
}
