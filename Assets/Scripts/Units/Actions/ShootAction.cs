using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : ActionParentClass
{
    private float totalSpinAmount;
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }

    private State state;
    private float stateTimer;
    private float stateCooldown = 0.5f;
    [SerializeField] int shootingRange = 6;
    
    private Unit targetUnit;
    private bool canShootBullet;
    private float unitAimRotationSpeed = 12f;

    private void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Aiming:
                RotateWhileAiming();
                break;
            case State.Shooting:
                if (canShootBullet == true)
                {
                    ProcessShoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
        }

        if (stateTimer <= 0)
        {
            NextState();
        }
    }

    private void RotateWhileAiming()
    {
        Vector3 aimDirection = (targetUnit.GetWorldPosition() - parentUnit.GetWorldPosition()).normalized;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * unitAimRotationSpeed);
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                if (stateTimer <= 0) state = State.Shooting;
                stateTimer = stateCooldown;
                break;
            case State.Shooting:
                if (stateTimer <= 0) state = State.Cooloff;
                stateTimer = stateCooldown;
                break;
            case State.Cooloff:
                if (stateTimer <= 0)
                stateTimer = stateCooldown;
                ActionEnd(onActionComplete);
                break;
        } 
    }


    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        canShootBullet = true;
        ShootProcedure(onActionComplete);    
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition currentGridPosition = parentUnit.GetGridPosition();

        for (int x = -shootingRange; x <= shootingRange; x++)
        {
            for (int z = -shootingRange; z <= shootingRange; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = currentGridPosition + offsetGridPosition;

                // check if position exists in LevelGrid
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > shootingRange) continue;
                
                // check if GridPosition isn't empty
                if (!LevelGrid.Instance.HasAnyUnit(testGridPosition)) continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == parentUnit.IsEnemy()) continue;
                
                validGridPositionList.Add(testGridPosition);
            }
        }
        
        return validGridPositionList;
    }
    
    private void ShootProcedure(Action onShootComplete)
    {
        state = State.Aiming;
        stateTimer = stateCooldown;
    }

    private void ProcessShoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = this.parentUnit
        });
        Debug.Log("Dealt X damage");
    }

    public override string GetActionName()
    {
        return "Shoot!";
    }
}
