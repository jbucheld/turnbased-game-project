using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitLocalUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarUI;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        UpdateActionPointsText();
        UpdateHealthBar();
        if (unit.IsEnemy())
        {
            ColorUtility.TryParseHtmlString("#FF0008", out Color color);
            healthBarUI.color = color;
        }
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }
    
    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetCurrentActionPoints().ToString();
    }
    private void UpdateHealthBar()
    {
        healthBarUI.fillAmount = healthSystem.GetCurrentHealthInFloat();
    }
    
    private  void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    
    
    
}
