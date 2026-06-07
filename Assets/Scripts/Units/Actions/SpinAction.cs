using System;
using System.Collections.Generic;
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

    private void Spin(Action onSpinComplete)
    {
        this.onActionComplete = onSpinComplete;
        totalSpinAmount = 0;
        isActive = true;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = parentUnit.GetGridPosition();
        return new List<GridPosition> { unitGridPosition };
    }

    public override string GetActionName()
    {
        return "Spin!";
    }
    
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Spin(onActionComplete);
    }
}
