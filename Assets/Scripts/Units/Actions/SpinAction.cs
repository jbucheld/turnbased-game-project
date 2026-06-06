using System;
using UnityEngine;

public class SpinAction : ActionParentClass
{
    private float totalSpinAmount;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (!isActive) return;
        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
       
        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360f)
        {
            isActive = false;
            onActionComplete();
        }
    }

    public void Spin(Action onSpinComplete)
    {
        this.onActionComplete = onSpinComplete;
        totalSpinAmount = 0;
        isActive = true;
    }
    
    public override string GetActionName()
    {
        return "Spin!";
    }
}
