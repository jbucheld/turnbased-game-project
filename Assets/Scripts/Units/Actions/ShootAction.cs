using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : ActionParentClass
{
    private float totalSpinAmount;

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
    
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Shoot(onActionComplete);    
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = parentUnit.GetGridPosition();
        return new List<GridPosition> { unitGridPosition };
    }
    
    private void Shoot(Action onSpinComplete)
    {
        this.onActionComplete = onSpinComplete;
        totalSpinAmount = 0;
        isActive = true;
    }

    public override string GetActionName()
    {
        return "Shoot!";
    }
}
