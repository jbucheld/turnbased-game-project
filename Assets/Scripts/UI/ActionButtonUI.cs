using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;
    
    private ActionParentClass thisAction;

    public void SetBaseAction(ActionParentClass actionParentClass)
    {
        this.thisAction = actionParentClass;
        textMeshProUGUI.text = actionParentClass.GetActionName().ToUpper();
        
        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(actionParentClass);
        });
    }

    public void UpdateSelectedVisual()
    {
        ActionParentClass selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedAction == thisAction);
    }
}
