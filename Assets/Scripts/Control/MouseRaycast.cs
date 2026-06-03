using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseRaycast : MonoBehaviour
{
    [SerializeField] private LayerMask mouseRaycastLayer;
    [SerializeField] private LayerMask selectableLayers;
    
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
    private static RaycastHit GetMouseRaycastHit(LayerMask mask)
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.look);
        Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, mask);
        return hit;
    }
    
    public static Vector3 GetPosition()
    {
        return GetMouseRaycastHit(Instance.mouseRaycastLayer).point;
    }
    
    private static bool TryGetMouseRaycastHit(LayerMask mask, out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.look);
        return Physics.Raycast(ray, out hit, float.MaxValue, mask);
    }

    public static GameObject GetSelectable()
    {
        return TryGetMouseRaycastHit(Instance.selectableLayers, out var hit)
            ? hit.collider.gameObject
            : null;
    }
}
