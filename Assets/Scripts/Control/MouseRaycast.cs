using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRaycast : MonoBehaviour
{
    [SerializeField] private LayerMask mouseRaycastLayer;
    
    private static MouseRaycast Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Debug.LogError("There is already a MouseRaycast object in the scene.");
            Destroy(this);
        }
    }

    void Update()
    {
        transform.position = GetPosition();
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.look);
        Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, Instance.mouseRaycastLayer); 
        Debug.Log(hit.point);
        return hit.point;
    }
}
