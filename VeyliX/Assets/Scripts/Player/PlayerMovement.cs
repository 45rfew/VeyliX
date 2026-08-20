using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerStats playerStats;
    private ZeroLockCheck zeroLockCheck;
    private float mass;
    private float moveSpeed ;
    private float acceleration;
    private float rotationSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();
        zeroLockCheck = GetComponent<ZeroLockCheck>();
        mass = playerStats.GetMass();
        moveSpeed = playerStats.GetSpeed()[0];
        acceleration = playerStats.GetAcceleration()[0];
        rotationSpeed = playerStats.GetRotationSpeed()[0];

    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 targetVelocity = transform.forward * moveSpeed;
            rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else if (zeroLockCheck.IsLocked())
        {
            rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, Vector3.zero, acceleration * Time.fixedDeltaTime);
        } 
        else
        {
            return;
        }
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