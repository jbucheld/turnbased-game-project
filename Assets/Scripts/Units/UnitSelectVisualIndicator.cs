using System;
using UnityEngine;

public class UnitSelectVisualIndicator : MonoBehaviour
{
    [SerializeField] private Unit unit;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }
    
    public void ToggleIndicator(bool value)
    {
        _meshRenderer.enabled = value;
    }
    
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        UpdateVisual();
    }
    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit) 
            _meshRenderer.enabled = true;
        else  
            _meshRenderer.enabled = false;
    }
}
