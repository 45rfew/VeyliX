using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    void Start()
    {

    }


    void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
        }
    }
}
