using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Draggable : MonoBehaviour
{
    [SerializeField] private bool _canDrag = true;

    // Use these to:
    // Add SFX & VFX for when the player starts dragging this object.
    // Add Check that item in correct location on drag end
    [SerializeField] private UnityEvent _onDragStarted;
    [SerializeField] private UnityEvent _onDragEnded;

    private Vector2 _pointerOffset;

    public bool IsBeingDragged { get; private set; }

    public void BeginDrag(Vector2 pointerPosition)
    {
        if (!_canDrag)
            return;

        // Remember the distance between the pointer and object center.
        _pointerOffset = (Vector2)transform.position - pointerPosition;

        IsBeingDragged = true;
        _onDragStarted?.Invoke();
    }

    public void Drag(Vector2 pointerPosition)
    {
        if (!_canDrag)
            return;

        Vector3 newPosition = new Vector3(pointerPosition.x + _pointerOffset.x, pointerPosition.y + _pointerOffset.y, transform.position.z);

        transform.position = newPosition;
    }

    public void EndDrag()
    {
        if (!_canDrag)
            return;

        IsBeingDragged = false;
        _onDragEnded?.Invoke();
    }

    public void SetCanDrag(bool canDrag)
    {
        _canDrag = canDrag;
    }

}
