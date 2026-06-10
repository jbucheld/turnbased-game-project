using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionParentClass : MonoBehaviour
{
    protected Unit parentUnit;
    protected bool isActive;
    protected bool isActionShootingType;

    protected Action onActionComplete;

    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

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

    public virtual int GetActionCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionEnd(Action onActionComplete)
    {
        isActive = false;
        onActionComplete();
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public bool IsActionShootingType()
    {
        return isActionShootingType;
    }
}
