using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private Camera _mainCamera;
    private Draggable _currentInteractable;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (_currentInteractable == null)
            return;

        if (Pointer.current == null)
            return;

        // Move the selected object to the pointer position.
        _currentInteractable.Drag(GetPointerWorldPosition());
    }

    private Vector2 GetPointerWorldPosition()
    {
        Vector2 screenPosition = Pointer.current.position.ReadValue();

        return _mainCamera.ScreenToWorldPoint(screenPosition);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        // The interaction button was pressed.
        if (context.performed)
        {
            BeginInteraction();
        }

        // The interaction button was released.
        if (context.canceled)
        {
            EndInteraction();
        }
    }

    private void BeginInteraction()
    {
        if (_mainCamera == null || Pointer.current == null)
            return;

        Vector2 pointerPosition = GetPointerWorldPosition();

        Collider2D clickedCollider =
            Physics2D.OverlapPoint(pointerPosition);

        // Stop if nothing was clicked.
        if (clickedCollider == null)
            return;

        // Check the clicked object and its parents for Interactable2D.
        Draggable interactable = clickedCollider.GetComponentInParent<Draggable>();

        // Stop if the clicked object is not interactable.
        if (interactable == null)
            return;

        // Store the selected object.
        _currentInteractable = interactable;

        // Start dragging it.
        _currentInteractable.BeginDrag(pointerPosition);
    }

    private void EndInteraction()
    {
        // Do nothing if no object is being dragged.
        if (_currentInteractable == null)
            return;

        // Tell the object that the button was released.
        _currentInteractable.EndDrag();

        // Clear the current selection.
        _currentInteractable = null;
    }
}
