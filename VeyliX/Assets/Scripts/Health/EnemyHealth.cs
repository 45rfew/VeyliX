using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemySpawn
{
    public GameObject enemyPrefab;
    public int amount;
}

public class EnemyHealth : Health
{
    public Action<EnemyHealth> OnEnemyDeath;
    public GameObject hitEffect;
    protected float explosionScale = 0.15f;
    protected float size;
    public int healthRegen;
    public int crystalDrop;
    public List<EnemySpawn> enemySpawns;
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
        if (enemySpawns == null || enemySpawns.Count == 0)
        {
            Destroy(gameObject);
            return;
        }
        foreach (EnemySpawn enemy in enemySpawns)
        {
            int amount = enemy.amount;
            for (int i = 0; i < amount; i++)
            {
                GameObject spawnedEnemy = Instantiate(enemy.enemyPrefab, transform.position, Quaternion.identity);
                float randomX = UnityEngine.Random.Range(-1f, 1f);
                float randomZ = UnityEngine.Random.Range(-1f, 1f);
                Vector3 randomDir = new Vector3(randomX, 0, randomZ).normalized;
                Rigidbody rb = spawnedEnemy.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.transform.Rotate(0f, UnityEngine.Random.Range(0f, 360f), 0f);
                    rb.AddForce(randomDir * 2, ForceMode.Impulse);
                }

            }

        }
        Destroy(gameObject);
    }
}