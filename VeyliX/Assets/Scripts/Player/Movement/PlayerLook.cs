using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private PlayerStats playerStats;
    private Camera viewCamera;
    private float rotation; 
    void Start()
    {
        viewCamera = Camera.main;
        playerStats = GetComponent<PlayerStats>();
        //rotation = playerStats.GetRotationSpeed()[0];
    }

    void Update()
    {
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);

            Vector3 direction = point - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                Ship ship = playerStats.GetCurrentShip();
                rotation = ship.rotationSpeed.GetValue();
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotation * Time.deltaTime);
            }
        }
    }
}