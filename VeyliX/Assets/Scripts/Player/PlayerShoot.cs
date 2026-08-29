using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bullet;

    // These will eventually come from the ship model
    public Transform[] bulletTransform;

    private PlayerStats playerStats;
    private PlayerMovement playerMovement;
    private PlayerEnergy playerEnergy;
    private Rigidbody rb;

    private float[] cannonTimers;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerMovement = GetComponent<PlayerMovement>();
        playerEnergy = GetComponent<PlayerEnergy>();
        rb = GetComponent<Rigidbody>();

        Ship ship = playerStats.GetCurrentShip();
        cannonTimers = new float[ship.cannons.Count];
    }

    void FixedUpdate()
    {
        Ship ship = playerStats.GetCurrentShip();
        // Update timers
        for (int i = 0; i < cannonTimers.Length; i++)
        {
            cannonTimers[i] += Time.fixedDeltaTime;
        }
        if (!Input.GetMouseButton(0))
            return;

        // Check every cannon
        for (int i = 0; i < ship.cannons.Count; i++)
        {
            Cannon cannonStats = ship.cannons[i];

            float fireRate = cannonStats.fireRate.GetValue();
            float timeBetweenShots = 1f / fireRate;

            float energyCost = cannonStats.energyCost.GetValue();

            // Cannon isn't ready
            if (cannonTimers[i] < timeBetweenShots)
                continue;
            // Not enough energy
            if (playerEnergy.GetEnergy() < energyCost)
                continue;
            // Make sure we actually have a fire point
            if (i >= bulletTransform.Length)
                continue;

            // Fire
            ShootBullet(
                bulletTransform[i],
                cannonStats
            );

            // Reset this cannon's timer
            cannonTimers[i] = 0f;

            // Consume energy
            playerEnergy.SetEnergy(energyCost);
        }
    }

    void ShootBullet(Transform cannonTransform, Cannon cannonStats)
    {
        GameObject newBullet = Instantiate(
            bullet,
            cannonTransform.position,
            cannonTransform.rotation
        );

        BulletScript bulletScript =
            newBullet.GetComponent<BulletScript>();

        bulletScript.owner = transform.root.gameObject;

        float damage = cannonStats.damage.GetValue();
        float bulletSpeed = cannonStats.bulletSpeed.GetValue();
        float bulletSize = cannonStats.bulletSize.GetValue();

        bulletScript.SetDamage(damage);

        // Apply bullet size
        newBullet.transform.localScale =
            Vector3.one * bulletSize;

        // Give bullet its velocity
        Rigidbody bulletRb =
            newBullet.GetComponent<Rigidbody>();

        Vector3 bulletVelocity =
            cannonTransform.forward * bulletSpeed
            + rb.linearVelocity;

        // Don't allow the player's velocity to make
        // the bullet exceed its maximum speed
        bulletVelocity =
            Vector3.ClampMagnitude(
                bulletVelocity,
                bulletSpeed
            );

        bulletRb.linearVelocity = bulletVelocity;

        playerMovement.AddRecoil(-cannonTransform.forward,cannonStats.recoil);
    }
}