using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    private CinemachineCamera _newCamera;
    private void Awake()
    {
        _newCamera = GetComponent<CinemachineCamera>();
    }

    public void ToggleToCamera()
    {
        if (_newCamera.Priority == 10)
        {
            _newCamera.Priority = 0;
        }
        else
        {
            _newCamera.Priority = 10;
        }
    }
}
