using UnityEngine;

public class KeepWorldRotation : MonoBehaviour
{
    private Quaternion _startingRotation;

    private void Awake()
    {
        _startingRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = _startingRotation;
    }
}
