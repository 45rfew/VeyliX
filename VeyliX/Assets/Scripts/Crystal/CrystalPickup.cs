using System;
using UnityEditor.Callbacks;
using UnityEngine;

public class CrystalPickup : MonoBehaviour
{
    private float value = 0;
    private float timer;
    private float pickupDelay = 0.2f;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        timer += Time.deltaTime;
    }
    private void OnTriggerStay(Collider collision)
    {
        if (timer < pickupDelay) return; 
        if (collision.CompareTag("Player"))
        {
            PlayerCrystals playerCrystals = collision.GetComponentInParent<PlayerCrystals>();
            if (playerCrystals != null)
            {
                playerCrystals.AddCrystals(value);
                Debug.Log("Crystal value: " + value);
                Destroy(gameObject, 0.1f);
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