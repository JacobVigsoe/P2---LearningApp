using UnityEngine;

public class ZoomController : MonoBehaviour
{
    public Camera mainCamera;
    public float zoomSpeed = 2f;
    public float targetSize = 2.2f;

    private bool isZooming = false;
    private float initialSize;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        initialSize = mainCamera.orthographicSize;
    }

    private void Update()
    {
        if (isZooming)
        {
            // Smoothly adjust the camera size
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);

            // Check if the camera has reached the target size
            if (Mathf.Abs(mainCamera.orthographicSize - targetSize) < 0.01f)
            {
                isZooming = false;
            }
        }
    }

    public void Zoom()
    {
        isZooming = true;
    }

    public void ResetZoom()
    {
        isZooming = true;
        targetSize = initialSize;
    }
}
