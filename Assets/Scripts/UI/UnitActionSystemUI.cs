using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem.Composites;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private List<ActionButtonUI> actionButtons;

    private void Awake()
    {
        actionButtons = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        UnitActionSystem_OnSelectedUnitChange(null, null);
        UpdateSelectedVisual();
    }

    private void CreateUnitActionButtons()
    {
        ClearUnitActionButtons();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        foreach (ActionParentClass action in selectedUnit.GetActionsArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtons.Add(actionButtonUI);
            actionButtonUI.SetBaseAction(action);
        }
    }

    private void ClearUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }
        actionButtons.Clear();
    }

    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }

    private void UnitActionSystem_OnSelectedActionChange(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }
    
    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI button in actionButtons)
        {
            button.UpdateSelectedVisual();
        }
    }
}
