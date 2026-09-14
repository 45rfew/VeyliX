using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerStats playerStats;
    private ZeroLockCheck zeroLockCheck;

    private float moveSpeed;
    private float acceleration;
    private float recoilDuration = 0.5f;
    private float recoilMaxSpeed; //hard cap while recoil is active
    private Vector3 recoilDirection;
    private float recoilForce;
    private float recoilTimer;
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();
        zeroLockCheck = GetComponent<ZeroLockCheck>();
        Ship ship = playerStats.GetCurrentShip();
        recoilMaxSpeed = ship.speed.GetValue() * 1f;
        moveSpeed = ship.speed.GetValue();
        acceleration = ship.acceleration.GetValue();
    }
    public void UpdateMovementStats()
    {
        Ship ship = playerStats.GetCurrentShip();
        recoilMaxSpeed = ship.speed.GetValue() * 1f;
        moveSpeed = ship.speed.GetValue();
        acceleration = ship.acceleration.GetValue();
    }
    void FixedUpdate()
    {
        if (playerStats == null) return;
        speed = rb.linearVelocity.magnitude;
        bool thrusting = Input.GetMouseButton(1);
        bool recoiling = recoilTimer > 0f;
        if (thrusting)
        {
            float forwardSpeed = Vector3.Dot(rb.linearVelocity,transform.forward);
            float speedDifference = moveSpeed - forwardSpeed;
            if (speedDifference > 0f)
            {
                float accelerationAmount = Mathf.Min(acceleration,speedDifference / Time.fixedDeltaTime);
                rb.AddForce(transform.forward * accelerationAmount,ForceMode.Acceleration);
            }
        }
        if (recoiling)
        {
            recoilTimer -= Time.fixedDeltaTime;
            float progress = 1f - Mathf.Clamp01(recoilTimer / recoilDuration);
            float curve = Mathf.Sin(progress * Mathf.PI); // smooth arch: 0 -> 1 -> 0
            float speedAlongRecoil = Vector3.Dot(rb.linearVelocity, recoilDirection);
            if (speedAlongRecoil < recoilMaxSpeed)
            {
                rb.AddForce(recoilDirection * recoilForce * curve, ForceMode.Acceleration);
            }
            if (rb.linearVelocity.magnitude > moveSpeed*1.2f)
            {
                float currentSpeed = rb.linearVelocity.magnitude;
                float newSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, acceleration * Time.fixedDeltaTime);
                rb.linearVelocity = rb.linearVelocity.normalized * newSpeed;
            }
        }
        
        
        //brake once recoil has fully finished
        if (!thrusting && zeroLockCheck.IsLocked())
        {
            rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity,Vector3.zero,acceleration * Time.fixedDeltaTime);
        }
        else if (rb.linearVelocity.magnitude > moveSpeed)
        {
            float currentSpeed = rb.linearVelocity.magnitude;
            float newSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, acceleration * Time.fixedDeltaTime);
            rb.linearVelocity = rb.linearVelocity.normalized * newSpeed;
        }
    }

    public void AddRecoil(Vector3 direction, float force)
    {
        recoilDirection = direction.normalized;
        recoilForce = force;
        recoilTimer = recoilDuration;
    }
}