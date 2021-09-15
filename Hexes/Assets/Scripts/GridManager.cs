using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridManager : MonoBehaviour
{
    //Array of Vector2Ints indicating which spaces on a grid are adjacent to a given space.
    //Could be simplified by auto-generating the second half, which is just the first half * -1.
    public static Vector2Int[] adjacencies = {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(1, -1),
    };
    [SerializeField] private Vector3 columnOffsetVector;
    [SerializeField] private Vector3 rowOffsetVector;
    [SerializeField] private GameObject hexPrefab;
    private Dictionary<Vector2Int, GridSpace> gridSpaces;
    public GameObject tokenPrefab;
    public TokenColor[] tokenColors;
    private static GridManager gridManager;
    void Start()
    {
        InitializeGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeGrid() {
        gridSpaces = new Dictionary<Vector2Int, GridSpace>();
        CreateSpace(Vector2Int.zero, GridSpace.SpaceOccupation.Red);
        CreateSpace(Vector2Int.right, GridSpace.SpaceOccupation.Blue);
        ExpandGrid();
    }

    public void ExpandGrid() {
        Vector2Int adjacentPosition;
        HashSet<Vector2Int> spacesToAdd = new HashSet<Vector2Int>();
        foreach (KeyValuePair<Vector2Int, GridSpace> entry in gridSpaces) {
            if (entry.Value.Occupation != GridSpace.SpaceOccupation.Free) {
                foreach (Vector2Int adjacency in GridManager.adjacencies) {
                    adjacentPosition = entry.Key + adjacency;
                    if (!gridSpaces.ContainsKey(adjacentPosition)) {
                        spacesToAdd.Add(adjacentPosition);
                    }
                }
            }
        }
        foreach (Vector2Int position in spacesToAdd) {
            CreateSpace(position, GridSpace.SpaceOccupation.Free);
        }
    }

    private void CreateSpace(Vector2Int position, GridSpace.SpaceOccupation occupation) {
        GameObject gridSpaceObject;
        GridSpace gridSpaceScript;
        gridSpaceObject = Instantiate(hexPrefab, this.transform);
        gridSpaceObject.name = string.Format("Hex Space {0},{1}", position.x, position.y);
        gridSpaceObject.transform.localPosition = columnOffsetVector * position.x + rowOffsetVector * position.y;
        gridSpaceScript = gridSpaceObject.GetComponent<GridSpace>();
        gridSpaceScript.Position = position;
        gridSpaceScript.Occupation = occupation;
        gridSpaces.Add(position, gridSpaceScript);
    }

    public void ShrinkGrid() {
        Vector2Int adjacentPosition;
        GridSpace adjacentSpace;
        bool removeSpace;
        List<Vector2Int> keysToRemove = new List<Vector2Int>();
        foreach (KeyValuePair<Vector2Int, GridSpace> entry in gridSpaces) {
            removeSpace = true;
            if (entry.Value.Occupation == GridSpace.SpaceOccupation.Free) {
                foreach (Vector2Int adjacency in GridManager.adjacencies) {
                    adjacentPosition = entry.Key + adjacency;
                    adjacentSpace = gridSpaces.ContainsKey(adjacentPosition) ? gridSpaces[adjacentPosition] : null;
                    if (adjacentSpace != null && adjacentSpace.Occupation != GridSpace.SpaceOccupation.Free) {
                        removeSpace = false;
                        break;
                    }
                }
                if (removeSpace) {
                    keysToRemove.Add(entry.Key);
                }
            }
        }
        foreach (Vector2Int position in keysToRemove) {
            Destroy(gridSpaces[position]);
            gridSpaces.Remove(position);
        }
    }

    public Material GetTokenMaterial(GridSpace.SpaceOccupation player) {
        foreach (TokenColor tokenColor in tokenColors) {
            if (tokenColor.player == player) {
                return tokenColor.material;
            }
        }
        return null;
    }

    public static GridManager GetGridManager() {
        //Watch out for issues when changing scenes, variable needs to be reset somehow
        if (GridManager.gridManager == null) {
            GridManager.gridManager = GameObject.FindObjectOfType<GridManager>();
        }
        return GridManager.gridManager;
    }
}

[Serializable]
public struct TokenColor {
    public GridSpace.SpaceOccupation player;
    public Material material;
}