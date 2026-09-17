using UnityEngine;

public class Wanderer : MonoBehaviour
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
    private float seperationDistance = 1.0f;
    private float seperationForce = 2.0f; //force applied to the enemy when it is too close to other enemies 
    private float steeringSmoothness = 8f;
    private Vector2 currentDirection = Vector2.zero;
    private float wanderTimer;
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

        if (distanceToPlayer < distanceToShoot)
        //spotted player, shoot and move towards them 
        {
            WanderAround();
            if (shootTimer >= shootInterval)
            {
                Shoot();
            }
            if ( distanceToPlayer > maxDistance) //stop moving once too close 
                MoveToPlayer();
        } else
        {
            WanderAround();
        }

    }

    void WanderAround()
    {
        wanderTimer += Time.fixedDeltaTime;
        if (wanderTimer >= 1.5f)
        {
            wanderTimer = 0f;
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            currentDirection = Vector2.Lerp(currentDirection, randomDirection, Time.fixedDeltaTime * 20);
        }
        rb.MovePosition(rb.position + new Vector3(currentDirection.x, 0f, currentDirection.y) * speed * Time.fixedDeltaTime);
    }
    void MoveToPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        transform.position += direction.normalized * speed * Time.deltaTime;
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
    void OrbitAroundPlayer()
    {
        Vector3 directionToEnemy = (transform.position - player.position).normalized;
        transform.position = player.position + directionToEnemy * maxDistance;
        transform.RotateAround(player.position, Vector3.up, speed * Time.deltaTime);
    }
}