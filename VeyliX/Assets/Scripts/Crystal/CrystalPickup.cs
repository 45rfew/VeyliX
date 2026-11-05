using UnityEditor.Callbacks;
using UnityEngine;

public class CrystalPickup : MonoBehaviour
{
    private int value;
    private float timer;
    private float pickupDelay = 0.38f;
    private Rigidbody rb;
    private float dir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dir = Random.value < 0.5f ? -1f : 1f;
    }
    void Update()
    {
        timer += Time.deltaTime;
        rb.transform.Rotate(0f, 45f * dir * Time.deltaTime, 0f);
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