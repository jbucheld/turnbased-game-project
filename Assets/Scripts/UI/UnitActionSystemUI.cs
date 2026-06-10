using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Composites;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI remainingActionPointsText;

    private List<ActionButtonUI> actionButtons;

    private void Awake()
    {
        actionButtons = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        
        UnitActionSystem_OnSelectedUnitChange(null, null);
        UpdateActionPoints();
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
    
    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI button in actionButtons)
        {
            button.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        int remainingPoints = UnitActionSystem.Instance.GetSelectedUnit().GetCurrentActionPoints();
        remainingActionPointsText.text = $"Action Points : {remainingPoints}";
    }
    
    
    // EVENT LISTENERS

    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedActionChange(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }
    
    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }
    
    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }
    
    
}
