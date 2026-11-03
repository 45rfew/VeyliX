using UnityEngine;

public class Runner2 : MonoBehaviour
{
    public float speed; //follow speed, can be set in inspector
    public float shootInterval;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public int bulletDamage;
    public float maxDistance;
    public float distanceToDash;
    public float dashForce;
    public float dashDuration;
    public float dashCooldown;
    private Transform player;
    private Rigidbody rb;
    private float dashTimer;
    private float steeringSmoothness = 8f;
    private bool isDashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").transform ?? null;
        }
        catch
        {
            return;
        }
    }
    void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }
        dashTimer += Time.fixedDeltaTime;
        if (dashTimer >= dashDuration)
        {
            isDashing = false;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < maxDistance)
        {
            LookAtPlayer();
        }
        MoveForward();
        if (distanceToPlayer < distanceToDash)
        {
            CombatBehaviour();
        }
    }
    void CombatBehaviour()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer <= 60f)
        {
            if (dashTimer >= dashCooldown)
            {
                dashTimer = 0f;
                isDashing = true;
                Dash();
            }
        }
    }
    void MoveForward()
    {
        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();
        rb.MovePosition(rb.position + forward * speed * Time.fixedDeltaTime);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > maxDistance * 10f)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0f;
            directionToPlayer.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * steeringSmoothness));
        }
    }
    void LookAtPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0f;
        directionToPlayer.Normalize();
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer <= 60f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * steeringSmoothness));
        }
    }
    void Dash()
    {
        Vector3 dashDirection = (player.position - transform.position).normalized;
        dashDirection.y = 0f;
        dashDirection.Normalize();
        rb.AddForce(transform.forward, ForceMode.Impulse);
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Runner collided with: " + collision.gameObject.name);
        Vector3 collisionDirection = collision.contacts[0].point - transform.position;
        if (Vector3.Dot(collisionDirection.normalized, transform.forward) < 0f)
        {
            //collision is from the front
        }
        if (collision.gameObject.name == "Gate Segment")
        {
            Quaternion oppositeDirection = Quaternion.LookRotation((transform.position - collision.contacts[0].point).normalized);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, oppositeDirection, Time.fixedDeltaTime * steeringSmoothness));
            //rb.transform.Rotate(0f, 180f, 0f); 
        }
    }

}