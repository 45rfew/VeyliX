using UnityEngine;

public class AsteroidHealth : Health
{
    private float size;
    private float yield = 10;
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
    private float dir;
    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        size = rb.transform.parent.localScale.x;
        maxHealth = healthValues[Mathf.FloorToInt(size - 1)];
        currentHealth = maxHealth;
        OnDamage += HandleDamage;
        OnDeath += HandleDeath;
        OnDamage?.Invoke(maxHealth, maxHealth);
        dir = Random.value > 0.5f ? -1f : 1f;
        base.Start();
    }
    void Update()
    {
        rb.transform.Rotate(0f, 15f * dir * Time.deltaTime, 0f);
    }

    void HandleDamage(int currentHealth, int maxHealth)
    {
        int damage = maxHealth - currentHealth;
    }
    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
    }
    void HandleDeath()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        CrystalSpawner.Instance.DropCrystals(Mathf.RoundToInt(size * Random.Range(2.0f, 2.5f)*yield), pos);
        Destroy(gameObject, 0.01f);
    }
}   

