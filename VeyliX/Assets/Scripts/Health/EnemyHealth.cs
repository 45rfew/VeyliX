using System;
using UnityEngine;
public class EnemyHealth : Health
{
    public Action<EnemyHealth> OnEnemyDeath;
    public GameObject hitEffect;
    protected float explosionScale = 0.15f;
    protected float size;
    public int healthRegen;
    public int crystalDrop;
    private float regenAccumulator;
    protected override void Start()
    {
        base.Start();
        OnDamage += HandleDamage;
        OnDeath += HandleDeath;
    }
    void Update()
    {
        if (currentHealth < maxHealth)
        {
            regenAccumulator += healthRegen * Time.deltaTime;
            int healing = Mathf.FloorToInt(regenAccumulator);
            if (healing > 0)
            {
                regenAccumulator -= healing;
                Heal(healing);
            }
        }
    }
    protected virtual void HandleDamage(int damage, int maxHealth)
    {
    }
    public void Heal(int amount)
    {
        currentHealth = Math.Min(currentHealth + amount, maxHealth);
    }
    protected virtual void HandleDeath()
    {
        OnEnemyDeath?.Invoke(this);
        OnDamage -= HandleDamage;
        OnDeath -= HandleDeath;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        //effect.transform.localScale = Vector3.one * size * explosionScale;
        //Destroy(effect, 2f);
        CrystalSpawner.Instance.DropCrystals(crystalDrop, pos);
        Destroy(gameObject);
    }
}