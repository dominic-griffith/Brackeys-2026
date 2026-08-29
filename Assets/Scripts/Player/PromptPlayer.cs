using UnityEngine;
using UnityEngine.Events;

public class PromptPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _promptUI;
    [SerializeField] private UnityEvent _onInteract;

    private void Awake()
    {
        // Sets the _promptUI text to be disabled on initialization.
        _promptUI.SetActive(false);
    }

    public void ShowPrompt(bool show)
    {
        if (_promptUI != null)
        {
            // Shows Prompt button when player is in-range of trigger collider
            _promptUI.SetActive(show);
        }
    }

    public void Interact()
    {
        _onInteract?.Invoke();
    }
}
 

