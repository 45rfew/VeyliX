public class Cannon
{
    public string name;
    public int priority; 
    public IntStatRange damage;
    public StatRange fireRate;
    public StatRange bulletSpeed;
    public StatRange recoil;
    public Cannon(
        string name,
        int priority,
        int damageMin,
        int damageMax,
        float bulletSpeedMin,
        float bulletSpeedMax,
        float fireRateMin,
        float fireRateMax,
        float recoilValue
        )
    {
        this.name = name;
        this.priority = priority;
        damage = new IntStatRange(damageMin, damageMax);
        bulletSpeed = new StatRange(bulletSpeedMin, bulletSpeedMax);
        fireRate = new StatRange(fireRateMin, fireRateMax);
        recoil = new StatRange(recoilValue, recoilValue);
    }
}