using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera overShoulderShootingCamera;
    private Unit unitToLeanOverShoulder;

    private void Awake()
    {
        // overShoulderShootingCamera.gameObject.SetActive(true);
        overShoulderShootingCamera.Priority = 1;
    }

    private void Start()
    {
        HideShootingCamera();
        ActionParentClass.OnAnyActionStarted += APC_OnAnyActionStarted;
        ActionParentClass.OnAnyActionCompleted += APC_OnAnyActionCompleted;
    }

    private void ShowShootingCamera()
    {
        overShoulderShootingCamera.Priority = 20;
        // overShoulderShootingCamera.gameObject.SetActive(true);

    }

    private void HideShootingCamera()
    {
        overShoulderShootingCamera.Priority = 1;
        // overShoulderShootingCamera.gameObject.SetActive(false);
    }

    private void APC_OnAnyActionStarted(object sender, EventArgs e)
    {
        ActionParentClass startedAction = (ActionParentClass)sender;
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            unitToLeanOverShoulder = UnitActionSystem.Instance.GetSelectedUnit();
        }
        else
        {
            unitToLeanOverShoulder = EnemyAI.Instance.GetAISelectedUnit();
        }
        
        
        if (startedAction.IsActionShootingType())
        {
            overShoulderShootingCamera.Target.TrackingTarget = unitToLeanOverShoulder.gameObject.transform;
            ShowShootingCamera();
        }
    }

    private void APC_OnAnyActionCompleted(object sender, EventArgs e)
    {
        HideShootingCamera();
    }
    
    
}
