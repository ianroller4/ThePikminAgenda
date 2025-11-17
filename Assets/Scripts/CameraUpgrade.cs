using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraUpgrade : MonoBehaviour
{
    Camera cam;

    [Header("Zoom")]
    // --- Zoom Variables ---
    public float minZoom = 5f;
    public float maxZoom = 20f;
    public float zoomSpeed = 3f;
    public bool useCurrentZoomAsMin = true;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();

        if (useCurrentZoomAsMin)
        {
            minZoom = cam.orthographicSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
    }

    public void Zoom()
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - Input.mouseScrollDelta.y * zoomSpeed, minZoom, maxZoom);
    }
}
