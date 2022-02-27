using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    [Tooltip("World Grid Size - Should match UnityEditor snap settings.")]
    [SerializeField] uint unityGridSize = 10;
    public uint UnityGridSize { get => unityGridSize; }

    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid { get => grid; }

    private void Awake()
    {
        CreateGrid();
    }

    public Node GetNode(Vector2Int theCoordinates)
    {
        if (grid.ContainsKey(theCoordinates))
        {
            return grid[theCoordinates];
        }

        return null;
    }

    public void BlockNode(Vector2Int theCoordinates)
    {
        if (grid.ContainsKey(theCoordinates))
        {
            grid[theCoordinates].isWalkable = false;
        }
    }

    public void ResetNodes()
    {
        foreach(var aEntry in grid)
        {
            aEntry.Value.connectedTo = null;
            aEntry.Value.isExplored = false;
            aEntry.Value.isPath = false;
        }
    }

    public Vector2Int GetCoordinatesFromPosition(Vector3 thePosition)
    {
        var aCoordinate = new Vector2Int();
        aCoordinate.x = Mathf.RoundToInt(thePosition.x / unityGridSize);
        aCoordinate.y = Mathf.RoundToInt(thePosition.z / unityGridSize);

        return aCoordinate;
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int theCoordinates)
    {
        var aCoordinate = new Vector3();
        aCoordinate.x = theCoordinates.x * unityGridSize;
        aCoordinate.z = theCoordinates.y * unityGridSize;

        return aCoordinate;
    }

    private void CreateGrid()
    {
        for (int x = 0; x <= gridSize.x; x++)
        {
            for (int y = 0; y <= gridSize.y; y++)
            {
                Vector2Int aCoordinate = new Vector2Int(x, y);
                grid.Add(aCoordinate, new Node(aCoordinate, true));
            }
        }
    }
}
