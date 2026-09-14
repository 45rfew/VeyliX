using System;
using UnityEngine;
public class EnemyHealth : Health
{
    public Action<EnemyHealth> OnEnemyDeath;
    public GameObject hitEffect;
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
        Destroy(gameObject);
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
        }
    }
}