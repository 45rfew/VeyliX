using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletScript : MonoBehaviour
{
    public GameObject owner;
    private Rigidbody rb;
    private float force;
    private float damage;
    private float timeToLive = 5f;
    //todo: make the bullet life base off its distance traveled
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
    }

    void Start()
    {
        Destroy(gameObject, timeToLive);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (owner != null)
        {
            if (collision.transform.root == owner.transform.root)
                return;
        }
        Debug.Log("Bullet hit: " + collision.gameObject.name);
        Health health = collision.GetComponentInParent<Health>();
        if (health != null)
            health.TakeDamage(damage);
        Destroy(gameObject);
    }

    public void SetBulletSpeed(float newSpeed)
    {
        force = newSpeed;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
}