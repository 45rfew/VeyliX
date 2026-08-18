using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0,15f,0);
    [SerializeField] private float smoothSpeed = 10f;


    void FixedUpdate()
    {
        if (player == null) return;
        Vector3 targetPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.fixedDeltaTime);
    }
}
