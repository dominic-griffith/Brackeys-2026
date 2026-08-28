using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField]
    private Vector3 _rotationSpeed = new Vector3(0f, 0f, 90f);

    private bool _isRotating = true;

    private void Update()
    {
        if (!_isRotating)
        {
            return;
        }

        transform.Rotate(_rotationSpeed * Time.deltaTime);
    }

    public void StartRotating()
    {
        _isRotating = true;
    }

    public void StopRotating()
    {
        _isRotating = false;
    }

    public void ResetRotation()
    {
        _isRotating = false;
        transform.localRotation = Quaternion.identity;
    }
}
