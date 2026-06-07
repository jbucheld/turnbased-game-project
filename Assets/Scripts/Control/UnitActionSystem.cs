using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    
    public event EventHandler OnSelectedUnitChange;
    public event EventHandler OnSelectedActionChange; 
    
    [SerializeField] private Unit selectedUnit;
    private ActionParentClass selectedAction;
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

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        // blocks taking actions unless previous has ended
        if (isBusy) return;
        
        // blocks taking actions if mouse is over UI
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        // handles unit selection
        if (InputManager.Instance.select)
        {
            if (TryHandleUnitSelection()) return;
        }
        
        // handles selected action
        HandleSelectedAction();
        
    }

    private void HandleSelectedAction()
    {
        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseRaycast.GetPosition());
        if (!InputManager.Instance.order) return;
        if (selectedAction.IsPositionValid(mouseGridPosition))
        {
            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);
        }
    }

    private void SetBusy()
    {
        isBusy = true;
    }

    public void ClearBusy()
    {
        isBusy = false;
    }
    
    private bool TryHandleUnitSelection()
    {
        GameObject selectedGameObject = MouseRaycast.GetSelectable();
        if (selectedGameObject)
        {
            selectedGameObject.transform.parent.TryGetComponent(out Unit unit);
            
            // Unit is already selected
            if (unit == selectedUnit) return false;
            
            if (unit) SetSelectedUnit(unit);
            // if (unit.IsEnemy())
            // {
            //     // Clicked on an Enemy
            //     return false;
            // }
            return true;
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
        InputManager.Instance.select = false;
    }
    
    public void SetSelectedAction(ActionParentClass action)
    {
        selectedAction = action;
        OnSelectedActionChange?.Invoke(this, EventArgs.Empty);

    }

    public ActionParentClass GetSelectedAction()
    {
        return selectedAction;
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
