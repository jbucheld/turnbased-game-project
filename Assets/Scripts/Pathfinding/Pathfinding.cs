using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public  static Pathfinding Instance { get; private set; } 
    
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    
    private int width;
    private int length;
    private int cellSize;
    private GridSystem<PathNode> gridSystem;
    [SerializeField] private Transform gridDebugObjectPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one instance of Pathfinding!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        gridSystem = new GridSystem<PathNode>
        (10, 10, 2f,  
            (GridSystem<PathNode> gridSystem, GridPosition gridPosition) => new PathNode(gridPosition)); 
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);

        openList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetLength(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);
                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }
        
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);
            if (currentNode == endNode)
            {
                // reached final node
                return CalculatePath(endNode);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();
                    
                    if (!openList.Contains(neighbourNode)) openList.Add(neighbourNode);
                }
            }
        }
        
        // no path found
        return null;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }
        pathNodeList.Reverse();
        
        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }
        return gridPositionList;
    }

    public int CalculateDistance(GridPosition a, GridPosition b)
    {
        GridPosition gridPositionDistance = a - b;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remainingDistance = Mathf.Abs(xDistance - zDistance);
        
        return (MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance)) + (MOVE_STRAIGHT_COST * remainingDistance);
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];
        foreach (PathNode pathNode in pathNodeList)
        {
            if (pathNode.GetFCost() < lowestFCostPathNode.GetFCost()) lowestFCostPathNode = pathNode;
        }
        return lowestFCostPathNode;
    }

    private PathNode GetPathNodeFromGridCoordinates(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        GridPosition gridPosition = currentNode.GetGridPosition();
        
        
        for (int x = gridPosition.x - 1; x <= gridPosition.x + 1; x++)
        {
            //checks if X is outside LevelGrid
            if (x < 0 || x >= LevelGrid.Instance.GetWidth()) continue;
            
            for (int z = gridPosition.z - 1; z <= gridPosition.z + 1; z++)
            {
                //checks if Z is outside LevelGrid
                if (z < 0 || z >= LevelGrid.Instance.GetLength()) continue;
                
                // skips given gridPosition
                if (x == gridPosition.x && z == gridPosition.z) continue;
                
                neighbourList.Add(gridSystem.GetGridObject(new GridPosition(x, z)));
            }
        }
        return neighbourList;
    }
    
}
