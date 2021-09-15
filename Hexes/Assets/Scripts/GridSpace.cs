using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace : MonoBehaviour
{
    private Color deselectedColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color selectedColor = new Color(1.0f, 0.7f, 0.0f, 1.0f);
    public enum SpaceOccupation {Free, Red, Blue}

    public int X {get; set;}
    public int Y {get; set;}
    public Vector2Int Position
    {
        get {
            return new Vector2Int(X, Y);
        }
        set {
            X = value.x;
            Y = value.y;
        }
    }
    private SpaceOccupation occupation = SpaceOccupation.Free;
    public SpaceOccupation Occupation
    {
        get {
            return occupation;
        }
        set {
            if (value == SpaceOccupation.Free && occupation != SpaceOccupation.Free) {
                Free();
            } else if (value != SpaceOccupation.Free && occupation == SpaceOccupation.Free) {
                Occupy(value);
            }
        }
    }
    private GameObject token = null;
    // Start is called before the first frame update
    void Start() {
        
    }
    // Update is called once per frame
    void Update() {
        
    }
    void Free() {
        occupation = SpaceOccupation.Free;
        Destroy(token);
        token = null;
    }
    void Occupy(SpaceOccupation newOccupation) {
        occupation = newOccupation;
        GridManager grid = GridManager.GetGridManager();
        token = Instantiate(grid.tokenPrefab, transform);
        SpriteRenderer spriteRenderer = token.GetComponent<SpriteRenderer>();
        spriteRenderer.material = grid.GetTokenMaterial(newOccupation);
    }
    public bool IsAdjacent(GridSpace other) {
        foreach (Vector2Int adjacency in GridManager.adjacencies) {
            if (Position - other.Position == adjacency) {
                return true;
            }
        }
        return false;
    }
}
