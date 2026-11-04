using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{
    private Camera cam;
    private float zoomSpeed = 1f;
    private float targetZoom = 180f;
    private float zoomTime = 60f;
    private float zoomTimer = 0f;
    private bool isZooming = false;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (!isZooming) return;
        if (zoomTimer < zoomTime)
        {
            zoomTimer += Time.deltaTime;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetZoom, zoomTimer / zoomTime);
        }
    }   
    
    public void ZoomOut()
    {
        isZooming = true;
        zoomTimer = 0f;
    }

}