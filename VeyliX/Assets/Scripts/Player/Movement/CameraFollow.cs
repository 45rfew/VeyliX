using Unity.AppUI.UI;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 offset = new Vector3(0,105f,0);
    private float smoothSpeed = 50f;
    private Camera cam;
    private float zoom; 

    void Awake()
    {
        cam = GetComponent<Camera>();
        zoom = cam.orthographicSize;
        //updateZoom(zoom+Mathf.Round(Mathf.Floor(player.localScale.x)));
    }
    public void updateZoom(float value)
    {
        cam.orthographicSize = zoom * value;
    }
    void FixedUpdate()
    {
        if (player == null) return;
        Vector3 targetPos = player.position + offset;
        //transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.fixedDeltaTime);
        transform.position = targetPos;
    }
}
