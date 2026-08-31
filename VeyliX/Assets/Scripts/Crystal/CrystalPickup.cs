using UnityEngine;
using UnityEngine.TestTools;

public class CrystalPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision) {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }    
    }
}