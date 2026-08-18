using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            Debug.Log("Right mouse button is held down");
            rb.MovePosition(
                rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime
            );
        }
    }
}