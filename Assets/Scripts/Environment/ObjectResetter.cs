using UnityEngine;

// Stores and restores the attached object's original local position.
public class ObjectResetter : MonoBehaviour
{
    private Vector3 _originalLocalPosition;
    private Quaternion _originalLocalRotation;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _originalLocalPosition = transform.localPosition;
        _originalLocalRotation = transform.localRotation;
        TryGetComponent(out _rigidbody);
    }

    public void ResetObjectPosition()
    {
        if (_rigidbody != null)
        {
            // Stop all existing falling and spinning movement.
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.angularVelocity = 0f;
        }

        // Restore its original position and rotation.
        transform.localPosition = _originalLocalPosition;
        transform.localRotation = _originalLocalRotation;

        // Update Unity's physics positions immediately.
        Physics2D.SyncTransforms();
    }

    // Changes the position that the object will reset to.
    // This is useful if its reset position changes during the game.
    public void SaveCurrentPosition()
    {
        _originalLocalPosition = transform.localPosition;
        _originalLocalRotation = transform.localRotation;
    }
}