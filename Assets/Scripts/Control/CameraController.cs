using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject cinemachineMainCamera;
    [SerializeField] private float moveSpeed = 8f;
    
    [SerializeField] private int previousRotateInput;
    private CinemachineOrbitalFollow cameraRotator;
    private float rotationOffset = 45f;
    private float targetRotation;
    private int inputRotateCamera;
    private bool isRotating;

    [SerializeField] private float rotationSpeed = 360f;

    private void Awake()
    {
        
    }

    private void Start()
    {
        inputRotateCamera = (int) InputManager.Instance.rotateCamera;
        cameraRotator = cinemachineMainCamera.GetComponent<CinemachineOrbitalFollow>();
    }

    private void Update()
    {
        // checks if camera is to move this frame
        MoveCamera();
        
        // rounds input to int for easier calculation
        inputRotateCamera = (int) InputManager.Instance.rotateCamera;
        
        // checks if rotation is to start: checking current input, previous input and animation state
        if (!isRotating && inputRotateCamera != 0 && previousRotateInput == 0) StartRotation();

        previousRotateInput = inputRotateCamera;
        RotateCamera();
    }

    private void MoveCamera()
    {
        // gets input and current camera rotation (camera and this tracker object are different objects)
        Vector2 cameraMoveOrder = InputManager.Instance.cameraMove;
        float cameraRotatorY = cameraRotator.HorizontalAxis.Value;
        
        //generates quaternion for WSAD correct movement
        Quaternion rotation = Quaternion.Euler(0, cameraRotatorY, 0);
        Vector3 forward = rotation * Vector3.forward;
        Vector3 right = rotation * Vector3.right;
        
        // eliminates y offsetting issues
        forward.y = 0;
        right.y = 0;
        
        //moves tracker based on WSAD movement
        Vector3 inputMoveDirection = right * cameraMoveOrder.x + forward * cameraMoveOrder.y;
        transform.position += inputMoveDirection * (moveSpeed * Time.deltaTime);
    }
    
    private void StartRotation()
    {
        float currentAngle = cameraRotator.HorizontalAxis.Value;
        
        // calculates camera angle with 45° offset
        targetRotation =
            Mathf.Round((currentAngle - rotationOffset) / 90f) * 90f
            + rotationOffset;

        targetRotation += inputRotateCamera * 90f;
        
        // marks animation
        isRotating = true;
    }

    private void RotateCamera()
    {
        // exits if animation isn't happening 
        if (!isRotating) return;
        
        // calculates camera rotation path
        cameraRotator.HorizontalAxis.Value = Mathf.MoveTowards(cameraRotator.HorizontalAxis.Value, targetRotation, rotationSpeed * Time.deltaTime);
        
        //calculates animation stop
        if (Mathf.Abs(Mathf.DeltaAngle(cameraRotator.HorizontalAxis.Value, targetRotation)) < .1f)
        {
            //prevents big numbers in this field
            targetRotation = Mathf.Repeat(targetRotation, 360f);

            // straightens current camera angle
            cameraRotator.HorizontalAxis.Value = targetRotation;
            
            // ends animation
            isRotating = false;
        }
    }
}
