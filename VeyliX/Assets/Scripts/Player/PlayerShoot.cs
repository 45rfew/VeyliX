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
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerStats = FindAnyObjectByType<PlayerStats>();
        rb = GetComponent<Rigidbody>();
        timeBetweenFiring = 1f / playerStats.fireRate;
    }
    void Update()
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
                GameObject newBullet = Instantiate(bullet, bulletTransform[i].position, bulletTransform[i].rotation);
                newBullet.GetComponent<BulletScript>().owner = transform.root.gameObject; //set the owner so bullets cant damage its origin spite 
                float bulletSpeed = playerStats.GetBulletSpeed()[0];
                newBullet.GetComponent<Rigidbody>().linearVelocity = bulletTransform[i].forward * bulletSpeed + rb.linearVelocity;
            }
        }
    }
    public void SetFireRate(float newFireRate)
    {
        timeBetweenFiring = 1f / newFireRate;
    }
}