using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform gridVisualSystemPrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;
    private GridSystemVisualSingle[,] gridSystemViusalsArray;

    
    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }
    public enum GridVisualType
    {
        GVT_NeutralWhite,
        GVT_AllyBlue,
        GVT_EnemyRed,
        GVT_IDKYellow
    }

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

        UnitActionSystem.Instance.OnSelectedActionChange += UAS_OnSelectedActionChange;
        UnitActionSystem.Instance.OnActionStarted += UAS_OnActionStarted;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LG_OnAnyUnitMovedGridPosition;
        
        HideAllGridPositions();
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

    public void ShowGridPositionList(List<GridPosition> gridPositions, GridVisualType gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositions)
        {
            gridSystemViusalsArray[gridPosition.x, gridPosition.z].
                Show(GetGVTMaterial(gridVisualType));
        }
    }

    private void UpdateGridVisual(Unit unit)
    {
        Instance.HideAllGridPositions();
        GridVisualType gridVisualType;
        
        ActionParentClass selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        switch (selectedAction)
        { 
            default: 
                gridVisualType = GridVisualType.GVT_NeutralWhite;
                break;
            case MoveAction moveAction:
                gridVisualType = GridVisualType.GVT_AllyBlue;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.GVT_EnemyRed;
                break;
        }
        
        Instance.ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }

    // EVENT LISTENERS
    
    private void UAS_OnSelectedActionChange(object sender, EventArgs e)
    {
        UpdateGridVisual(UnitActionSystem.Instance.GetSelectedUnit());
    }

    private void LG_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual(UnitActionSystem.Instance.GetSelectedUnit());
    }
    
    private void UAS_OnActionStarted(object sender, EventArgs e)
    {
        HideAllGridPositions();
    }

    private Material GetGVTMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gvtMaterial in gridVisualTypeMaterialList)
        {
            if (gvtMaterial.gridVisualType == gridVisualType) return gvtMaterial.material;
        }
        Debug.LogError("Could not find GVT_Material for given GVT !");
        return null;
    }
}
