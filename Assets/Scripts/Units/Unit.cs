using System;
using UnityEngine;
using UnityEngine.InputSystem;


// #if ENABLE_INPUT_SYSTEM
// [RequireComponent(typeof(PlayerInput))]
// #endif
// [RequireComponent(typeof(TacticalViewController))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDied;

    [SerializeField] private bool isEnemy;
    
    // private float stoppingDistance = 0.05f;
    private GridPosition currentGridPosition;
    [Header("Actions")] 
    private ActionParentClass[] actionsArray;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private HealthSystem healthSystem;
    private int startingActionPoints = 2;
    private int currentActionPoints;
    
    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        healthSystem = GetComponent<HealthSystem>();
        actionsArray = GetComponentsInChildren<ActionParentClass>();
        currentActionPoints = startingActionPoints;
    }

    private void Start()
    {
        currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnUnitChanged;
        healthSystem.OnUnitDeath += HealthSystem_OnUnitDeath;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }
    

    private void Update()
    {
       currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return currentGridPosition;
    }

    public ActionParentClass[] GetActionsArray()
    {
        return actionsArray;
    }

    public int GetCurrentActionPoints()
    {
        return currentActionPoints;
    }
        

    public bool CanSpendActionPointsToTakeAction(ActionParentClass apc)
    {
        // prevents weird bugs with negative currentActionPoints
        if (currentActionPoints < 0)
        {
            currentActionPoints = 0;
            return false;
        }
        // prevents from taking free actions while action points are depleted
        if (currentActionPoints == 0) return false;
        if (apc.GetActionCost() > currentActionPoints) return false;
        return true;
    }

    private void SpendActionPoints(int actionCost)
    {
        currentActionPoints -= actionCost;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);

    }

    public bool TrySpendActionPointsToTakeAction(ActionParentClass apc)
    {
        if (CanSpendActionPointsToTakeAction(apc))
        {
            SpendActionPoints(apc.GetActionCost());
            return true;
        } 
        return false;
    }
    private void TurnSystem_OnUnitChanged(object sender, EventArgs e)
    {
        ResetActionPoints();
    }
    
    public void ResetActionPoints()
    {
        if (IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()
            || !IsEnemy() && TurnSystem.Instance.IsPlayerTurn())
        {
            this.currentActionPoints = startingActionPoints;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void TakeDamage(int damageIncome)
    {
        healthSystem.TakeDamage(damageIncome);
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    private void HealthSystem_OnUnitDeath(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(this.currentGridPosition, this);
        Destroy(this.gameObject);
        OnAnyUnitDied?.Invoke(this, EventArgs.Empty);
    }
}

