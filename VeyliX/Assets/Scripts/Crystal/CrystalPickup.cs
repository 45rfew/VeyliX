using UnityEngine;
using UnityEngine.TestTools;

public class CrystalPickup : MonoBehaviour
{

    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider collision) {
        if (collision.CompareTag("Player"))
        {
            Debug.Log(transform.parent.localScale.x);
            Destroy(gameObject);
        }    
    }
}