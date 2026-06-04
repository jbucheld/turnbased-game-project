using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class InputManager : MonoBehaviour
{
    [Header("Player inputs tracker")] 
    public Vector2 look;
    public Vector2 cameraMove;
    public bool testKey;
    public float rotateCamera;
    public bool select;
    public bool order;

    public static InputManager Instance { get;  private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There is already a CustomPlayerInputs object in the scene.");
            Destroy(this);
        }
    }
    
#if ENABLE_INPUT_SYSTEM

    public void OnLook(InputValue value)
    {
        LookInput(value.Get<Vector2>());
    }

    public void OnSelect(InputValue value)
    {
        SelectInput(value.isPressed);
    }

    public void OnOrder(InputValue value)
    {
        OrderInput(value.isPressed);
    }

    public void OnTestKey(InputValue value)
    {
        TestKeyInput(value.isPressed);
    }

    public void OnCameraMove(InputValue value)
    {
        CameraMoveInput(value.Get<Vector2>());
    }

    public void OnRotateCamera(InputValue value)
    {
        RotateCameraInput(value.Get<float>());
    }
    

#endif
    
    private void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    private void CameraMoveInput(Vector2 newCameraMoveDirection)
    {
        cameraMove = newCameraMoveDirection;
    }

    private void RotateCameraInput(float newRotateValue)
    {
        rotateCamera = newRotateValue;
    }

    private void SelectInput(bool newSelectState)
    {
        select = newSelectState;
    }

    private void OrderInput(bool newOrderState)
    {
        order = newOrderState;
    }
    
    private void TestKeyInput(bool newTestKeyState)
    {
        testKey = newTestKeyState;
    }
}
