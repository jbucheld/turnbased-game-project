using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform gridVisualSystemPrefab;
    private GridSystemVisualSingle[,] gridSystemViusalsArray;

    public static GridSystemVisual Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There is already an instance of GridSystemVisualSingle!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gridSystemViusalsArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(), 
            LevelGrid.Instance.GetLength()
        ];
        
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetLength(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform = 
                    Instantiate(gridVisualSystemPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                gridSystemViusalsArray[x,z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }
        
        HideAllGridPositions();
    }

    private void Update()
    {
        UpdateGridVisual(UnitActionSystem.Instance.GetSelectedUnit());
    }

    public void HideAllGridPositions()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetLength(); z++)
            {
                gridSystemViusalsArray[x,z].Hide();
            }
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositions)
    {
        foreach (GridPosition gridPosition in gridPositions)
        {
            gridSystemViusalsArray[gridPosition.x, gridPosition.z].Show();
        }
    }

    private void UpdateGridVisual(Unit unit)
    {
        Instance.HideAllGridPositions();

        ActionParentClass selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        Instance.ShowGridPositionList(selectedAction.GetValidActionGridPositionList());
    }
}
