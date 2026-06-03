using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    
    public event EventHandler OnSelectedUnitChange;
    
    [SerializeField] private Unit selectedUnit;

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
        if (InputManager.Instance.order) SetTargetPosition();
        if (InputManager.Instance.select) SelectUnit();
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
        selectedUnit.Move(MouseRaycast.GetPosition());
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
    }
    
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
