using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class BulletScript : MonoBehaviour 
{
    public GameObject owner;
    private Rigidbody rb;
    private float force;
    private float timeToLive = 5f;
    private PlayerStats playerStats;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
    }

    void Start()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();
        Destroy(gameObject, timeToLive);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.transform.root.name == owner.transform.root.name)
        {
            return;
        }
        
        Health health = collision.gameObject.GetComponent<Health>() ?? null;
        if (health != null)
        {
            health.TakeDamage(playerStats.GetDamage()[0]);
        }
        
        Destroy(gameObject);
    }
    public void SetBulletSpeed(float newSpeed)
    {
        force = newSpeed;
    }
}