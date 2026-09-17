using System;
using UnityEditor.Callbacks;
using UnityEngine;

public class CrystalPickup : MonoBehaviour
{
    private int value;
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
                Destroy(gameObject, 0.01f);
            }
        }
    }
    public void SetValue(int value)
    {
        this.value = value;
    }

    public int GetValue()
    {
        return value;
    }
}