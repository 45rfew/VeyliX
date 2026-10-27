using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletScript : MonoBehaviour
{
    public GameObject owner;
    private Rigidbody rb;
    private float force;
    private int damage;
    private float timeToLive = 5f;
    public GameObject collisionEffectPrefab;
    [SerializeField] private float effectScaleMultiplier = 0.3f;
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
            if (collision.transform.root == owner.transform.root) return;
        }
        Debug.Log("Bullet hit: " + collision.gameObject.name);
        Health health = collision.GetComponentInParent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    if (collisionEffectPrefab == null)
    {
        Debug.LogError("Collision Effect Prefab is NOT assigned!");
        Destroy(gameObject);
        return;
    }

    // Test: spawn directly at the bullet's position
    GameObject effect = Instantiate(
        collisionEffectPrefab,
        transform.position,
        Quaternion.identity
    );

    effect.transform.localScale = Vector3.one * 3f;

    Debug.Log("Spawned effect: " + effect.name);
    Debug.Log("Effect position: " + effect.transform.position);
    Debug.Log("Effect active: " + effect.activeInHierarchy);

        Destroy(gameObject, 0.01f);
    }
    void OnCollisionEnter(Collision collision)
    {
         if (collisionEffectPrefab == null)
        {
            return;
        }
        Vector3 size = collision.transform.localScale;
        ContactPoint contact = collision.GetContact(0);
        Quaternion rotation = Quaternion.LookRotation(contact.normal);
        float effectScale = size.magnitude * effectScaleMultiplier;
        GameObject effect = Instantiate(collisionEffectPrefab, contact.point, rotation);
        effect.transform.localScale = Vector3.one * effectScale;       
    }
    public void SetBulletSpeed(float newSpeed)
    {
        force = newSpeed;
    }
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
}