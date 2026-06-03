using UnityEngine;

public class GridSystem
{
    private int width;
    private int length;
    private float cellSize;
    
    private GridObject[,] gridObjectArray;
    
    public GridSystem( int width, int length, float cellSize)
    {
        this.width = width;
        this.length = length;
        this.cellSize = cellSize;
        
        this.gridObjectArray = new GridObject[width, length];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = new GridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 mapPosition)
    {
        return  new GridPosition(
            Mathf.RoundToInt(mapPosition.x / cellSize),
            Mathf.RoundToInt( mapPosition.z / cellSize)
            );
    }

    public void CreateDebugObjects(Transform prefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                
                Transform debugTransform = GameObject.Instantiate(prefab, this.GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }
}
