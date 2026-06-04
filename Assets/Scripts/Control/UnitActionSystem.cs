using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    
    public event EventHandler OnSelectedUnitChange;
    
    [SerializeField] private Unit selectedUnit;
    private bool isBusy;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one instance of UnitActionSystem!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    private void Update()
    {
        if (isBusy) return ;
        if (InputManager.Instance.order) SetTargetPosition();
        if (InputManager.Instance.select) SelectUnit();
        if (InputManager.Instance.testKey) SpinUnit();
    }

    private void SetBusy()
    {
        isBusy = true;
    }

    public void ClearBusy()
    {
        isBusy = false;
    }
    
    private void SelectUnit()
    {
        GameObject selectedGameObject = MouseRaycast.GetSelectable();
        if (selectedGameObject)
        {
            selectedGameObject.transform.parent.TryGetComponent(out Unit unit);
            if (unit) SetSelectedUnit(unit);
        }
    }
    
    private void SetTargetPosition()
    {
        SetBusy();
        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseRaycast.GetPosition());
        if (selectedUnit.GetMoveAction().IsPositionValidForMovement(mouseGridPosition))
        {
            selectedUnit.GetMoveAction().Move(mouseGridPosition);
        }
        InputManager.Instance.order = false;
        ClearBusy();
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
        InputManager.Instance.select = false;
    }

    private void SpinUnit()
    {
        SetBusy();
        selectedUnit.GetSpinAction().Spin(ClearBusy);
        InputManager.Instance.testKey = false;
    }
    
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
