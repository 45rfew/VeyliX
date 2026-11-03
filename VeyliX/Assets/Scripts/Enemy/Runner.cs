using UnityEngine;

public class Runner : MonoBehaviour
{
    public float speed; //follow speed, can be set in inspector
    public float shootInterval;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public int bulletDamage;
    public float maxDistance;
    public float distanceToShoot;
    private Transform player;
    private Rigidbody rb;
    private float shootTimer;
    private float steeringSmoothness = 8f;

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
        shootTimer += Time.fixedDeltaTime;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < maxDistance)
        {
            LookAtPlayer();
        }
        MoveForward();
        if (distanceToPlayer < distanceToShoot)
        {
            CombatBehaviour();
        }
    }
    void CombatBehaviour()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer <= 35f)
        {
            if (shootTimer >= shootInterval)
            {
                shootTimer = 0f;
                Shoot();
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
        if (angleToPlayer <= 35f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * steeringSmoothness));
        }
    }
    void Shoot()
    {
        rb.linearVelocity = Vector2.zero;
        BulletScript bullet = Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<BulletScript>();
        if (bullet != null)
        {
            bullet.owner = transform.root.gameObject;
            int damage = bulletDamage;
            float bulletSpeed = this.bulletSpeed;
            //set the bullet size based on damage
            float t = Mathf.InverseLerp(1f, 255f, damage);
            float scale = 1f + 69f * Mathf.Pow(t, 0.638f);
            bullet.SetDamage(damage);
            bullet.transform.localScale = Vector3.one * scale;
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            Vector3 direction = player.position - transform.position;
            Vector3 bulletVelocity = direction.normalized * bulletSpeed;
            bulletVelocity = Vector3.ClampMagnitude(bulletVelocity, bulletSpeed);
            bulletRb.rotation = Quaternion.LookRotation(direction);
            bulletRb.linearVelocity = bulletVelocity;
        }
        shootTimer = 0;
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