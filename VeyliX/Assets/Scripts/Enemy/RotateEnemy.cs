using UnityEngine;

public class RotateEnemy : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 20f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb == null)
        {
            return;
        }
        transform.Rotate(0f, 0f, Time.fixedDeltaTime * speed);
    }
}