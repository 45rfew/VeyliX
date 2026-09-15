using System;
using UnityEngine;
public class EnemyHealth : Health
{
    public Action<EnemyHealth> OnEnemyDeath;
    public GameObject hitEffect;
    public GameObject crystalPrefab;
    protected float explosionScale = 0.15f;
    protected float size;
    public float healthRegen;
    protected override void Start()
    {
        base.Start();

        OnDamage += HandleDamage;
        OnDeath += HandleDeath;

    }
    protected virtual void HandleDamage(float damage, float maxHealth)
    {
    }
    protected virtual void HandleDeath()
    {
        OnEnemyDeath?.Invoke(this);
        OnDamage -= HandleDamage;
        OnDeath -= HandleDeath;

        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        //effect.transform.localScale = Vector3.one * size * explosionScale;
        //Destroy(effect, 2f);
        if (crystalPrefab != null)
        {
            size = 1;
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject crystal = Instantiate(crystalPrefab, pos, Quaternion.identity);
            float t = Mathf.InverseLerp(1f, 9f, size);
            float scale = 0.6f + 2f * Mathf.Pow(t, 0.7f);
            crystal.GetComponentInChildren<CrystalPickup>().SetValue(size*10);
            crystal.transform.localScale = Vector3.one * scale;
        }
        Destroy(gameObject);
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
        }
    }

}