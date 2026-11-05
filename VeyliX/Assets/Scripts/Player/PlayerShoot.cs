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
    private int nextCannon = 0;
    void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerEnergy = GetComponentInParent<PlayerEnergy>();
        rb = GetComponentInParent<Rigidbody>();
        EnsureCannonTimers();
    }
    void FixedUpdate()
    {
        EnsureCannonTimers();
        for (int i = 0; i < cannonTimers.Length; i++){
            cannonTimers[i] += Time.fixedDeltaTime;
        }
        if (!Input.GetMouseButton(0)) return;
        TryShoot();        
    }
    void TryShoot()
    {
        Ship ship = playerStats.GetCurrentShip();
        int cannonCount = ship.cannons.Count;
        int selectedCannon = -1;
        int highestPriority = int.MinValue;
        for (int offset = 0; offset < cannonCount; offset++)
        {
            int i = (nextCannon + offset) % cannonCount;
            Cannon cannon = ship.cannons[i];
            float fireRate = cannon.fireRate.GetValue();
            float timeBetweenShots = 1f / fireRate;
            int energyCost = cannon.damage.GetValue();
            //cannon not ready
            if (cannonTimers[i] < timeBetweenShots) continue;
            //cant afford
            if (playerEnergy.GetEnergy() < energyCost) continue;
            //no fire point
            if (i >= bulletTransform.Length) continue;
            //priority is more important than rotation
            if (cannon.priority > highestPriority)
            {
                highestPriority = cannon.priority;
                selectedCannon = i;
            }
        }
        if (selectedCannon == -1) return;
        Cannon selected = ship.cannons[selectedCannon];
        ShootBullet(bulletTransform[selectedCannon],selected);
        cannonTimers[selectedCannon] = 0f;
        playerEnergy.SetEnergy(selected.damage.GetValue());
        //start searching from the next cannon next time
        nextCannon = (selectedCannon + 1) % cannonCount;
    }
    void ShootBullet(Transform cannonTransform, Cannon cannonStats)
    {
        GameObject newBullet = Instantiate(bullet, cannonTransform.position, cannonTransform.rotation);
        BulletScript bulletScript = newBullet.GetComponent<BulletScript>();
        bulletScript.owner = transform.root.gameObject;
        int damage = cannonStats.damage.GetValue();
        float bulletSpeed = cannonStats.bulletSpeed.GetValue();
        //set the bullet size based on damage
        float t = Mathf.InverseLerp(1f, 255f, damage);
        float scale = 1f + 69f * Mathf.Pow(t, 0.638f);
        float error = cannonStats.error;
        bulletScript.SetDamage(damage);
        newBullet.transform.localScale = Vector3.one * scale;
        newBullet.transform.Rotate( Random.Range(-error, error), 0f, 0f );
        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
        Vector3 bulletVelocity = cannonTransform.forward + newBullet.transform.forward * bulletSpeed + rb.linearVelocity;
        //don't allow the player's velocity to make the bullet exceed its maximum speed
        bulletVelocity = Vector3.ClampMagnitude(bulletVelocity, bulletSpeed);
        bulletRb.linearVelocity = bulletVelocity;
        playerMovement.AddRecoil(-cannonTransform.forward, cannonStats.recoil.GetValue());
        SoundManager.PlaySound(SoundType.Shoot,damage);
    }
    public void ResetCannons(){
        EnsureCannonTimers();
    }

    private void EnsureCannonTimers()
    {
        if (playerStats == null)
        {
            playerStats = GetComponentInParent<PlayerStats>();
        }

        Ship ship = playerStats.GetCurrentShip();
        if (cannonTimers == null || cannonTimers.Length != ship.cannons.Count)
        {
            cannonTimers = new float[ship.cannons.Count];
            nextCannon = 0;
        }
    }
}