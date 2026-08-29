using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bullet;
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
        for (int i = 0; i < cannonTimers.Length; i++)
        {
            cannonTimers[i] += Time.fixedDeltaTime;
        }
        if (!Input.GetMouseButton(0))
            return;
        for (int i = 0; i < ship.cannons.Count; i++)
        {
            Cannon cannonStats = ship.cannons[i];
            float fireRate = cannonStats.fireRate.GetValue();
            float timeBetweenShots = 1f / fireRate;
            float energyCost = cannonStats.damage.GetValue();
            //cannon isn't ready
            if (cannonTimers[i] < timeBetweenShots)
                continue;
            //not enough energy
            if (playerEnergy.GetEnergy() < energyCost)
                continue;
            //make sure we actually have a fire point
            if (i >= bulletTransform.Length)
                continue;
            ShootBullet(bulletTransform[i],cannonStats);
            cannonTimers[i] = 0f;
            playerEnergy.SetEnergy(energyCost);
        }
    }

    void ShootBullet(Transform cannonTransform, Cannon cannonStats)
    {
        GameObject newBullet = Instantiate(bullet,cannonTransform.position,cannonTransform.rotation);
        BulletScript bulletScript = newBullet.GetComponent<BulletScript>();
        bulletScript.owner = transform.root.gameObject;
        float damage = cannonStats.damage.GetValue();
        float bulletSpeed = cannonStats.bulletSpeed.GetValue();
        //set the bullet size based on damage
        float t = Mathf.InverseLerp(1f, 255f, damage);
        float scale = 1f + 69f * Mathf.Pow(t, 0.638f);
        bulletScript.SetDamage(damage);
        newBullet.transform.localScale = Vector3.one * scale;
        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
        Vector3 bulletVelocity = cannonTransform.forward * bulletSpeed + rb.linearVelocity;
        //don't allow the player's velocity to make the bullet exceed its maximum speed
        bulletVelocity = Vector3.ClampMagnitude(bulletVelocity,bulletSpeed);
        bulletRb.linearVelocity = bulletVelocity;
        playerMovement.AddRecoil(-cannonTransform.forward,cannonStats.recoil.GetValue());
    }
}