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

    private float stoppingDistance = 0.05f;
    private GridPosition currentGridPosition;
    [Header("Actions")] 
    private ActionParentClass[] actionsArray;
    private MoveAction moveAction;
    private SpinAction spinAction;
    


    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        actionsArray = GetComponentsInChildren<ActionParentClass>();
    }

    private void Start()
    {
        currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        // GetMoveAction().Move(currentGridPosition);
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

}

