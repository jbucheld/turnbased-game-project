using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Button button;

    public void SetBaseAction(ActionParentClass actionParentClass)
    {
        textMeshProUGUI.text = actionParentClass.GetActionName().ToUpper();
    }
    void Update()
    {
        
    }
}
