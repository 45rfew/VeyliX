using UnityEngine;

public class Wander : MonoBehaviour
{
    public float speed; //follow speed, can be set in inspector
    public float shootInterval;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float bulletDamage;
    public float maxDistance; 
    private Transform player;
    private Rigidbody rb;
    private float shootTimer;
    private float seperationDistance = 1.0f;
    private float seperationForce = 2.0f; //force applied to the enemy when it is too close to other enemies 
    private float steeringSmoothness = 8f;
    private Vector2 currentDirection = Vector2.zero;
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
        if (player == null) return;
        shootTimer += Time.fixedDeltaTime;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector3 direction = player.position - transform.position;
        direction.y = 0f; // Keep the direction in the horizontal plane
        rb.rotation = Quaternion.LookRotation(direction);



        if (distanceToPlayer > maxDistance)
        {
            Collider2D[] enemiesNearby = Physics2D.OverlapCircleAll(transform.position, seperationDistance);

            Vector2 toPlayer = ((Vector2)player.position - (Vector2)transform.position).normalized;
            Vector2 seperationVector = Vector2.zero;
            foreach (Collider2D enemy in enemiesNearby)
            {
                if (enemy.gameObject == gameObject) continue; //skip self
                if (!enemy.CompareTag("Enemy")) continue; //skip non-enemies
                Vector2 awayFromEnemy = (Vector2)transform.position - (Vector2)enemy.transform.position;
                float distance = awayFromEnemy.magnitude;
                if (distance > 0)
                {
                    seperationVector += awayFromEnemy.normalized / distance; //the closer the enemy, the stronger the force
                }
            }




            //Vector2 direction = (toPlayer + seperationVector * seperationForce).normalized;

            Vector2 targetDirection = (toPlayer + seperationVector * seperationForce).normalized;
            currentDirection = Vector2.Lerp(currentDirection, targetDirection, steeringSmoothness * Time.deltaTime).normalized;
            rb.linearVelocity = currentDirection * speed;


        }
        else
        {
            if (shootTimer >= shootInterval)
            {
                rb.linearVelocity = Vector2.zero;
                BulletScript bullet = Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<BulletScript>();
                if (bullet != null)
                {
                    bullet.owner = transform.root.gameObject;
                    float damage = bulletDamage;
                    float bulletSpeed = this.bulletSpeed;
                    //set the bullet size based on damage
                    float t = Mathf.InverseLerp(1f, 255f, damage);
                    float scale = 1f + 69f * Mathf.Pow(t, 0.638f);
                    bullet.SetDamage(damage);
                    bullet.transform.localScale = Vector3.one * scale;
                    Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                    Vector3 bulletVelocity = transform.forward * bulletSpeed;
                    bulletVelocity = Vector3.ClampMagnitude(bulletVelocity, bulletSpeed);
                    bulletRb.linearVelocity = bulletVelocity;
                }
                shootTimer = 0;
            }
        }
    }
    void SteerFromEnemies()
    {
        Collider2D[] enemiesNearby = Physics2D.OverlapCircleAll(transform.position, seperationDistance);
        Vector2 toPlayer = ((Vector2)player.position - (Vector2)transform.position).normalized;
        Vector2 seperationVector = Vector2.zero;
        foreach (Collider2D enemy in enemiesNearby)
        {
            if (enemy.gameObject == gameObject) continue; //skip self
            if (!enemy.CompareTag("Enemy")) continue; //skip non-enemies
            Vector2 awayFromEnemy = (Vector2)transform.position - (Vector2)enemy.transform.position;
            float distance = awayFromEnemy.magnitude;
            if (distance > 0)
            {
                seperationVector += awayFromEnemy.normalized / distance; //the closer the enemy, the stronger the force
            }
        }
    }
}