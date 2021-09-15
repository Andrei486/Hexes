using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Extensions;

public class CursorController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    public GridSpace.SpaceOccupation Player {get; set;}
    private Vector3 movement;
    [SerializeField] private GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started");
        SwitchPlayer();
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
        Debug.Log("Pressed select");
        GridSpace selectedSpace;
        Collider2D[] colliders = new Collider2D[1];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        LayerMask mask = Physics2D.GetLayerCollisionMask(LayerMask.NameToLayer("Grid"));
        filter.SetLayerMask(mask);
        //return only one collider: cursor hitbox should be small enough that the space between hexes
        //guarantees that only one is selected at a time
        int resultCount = Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, colliders);
        Debug.Log(resultCount);
        if (resultCount == 1) {
            selectedSpace = colliders[0].gameObject.GetComponent<GridSpace>();
            Debug.Log(selectedSpace.Position);
            if (selectedSpace.Occupation == GridSpace.SpaceOccupation.Free) {
                selectedSpace.Occupation = Player;
                SwitchPlayer();
                gridManager.ExpandGrid();
            }
        }
    }

    public void SwitchPlayer() {
        do {
            Player = Player.Next<GridSpace.SpaceOccupation>();
        } while (Player == GridSpace.SpaceOccupation.Free);
        GetComponent<SpriteRenderer>().material = gridManager.GetTokenMaterial(Player);
    }
}