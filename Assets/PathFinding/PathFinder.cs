using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinate;
    public Vector2Int StartCoordinate { get => startCoordinate; }

    [SerializeField] Vector2Int destinationCoordinate;
    public Vector2Int DestinationCoordinate { get => destinationCoordinate; }

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    GridManager gridManager;
    IDictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();


    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null)
        {
            grid = gridManager.Grid;
            startNode = grid[StartCoordinate];
            destinationNode = grid[DestinationCoordinate];
        }
    }

    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinate);
    }

    public List<Node> GetNewPath(Vector2Int theCoordinates)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(theCoordinates);
        return BuildPath();
    }



    private void ExploreNeighbors()
    {
        var aNeighborList = new List<Node>();
        foreach (var aDirection in directions)
        {
            var aNeighborCoordinate = currentSearchNode.coordinates + aDirection;
            if (grid.ContainsKey(aNeighborCoordinate))
            {
                aNeighborList.Add(grid[aNeighborCoordinate]);

            }
        }
        foreach (var neighbor in aNeighborList)
        {
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BreadthFirstSearch(Vector2Int theCoordinates)
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        frontier.Enqueue(grid[theCoordinates]);
        reached.Add(theCoordinates, grid[theCoordinates]);

        while (frontier.Count > 0 && isRunning == true)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if (currentSearchNode.coordinates == DestinationCoordinate)
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();

        return path;
    }

    public bool WillBlockPath(Vector2Int theCoordinates)
    {
        if (grid.ContainsKey(theCoordinates))
        {
            var aPreviousState = grid[theCoordinates].isWalkable;
            grid[theCoordinates].isWalkable = false;
            List<Node> aNewPath = GetNewPath();
            grid[theCoordinates].isWalkable = aPreviousState;

            if (aNewPath.Count <= 1)
            {
                return true;
            }
        }

        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
