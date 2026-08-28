using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    private CinemachineCamera _newCamera;
    public PlayerInput playerInput;
    public InputAction moveAction;

    private void Awake()
    {
        _newCamera = GetComponent<CinemachineCamera>();
        moveAction = playerInput.actions.FindActionMap("Player").FindAction("Move");
    }

    public void ToggleToCamera()
    {
        if (_newCamera.Priority == 10)
        {
            // Reverting Camera Back To Original
            _newCamera.Priority = 0; 

            // Re-enabling Movement Input
            moveAction.Enable();
        }
        else
        {
            // Activating New Camera To Be Seen
            _newCamera.Priority = 10;

            // Disabling Movement Input while in Event Camera.
            moveAction.Disable();
        }
    }
}
