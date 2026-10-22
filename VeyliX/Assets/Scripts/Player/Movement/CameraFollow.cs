using Unity.AppUI.UI;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 offset = new Vector3(0,5f,0);
    private float smoothSpeed = 50f;
    private Camera cam;
    public float zoom;

    void Awake()
    {
        cam = GetComponent<Camera>();
        updateZoom(zoom+Mathf.Round(Mathf.Floor(player.localScale.x)));
    }
    void updateZoom(float zoom)
    {
        cam.orthographicSize = zoom;
    }
    void FixedUpdate()
    {
        if (player == null) return;
        Vector3 targetPos = player.position + offset;
        //transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.fixedDeltaTime);
        transform.position = targetPos;
    }
}
