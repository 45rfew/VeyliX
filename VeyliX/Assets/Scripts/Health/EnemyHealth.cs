using System;
using UnityEngine;
public class EnemyHealth : Health
{
    public Action<EnemyHealth> OnEnemyDeath;
    public GameObject hitEffect;
    protected float explosionScale = 0.15f;
    protected float size;
    public float healthRegen;
    public float crystalDrop;
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
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        //effect.transform.localScale = Vector3.one * size * explosionScale;
        //Destroy(effect, 2f);
        CrystalSpawner.Instance.DropCrystals(crystalDrop, pos);
        Destroy(gameObject);
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
        }
    }

}