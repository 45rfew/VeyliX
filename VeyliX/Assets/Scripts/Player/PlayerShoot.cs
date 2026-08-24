using System;
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
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerStats = FindAnyObjectByType<PlayerStats>();
        rb = GetComponent<Rigidbody>();
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

        if (Input.GetMouseButton(0) && canFire)
        {
            canFire = false;

            for (int i = 0; i < bulletTransform.Length; i++)
            {
                GameObject newBullet = Instantiate(
                    bullet,
                    bulletTransform[i].position,
                    bulletTransform[i].rotation
                );

                BulletScript bulletScript = newBullet.GetComponent<BulletScript>();
                bulletScript.owner = transform.root.gameObject;
                bulletScript.SetDamage(playerStats.GetDamage()[0]);
                float bulletSpeed = playerStats.GetBulletSpeed()[0];
                Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
                Vector3 bulletVelocity = bulletTransform[i].forward * bulletSpeed + rb.linearVelocity;
                bulletVelocity = Vector3.ClampMagnitude(bulletVelocity, bulletSpeed);
                bulletRb.linearVelocity = bulletVelocity;
                playerMovement.AddRecoil(-bulletTransform[i].forward,playerStats.recoil);

            }



        }
    }
    public void SetFireRate(float newFireRate)
    {
        timeBetweenFiring = 1f / newFireRate;
    }
}