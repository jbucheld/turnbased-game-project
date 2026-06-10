using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float timer = 3f;
    private float shortCooldown = 1f;
    private State state;


    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        switch (state)
        {
            case (State.WaitingForEnemyTurn) :
                break;
            case (State.TakingTurn) :
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    state = State.Busy;
                    TakeEnemyAIAction(SetStateTakingTurn);
                }
                break;
            case (State.Busy) :
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        timer = shortCooldown;
        state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 3f;
        }
    }

    private void TakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            TakeSingleEnemyAIAction(enemyUnit, onEnemyAIActionComplete);
        }
    }
    
    private void TakeSingleEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();
        
        GridPosition enemyUnitPosition = enemyUnit.GetGridPosition();
        if (!spinAction.IsPositionValid(enemyUnitPosition)) return;
        
        // checks action points availability and spends them
        if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction)) return;
        
        spinAction.TakeAction(enemyUnitPosition, onEnemyAIActionComplete);
    }
}
