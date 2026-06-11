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

    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();
        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

        foreach (GridPosition gridPosition in validActionGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }
        
        if (enemyAIActionList.Count > 0)
        {
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            // Debug.Log($" APC wyliczyło najlepszą akcję :: {enemyAIActionList[0]}");
            return enemyAIActionList[0];
        }
        else
        {
            //no possible actions
            return null;
        }
    }

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
}
