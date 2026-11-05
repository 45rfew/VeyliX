using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rammer : MonoBehaviour
{
    public float speed; //follow speed, can be set in inspector
    public float maxDistance;
    public float distanceToDash;
    public float dashForce;
    public float dashDuration;
    public float dashCooldown;
    private Transform player;
    private Rigidbody rb;
    private float dashTimeRemaining;
    private float dashCooldownRemaining;
    private float steeringSmoothness = 8f;
    private bool isDashing = false;
    private float directionChangeCooldown = 2f;
    private float directionChangeTimer = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }
    void FixedUpdate()
    {
        if (player == null || rb == null)
        {
            return;
        }
        directionChangeTimer += Time.fixedDeltaTime;
        dashCooldownRemaining = Mathf.Max(0f, dashCooldownRemaining - Time.fixedDeltaTime);
        if (isDashing)
        {
            dashTimeRemaining -= Time.fixedDeltaTime;
            if (dashTimeRemaining > 0f)
            {
                return;
            }
            isDashing = false;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < maxDistance)
        {
            LookAtPlayer();
        }

        if (distanceToPlayer < distanceToDash && dashCooldownRemaining <= 0f)
        {
            CombatBehaviour();
            if (isDashing)
            {
                return;
            }
        }
        MoveForward();
    }
    void CombatBehaviour()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;
        if (directionToPlayer.sqrMagnitude < 0.0001f)
        {
            return;
        }
        directionToPlayer.Normalize();
        dashTimeRemaining = Mathf.Max(0f, dashDuration);
        dashCooldownRemaining = Mathf.Max(0f, dashCooldown);
        isDashing = dashTimeRemaining > 0f;
        Dash(directionToPlayer);
    }
    void MoveForward()
    {
        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();
        rb.MovePosition(rb.position + forward * speed * Time.fixedDeltaTime);
        if (directionChangeTimer >= directionChangeCooldown)
        {
            LookRandomDirection();
            directionChangeTimer = 0f;
        }
    }
    void LookRandomDirection()
    {
        if (directionChangeTimer < directionChangeCooldown)
        {
            return;
        }
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 direction = new Vector3(randomDirection.x, 0f, randomDirection.y);
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * steeringSmoothness));
    }
    void LookAtPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;
        if (directionToPlayer.sqrMagnitude < 0.0001f)
        {
            return;
        }
        directionToPlayer.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * steeringSmoothness));
    }
    void Dash(Vector3 dashDirection)
    {
        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 collisionDirection = collision.contacts[0].point - transform.position;
        if (Vector3.Dot(collisionDirection.normalized, transform.forward) < 0f)
        {
            //collision is from the front
        }
        if (collision.gameObject.name == "Gate Segment" || collision.gameObject.name == "Asteroid")
        {
            Quaternion oppositeDirection = Quaternion.LookRotation((transform.position - collision.contacts[0].point).normalized);
            //rb.MoveRotation(Quaternion.Slerp(rb.rotation, oppositeDirection, Time.fixedDeltaTime * steeringSmoothness));
            rb.transform.Rotate(0f, UnityEngine.Random.Range(-80,80), 0f); 
        }
    }
}