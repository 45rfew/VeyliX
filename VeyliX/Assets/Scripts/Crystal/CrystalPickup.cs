using System;
using UnityEngine;

public class CrystalPickup : MonoBehaviour
{
    private float value = 0;
    private float timer;
    private float pickupDelay = 0.5f;
    void Update()
    {
        timer += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (timer < pickupDelay) return; 
        if (collision.CompareTag("Player"))
        {
            PlayerCrystals playerCrystals = collision.GetComponentInParent<PlayerCrystals>();
            if (playerCrystals != null)
            {
                playerCrystals.AddCrystals(value);
                Debug.Log("Crystal value: " + value);
                Destroy(gameObject);
            }
        }
    }

    public void SetValue(float value)
    {
        this.value = value;
    }

    public float GetValue()
    {
        return value;
    }
}