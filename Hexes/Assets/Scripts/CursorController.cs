using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    public GridSpace.SpaceOccupation Player {get; set;}
    private Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = movement;
    }

    public void OnMove(InputValue input) {
        movement = input.Get<Vector2>() * moveSpeed;
    }

    public void OnSelect() {
        Debug.Log("Selected");
    }
}
