using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    private Vector3 target;
    private static float zoomFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        target = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        camera.transform.position = Vector3.MoveTowards(camera.transform.position, target, CursorController.MoveSpeed * 3f * Time.deltaTime);
    }

    public void OnSelect() {
        target = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);
    }

    public void OnZoomCamera(InputValue input) {
        float newSize = camera.orthographicSize - input.Get<float>() * zoomFactor;
        CursorController.MoveSpeed *= newSize/camera.orthographicSize;
        camera.orthographicSize = newSize;
    }
}
