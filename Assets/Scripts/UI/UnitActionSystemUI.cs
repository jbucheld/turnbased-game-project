using System;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem_OnSelectedUnitChange(null, null);
    }

    private void CreateUnitActionButtons()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        foreach (ActionParentClass action in selectedUnit.GetActionsArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(action);
            
        }
    }

    private void ClearUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs e)
    {
        ClearUnitActionButtons();
        CreateUnitActionButtons();
    }
}
