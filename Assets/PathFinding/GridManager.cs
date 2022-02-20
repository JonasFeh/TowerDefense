using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    private void Awake()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        for (int x = 0; x <= gridSize.x; x++)
        {
            for (int y = 0; y <= gridSize.y; y++)
            {
                Vector2Int aCoordinate = new Vector2Int(x, y);
                grid.Add(aCoordinate, new Node(aCoordinate, true));
                Debug.Log(grid[aCoordinate].coordinates + " = " + grid[aCoordinate].isWalkable);
            }
        }
    }
}
