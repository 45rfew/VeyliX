using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerStats playerStats;
    private ZeroLockCheck zeroLockCheck;
    [SerializeField] private ParticleSystem[] flamePrefabs;
    private float mass;
    private float moveSpeed ;
    private float acceleration;
    private float rotationSpeed;
    private Vector3 recoilVelocity;
    private float recoilDecay = 10f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();
        zeroLockCheck = GetComponent<ZeroLockCheck>();
        Ship ship = playerStats.GetCurrentShip();
        moveSpeed = ship.speed.GetValue();
        acceleration = ship.acceleration.GetValue();
        rotationSpeed = ship.rotationSpeed.GetValue();
        mass = ship.mass.GetValue();

    }

    void Awake()
    {
        flamePrefabs = GetComponentsInChildren<ParticleSystem>(true);
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity;

        if (Input.GetMouseButton(1))
        {
            targetVelocity = transform.forward * moveSpeed;
            foreach (ParticleSystem flame in flamePrefabs ?? System.Array.Empty<ParticleSystem>())
            {
                if (flame != null)
                {
                    flame.Play();
                }
            }
        }
        else
        {
            foreach (ParticleSystem flame in flamePrefabs ?? System.Array.Empty<ParticleSystem>())
            {
                if (flame != null)
                {
                    flame.Stop(
                        false,
                        ParticleSystemStopBehavior.StopEmitting
                    );
                }
            }
            if (zeroLockCheck.IsLocked())
            {
                targetVelocity = Vector3.zero;
            }
            else
            {
                targetVelocity = rb.linearVelocity;
            }
        }
        Vector3 movementVelocity = rb.linearVelocity - recoilVelocity;
        movementVelocity = Vector3.MoveTowards(movementVelocity,targetVelocity,acceleration * Time.fixedDeltaTime);
        recoilVelocity = Vector3.MoveTowards(recoilVelocity,Vector3.zero,recoilDecay * Time.fixedDeltaTime);
        rb.linearVelocity = movementVelocity + recoilVelocity;
    }

    public void AddRecoil(Vector3 direction, float strength)
    {
        recoilVelocity += direction.normalized * strength;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        //get the direction 
        Vector3 collisionNormal = collision.contacts[0].normal;
        Vector3 velocity = rb.linearVelocity;
        if (velocity.magnitude <= 0.1f) return;

        //check how direct we are hitting it - 0 = glancing, 1 = direct
        float impactAngle = Mathf.Abs(Vector3.Dot(velocity, collisionNormal));
        float referenceMass = 100f;
        float massEffect = referenceMass / mass;
        float baseSlowdown = 0.3f;
        float slowdownFactor = baseSlowdown * impactAngle * massEffect;
        slowdownFactor = Mathf.Clamp(slowdownFactor, 0.3f, 0.6f);
        rb.linearVelocity *= slowdownFactor;
    }
}