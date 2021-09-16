using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public static float MoveSpeed = 8.0f;
    public Camera camera;
    private static float zoomFactor = 0.5f;
    private Vector2 movement;
    private Vector3? target;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, target.Value, MoveSpeed * 2f * Time.deltaTime);
        }
    }

    public void OnSelect() {
        target = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);
    }

    public void OnMoveCamera(InputValue input) {
        movement = input.Get<Vector2>() * MoveSpeed;
        if (input.Get<Vector2>() != Vector2.zero) {
            target = null;
        }
        camera.GetComponent<Rigidbody2D>().velocity = movement;
    }

    public void OnZoomCamera(InputValue input) {
        float newSize = camera.orthographicSize - input.Get<float>() * zoomFactor;
        CursorController.MoveSpeed *= newSize/camera.orthographicSize;
        MoveSpeed *= newSize/camera.orthographicSize;
        camera.orthographicSize = newSize;
    }
}
