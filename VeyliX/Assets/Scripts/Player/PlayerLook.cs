using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private Camera viewCamera;

    void Start()
    {
        viewCamera = Camera.main;
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
                transform.rotation = Quaternion.LookRotation(direction);
            }

        }
    }
}