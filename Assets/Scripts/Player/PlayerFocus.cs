using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerFocus : MonoBehaviour
{
    private PromptPlayer _currentInteractable;

    public void Focus(InputAction.CallbackContext context)
    {
        // The focus button was pressed.
        if (context.started && _currentInteractable != null)
        {
            _currentInteractable.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PromptPlayer interactable))
        {
            _currentInteractable = interactable;
            _currentInteractable.ShowPrompt(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out PromptPlayer interactable) && interactable == _currentInteractable)
        {
            _currentInteractable.ShowPrompt(false);
            _currentInteractable = null;
        }
    }
}
