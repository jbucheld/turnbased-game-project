using System;
using UnityEngine;
using UnityEngine.InputSystem;


// #if ENABLE_INPUT_SYSTEM
// [RequireComponent(typeof(PlayerInput))]
// #endif
// [RequireComponent(typeof(TacticalViewController))]
public class Unit : MonoBehaviour
{
    private PlayerInput _player;
    private TacticalViewController _controller;
    [SerializeField] private Animator _animator;

    private float stoppingDistance = 0.05f;
    
    private Vector3 targetPosition;
    [SerializeField] private float unitMoveSpeed = 4f;
    [SerializeField] private float unitRotationSpeed = 12f;
    

    private void Start()
    {
        // _controller = GetComponent<TacticalViewController>();
        // _player = GetComponentInParent<PlayerInput>();
    }

    private void Update()
    {
       Move();
    }

    private void Move()
    {
        // gets position from MouseRaycast
        SetTargetPosition();
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
    }

    private void SetTargetPosition()
    {
        if (InputManager.Instance.order)  targetPosition = MouseRaycast.GetPosition();
    }
}

