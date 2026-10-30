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
            Destroy(gameObject);
            return;
        }
        Vector3 incoming = rb.linearVelocity.sqrMagnitude > 0.0001f
            ? rb.linearVelocity.normalized
            : transform.forward;
        Vector3 horizontalDirection = Vector3.ProjectOnPlane(incoming, Vector3.up);
        if (horizontalDirection.sqrMagnitude < 0.0001f)
        {
            horizontalDirection = Vector3.forward;
        }

        float yaw = Vector3.SignedAngle(Vector3.forward, horizontalDirection, Vector3.up);
        Quaternion effectRotation = Quaternion.Euler(0f, yaw, 0f);
        GameObject effect = Instantiate(collisionEffectPrefab, transform.position, effectRotation);
        effect.transform.localScale = Vector3.one * Mathf.Max(8f, damage / 50f);
        Destroy(effect, 1f);
        Destroy(gameObject, 0.01f);
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