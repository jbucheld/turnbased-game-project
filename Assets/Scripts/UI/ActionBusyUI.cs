using System;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    [SerializeField] private Transform busyImageBar;

    private void Start()
    {
        busyImageBar.gameObject.SetActive(false);
        UnitActionSystem.Instance.OnBusyChange += UnitActionSystem_OnBusyChange;
        // UnitActionSystem.Instance.OnClearBusy += UnitActionSystem_OnClearBusy;
        // UnitActionSystem.Instance.OnSetBusy += UnitActionSystem_OnSetBusy;
    }

    private void UnitActionSystem_OnBusyChange(object sender, bool isBusy)
    {
        busyImageBar.gameObject.SetActive(isBusy);
    }
    
    private void UnitActionSystem_OnSetBusy(object sender, EventArgs e)
    {
        busyImageBar.gameObject.SetActive(true);
    }

    private void UnitActionSystem_OnClearBusy(object sender, EventArgs e)
    {
        busyImageBar.gameObject.SetActive(false);
    }
}
