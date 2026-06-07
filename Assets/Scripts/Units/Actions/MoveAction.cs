using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : ActionParentClass
{
    private Vector3 targetPosition;
    private GridPosition currentGridPosition;
    
    [Header("Movement")]
    [SerializeField] private float stoppingDistance = 0.05f;
    [SerializeField] private float unitMoveSpeed = 4f;
    [SerializeField] private float unitRotationSpeed = 12f;
    [SerializeField] private int maxMoveDistance = 4;

    [SerializeField] private Animator _animator;
    private bool isMoving = false;
    
    
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

    public void OrderMove(GridPosition givenOrderPosition, Action onMoveComplete)
    {
        this.onActionComplete = onMoveComplete;
        Debug.Log("OrderMove function called.");
        if (InputManager.Instance.order)
        {
            Debug.Log($"OrderButton pressed : {InputManager.Instance.order}");
            targetPosition = LevelGrid.Instance.GetWorldPosition(givenOrderPosition);
            isActive = true;
            
            InputManager.Instance.order = false;
            Debug.Log($"OrderButton should be unpressed : {InputManager.Instance.order}");
        }
    }
    
    private void Move()
    {
        // targetPosition = LevelGrid.Instance.GetWorldPosition(givenOrderPosition);
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        // apply simple move mechanism
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            _animator.SetBool("IsWalking", true);
            transform.position +=  moveDirection * (Time.deltaTime * unitMoveSpeed);
        }
        else
        {
            isActive = false;
            _animator.SetBool("IsWalking", false);
            onActionComplete();
        }
        
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * unitRotationSpeed);
        
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != currentGridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this.currentGridPosition, newGridPosition, parentUnit);
        }
        currentGridPosition = newGridPosition;
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
        OrderMove(gridPosition, onActionComplete);
    }
}
