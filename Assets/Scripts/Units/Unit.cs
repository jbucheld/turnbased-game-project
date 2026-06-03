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
    
    private Vector3 targetPosition;
    private GridPosition currentGridPosition;
    
    [SerializeField] private float unitMoveSpeed = 4f;
    [SerializeField] private float unitRotationSpeed = 12f;


    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Start()
    {
        
        currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(currentGridPosition, this);
    }

    private void Update()
    {
       Move(targetPosition);

    }

    public void Move(Vector3 givenOrderPosition)
    {
        targetPosition = givenOrderPosition;
        
        // apply simple move mechanism
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            _animator.SetBool("IsWalking", true);
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            
            transform.position +=  moveDirection * (Time.deltaTime * unitMoveSpeed);
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * unitRotationSpeed);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if (newGridPosition != currentGridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this.currentGridPosition, newGridPosition, this);
        }
        currentGridPosition = newGridPosition;

    }

    

    
}

