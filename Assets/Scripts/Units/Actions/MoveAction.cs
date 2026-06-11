using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : ActionParentClass
{
    private Vector3 targetPosition;
    private GridPosition currentGridPosition;
    public event EventHandler OnUnitStartMoving;
    public event EventHandler OnUnitStopMoving;
    
    [Header("Movement")]
    [SerializeField] private float stoppingDistance = 0.05f;
    [SerializeField] private float unitMoveSpeed = 4f;
    [SerializeField] private float unitRotationSpeed = 12f;
    [SerializeField] private int maxMoveDistance = 4;

    // [SerializeField] private Animator _animator;
    // private bool isMoving = false;
    
    
    protected override void Awake()
    {
        base.Awake();
        targetPosition = this.transform.position;
    }
    

    private void Start()
    {
        currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(currentGridPosition, parentUnit);
    }
    
    private void Update()
    {
        if(isActive) Move();
    }

    public void OrderMove(GridPosition givenOrderPosition)
    {
        targetPosition = LevelGrid.Instance.GetWorldPosition(givenOrderPosition);
        OnUnitStartMoving?.Invoke(this, EventArgs.Empty);
    }
    
    private void Move()
    {
        // targetPosition = LevelGrid.Instance.GetWorldPosition(givenOrderPosition);
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        // apply simple move mechanism
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position +=  moveDirection * (Time.deltaTime * unitMoveSpeed);
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * unitRotationSpeed);
            
        }
        else
        {
            CheckGridPosition();
            ActionEnd(onActionComplete);
            OnUnitStopMoving?.Invoke(this, EventArgs.Empty);
        }
    }

    private void CheckGridPosition()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != currentGridPosition)
        {
            GridPosition oldGridPosition = currentGridPosition;
            currentGridPosition = newGridPosition;
            
            LevelGrid.Instance.UnitMovedGridPosition(oldGridPosition, currentGridPosition, parentUnit);
        }
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition currentGridPosition = parentUnit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = currentGridPosition + offsetGridPosition;

                // check if position exists in LevelGrid
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                // check if unit isn't standing at target position;
                if (parentUnit.GetGridPosition() == testGridPosition) continue;
                // check for other units standing at target position
                if (LevelGrid.Instance.HasAnyUnit(testGridPosition)) continue;                
                
                validGridPositionList.Add(testGridPosition);
            }
        }
        
        return validGridPositionList;
    }
    
    public override string GetActionName()
    {
        return "Move";
    }
    
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        OrderMove(gridPosition);
        ActionStart(onActionComplete);
    }
    
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = parentUnit.GetShootAction().GetTargetCountAtPosition(gridPosition);
        
        return new EnemyAIAction()
        {
            gridPosition = gridPosition,
            actionValue = 10 + (targetCountAtGridPosition * 10),
        };
    }
}
