using System;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public event EventHandler OnEnemyActionStart;
    public static EnemyAI Instance;
    
    private float timer = 3f;
    private float shortCooldown = 1f;
    private State state;
    private Unit selectedUnit;


    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of EnemyAI!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
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
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    } else
                    {
                        // no more enemies have available actions to take
                        TurnSystem.Instance.NextTurn();
                        state = State.WaitingForEnemyTurn;
                    }
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

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TryTakeSingleEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }
        return false;
    }
    
    private bool TryTakeSingleEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        //sends message of a new enemy unit taking action
        selectedUnit = enemyUnit;
        OnEnemyActionStart?.Invoke(this, EventArgs.Empty);

        EnemyAIAction bestEnemyAiAction = null;
        ActionParentClass bestAction = null;

        if (enemyUnit.GetCurrentActionPoints() == 0) return false;
        foreach (ActionParentClass action in enemyUnit.GetActionsArray())
        {
            if (!enemyUnit.CanSpendActionPointsToTakeAction(action))
            {
                // enemy cannot afford this action
                continue;
            }

            if (bestEnemyAiAction == null)
            {
                bestEnemyAiAction = action.GetBestEnemyAIAction();
                bestAction = action;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = action.GetBestEnemyAIAction();
                if (testEnemyAIAction != null
                    && testEnemyAIAction.actionValue > bestEnemyAiAction.actionValue)
                {
                    bestEnemyAiAction = action.GetBestEnemyAIAction();
                    bestAction = action;
                }
            }
            action.GetBestEnemyAIAction();

        }

        if (bestEnemyAiAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestAction))
        {
            bestAction.TakeAction(bestEnemyAiAction.gridPosition , onEnemyAIActionComplete);
            return true;
        } 
        
        return false;
    }

    public Unit GetAISelectedUnit()
    {
        return selectedUnit;
    }
}
