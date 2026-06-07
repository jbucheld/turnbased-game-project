using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionParentClass : MonoBehaviour
{
    protected Unit parentUnit;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        parentUnit = GetComponent<Unit>();
    }

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);
    
    public virtual bool IsPositionValid(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition) ;
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();
    
    public abstract string GetActionName();
}
