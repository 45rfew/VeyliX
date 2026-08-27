using System;
using System.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform[] bulletTransform;
    public bool canFire = true;
    private PlayerStats playerStats;
    private Rigidbody rb;
    private float timer;
    private float timeBetweenFiring;
    private PlayerMovement playerMovement;
    private PlayerEnergy playerEnergy;
    private int currentCannon = 0; 
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerStats = FindAnyObjectByType<PlayerStats>();
        rb = GetComponent<Rigidbody>();
        playerEnergy = GetComponent<PlayerEnergy>();
        timeBetweenFiring = 1f / playerStats.fireRate;
    }
    void FixedUpdate()
    {
        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }
        float damage = playerStats.GetDamage()[0];
        Debug.Log(playerEnergy.GetEnergy());
        if (Input.GetMouseButton(0))
        {

            //canFire = false;
            if (playerEnergy.GetEnergy() < damage) return;
            Transform cannon = bulletTransform[currentCannon];


            GameObject newBullet = Instantiate(bullet, cannon.position, cannon.rotation);
            BulletScript bulletScript = newBullet.GetComponent<BulletScript>();
            bulletScript.owner = transform.root.gameObject;
            bulletScript.SetDamage(damage);
            float t = Mathf.InverseLerp(1f, 255f, damage);
            float scale = 1f + 69f * Mathf.Pow(t, 0.638f);
            newBullet.transform.localScale = Vector3.one * scale;
            float bulletSpeed = playerStats.GetBulletSpeed()[0];
            Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
            Vector3 bulletVelocity = cannon.forward * bulletSpeed + rb.linearVelocity;
            bulletVelocity = Vector3.ClampMagnitude(bulletVelocity, bulletSpeed);
            bulletRb.linearVelocity = bulletVelocity;
            playerMovement.AddRecoil(-cannon.forward, playerStats.recoil);
            playerEnergy.SetEnergy(damage);
            currentCannon++;
            if (currentCannon >= bulletTransform.Length) currentCannon = 0;
        }
    }
    void ShootBullet(Transform cannon, float damage)
    {
        GameObject newBullet = Instantiate(bullet, cannon.position, cannon.rotation);
        BulletScript bulletScript = newBullet.GetComponent<BulletScript>();
        bulletScript.owner = transform.root.gameObject;
        bulletScript.SetDamage(damage);
        float t = Mathf.InverseLerp(1f, 255f, damage);
        float scale = 1f + 69f * Mathf.Pow(t, 0.638f);
        newBullet.transform.localScale = Vector3.one * scale;      
        float bulletSpeed = playerStats.GetBulletSpeed()[0];
        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
        Vector3 bulletVelocity = cannon.forward * bulletSpeed + rb.linearVelocity;
        bulletVelocity = Vector3.ClampMagnitude(bulletVelocity, bulletSpeed);
        bulletRb.linearVelocity = bulletVelocity;
        playerMovement.AddRecoil(-cannon.forward, playerStats.recoil);
        
    }
    public void SetFireRate(float newFireRate)
    {
        timeBetweenFiring = 1f / newFireRate;
    }
}