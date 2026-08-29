public class Cannon
{
    public string name;
    public float piority; 
    public StatRange damage;
    public StatRange fireRate;
    public StatRange bulletSpeed;
    public StatRange recoil;
    public Cannon(
        string name,
        //float piority,
        float damageMin,
        float damageMax,
        float bulletSpeedMin,
        float bulletSpeedMax,
        float fireRateMin,
        float fireRateMax,
        float recoilValue
        )
    {
        this.name = name;
        //this.piority = piority;
        damage = new StatRange(damageMin, damageMax);
        bulletSpeed = new StatRange(bulletSpeedMin, bulletSpeedMax);
        fireRate = new StatRange(fireRateMin, fireRateMax);
        recoil = new StatRange(recoilValue, recoilValue);
    }
}