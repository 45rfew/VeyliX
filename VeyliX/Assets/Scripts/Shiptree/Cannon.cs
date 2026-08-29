public class Cannon
{
    public string name;
    public float piority; 
    public StatRange damage;
    public StatRange energyCost;
    public StatRange fireRate;
    public StatRange bulletSpeed;
    public StatRange bulletSize;
    public StatRange recoil;
    public Cannon(
        string name,
        float piority,
        float damageMin,
        float damageMax,
        float energyMin,
        float energyMax,
        float fireRateMin,
        float fireRateMax,
        float bulletSpeedMin,
        float bulletSpeedMax,
        float bulletSizeMin,
        float bulletSizeMax,
        float recoilValue
        )
    {
        this.name = name;
        this.piority = piority;
        damage = new StatRange(damageMin, damageMax);
        energyCost = new StatRange(damageMin, damageMax);
        fireRate = new StatRange(fireRateMin, fireRateMax);
        bulletSpeed = new StatRange(bulletSpeedMin, bulletSpeedMax);
        bulletSize = new StatRange(bulletSizeMin, bulletSizeMax);
        recoil = new StatRange(recoilValue, recoilValue);
    }
}