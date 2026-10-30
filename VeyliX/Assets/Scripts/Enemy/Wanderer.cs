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
    private float seekTimer;
    private bool isWandering = true;
    private float stateTimer = 0f;
    private float stateBuffer = 1f;
    enum EnemyState
    {
        Wandering,
        Seeking,
        Shooting
    }
    EnemyState currentState = EnemyState.Wandering;
    float combatMoveTimer = 0f;
    bool isMovingInCombat = false;
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
        UpdateState(distanceToPlayer);
        switch (currentState)
        {
            case EnemyState.Wandering:
                WanderAround();
                break;

            case EnemyState.Seeking:
                ApproachPlayer();
                break;

            case EnemyState.Shooting:
                CombatBehaviour();
                break;
        }
    }
    void UpdateState(float distance)
    {
        EnemyState next = currentState;

        switch (currentState)
        {
            case EnemyState.Wandering:
                if (distance < distanceToShoot) next = EnemyState.Shooting;
                else if (distance < maxDistance) next = EnemyState.Seeking;
                break;

            case EnemyState.Seeking:
                if (distance < distanceToShoot) next = EnemyState.Shooting;
                else if (distance > maxDistance + stateBuffer) next = EnemyState.Wandering;
                break;

            case EnemyState.Shooting:
                if (distance > maxDistance + stateBuffer) next = EnemyState.Wandering;
                else if (distance > distanceToShoot + stateBuffer) next = EnemyState.Seeking;
                break;
        }

        if (next != currentState)
        {
            currentState = next;
            OnStateChanged(next);
        }
    }

    void OnStateChanged(EnemyState newState)
    {
        stateTimer = 0f;
        wanderTimer = 0f;
        combatMoveTimer = 0f;
        isMovingInCombat = false;
        isWandering = (newState != EnemyState.Seeking);
    }
    
    void CombatBehaviour()
    {
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            Shoot();
        }
        /*
        if (combatMoveTimer >= 1f)
        {
            combatMoveTimer = 0f;
            isMovingInCombat = !isMovingInCombat;
        }
        if (isMovingInCombat)
        {
            WanderAround();
        }
        */
        WanderAround();

    }
    void ApproachPlayer()
    {
        stateTimer += Time.fixedDeltaTime;

        if (stateTimer >= 1f)
        {
            stateTimer = 0f;
            isWandering = !isWandering;
        }

        if (isWandering)
        {
            WanderAround();
        }
        else
        {
            MoveToPlayer();
        }
    }

    void WanderAround()
    {
        wanderTimer += Time.fixedDeltaTime;
        if (wanderTimer >= 1f)
        {
            wanderTimer = 0f;
            isWandering = true;
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            currentDirection = Vector2.Lerp(currentDirection, randomDirection, Time.fixedDeltaTime * 20);
        }
        rb.MovePosition(rb.position + new Vector3(currentDirection.x, 0f, currentDirection.y) * speed * Time.fixedDeltaTime);
    }


    void MoveToPlayer()
    {
        if (!isWandering)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            direction.Normalize();
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
    }

    void MoveToPlayer3()
    {
        seekTimer += Time.fixedDeltaTime;
        if (seekTimer >= 0.5f)
        {

            seekTimer = 0f;
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            // Add random movement
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f),
                0f,
                Random.Range(-1f, 1f)
            );

            // Combine chasing and random movement
            direction += randomDirection * 0.5f;

            transform.position += direction.normalized * speed * Time.deltaTime;
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