using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public static CameraController instance;

    public Camera camera;
    protected float zoomSpeed = 0.1f;
    public float zoomIn = 3;
    public float zoomOut = 7;
    protected float zoom;

    void Awake() {
        instance = this;
        zoom = zoomOut;
    }

    void Update() {
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoom, zoomSpeed);
    }

    public static void ZoomIn() {
        instance.zoom = instance.zoomIn;
    }

    public static void ZoomOut() {
        instance.zoom = instance.zoomOut;
    }
}
