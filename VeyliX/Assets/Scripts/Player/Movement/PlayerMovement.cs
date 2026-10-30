using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerStats playerStats;
    private ZeroLockCheck zeroLockCheck;

    private float moveSpeed;      // ship top speed = max SUSTAINED speed
    private float acceleration;

    [Header("Recoil")]
    [Tooltip("OFF: the direction passed to AddRecoil is already the direction the ship should be pushed. ON: it is the direction you SHOT, so the ship is pushed the opposite way.")]
    public bool pushOppositeOfShot = false;
    [Tooltip("Converts a cannon's recoil stat into an instant speed kick. Kick = cannon recoil * this.")]
    public float recoilStrength = 0.05f;

    [Header("Overspeed (speed above the ship's top speed)")]
    [Tooltip("The most speed recoil can stack up to, as a multiple of the ship's top speed. 2 = recoil can double your top speed for a moment.")]
    public float maxOverspeedMultiplier = 2f;
    [Tooltip("How fast extra speed bleeds off, as a fraction of the ship's acceleration. Higher = boost ends quickly. Lower = longer boost.")]
    [Range(0.01f, 1f)] public float overspeedRecovery = 0.1f;

    [Header("Debug")]
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();
        zeroLockCheck = GetComponent<ZeroLockCheck>();
        UpdateMovementStats();
    }

    public void UpdateMovementStats()
    {
        Ship ship = playerStats.GetCurrentShip();
        moveSpeed = ship.speed.GetValue();
        acceleration = ship.acceleration.GetValue();
    }

    void FixedUpdate()
    {
        if (playerStats == null) return;

        float dt = Time.fixedDeltaTime;
        bool thrusting = Input.GetMouseButton(1);
        bool zeroLocked = zeroLockCheck != null && zeroLockCheck.IsLocked();

        Vector3 v = rb.linearVelocity;
        if (rb == null) return;
        if (rb.transform.localPosition.y != 0f)
        {
            rb.transform.localPosition = new Vector3(rb.transform.localPosition.x, 0f, rb.transform.localPosition.z);
        }
        // 1. Engine
        if (thrusting)
        {
            // Steer velocity toward the nose. If we are currently above top speed
            // (from recoil), keep that extra speed while steering. Recovery below removes it.
            float targetSpeed = Mathf.Max(moveSpeed, v.magnitude);
            Vector3 target = transform.forward * targetSpeed;
            v = Vector3.MoveTowards(v, target, acceleration * dt);
        }
        else if (zeroLocked)
        {
            // Zero lock ON: no free drift, brake to a stop (this also eats recoil boosts quickly)
            v = Vector3.MoveTowards(v, Vector3.zero, acceleration * dt);
        }
        // Zero lock OFF and not thrusting: keep momentum

        // 2. Overspeed recovery: anything above top speed slowly bleeds back down.
        // Better acceleration = faster recovery.
        float mag = v.magnitude;
        if (mag > moveSpeed)
        {
            float newMag = Mathf.MoveTowards(mag, moveSpeed, acceleration * overspeedRecovery * dt);
            v = v.normalized * newMag;
        }

        rb.linearVelocity = v;
        speed = v.magnitude;
    }

    // force = the firing cannon's recoil stat
    public void AddRecoil(Vector3 direction, float force)
    {
        Vector3 pushDir = direction.normalized;
        if (pushOppositeOfShot) pushDir = -pushDir;

        float kick = force * recoilStrength;

        Vector3 v = rb.linearVelocity;
        float cap = moveSpeed * maxOverspeedMultiplier;

        // Can't gain speed past the cap, but if already above it we don't snap down,
        // and you can still change direction.
        float limit = Mathf.Max(cap, v.magnitude);
        rb.linearVelocity = Vector3.ClampMagnitude(v + pushDir * kick, limit);
    }
}