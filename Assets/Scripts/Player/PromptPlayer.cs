using UnityEngine;
using UnityEngine.Events;

public class PromptPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _promptUI;
    [SerializeField] private UnityEvent _onInteract;

    public void ShowPrompt(bool show)
    {
        if (_promptUI != null)
        {
            _promptUI.SetActive(show);
        }
    }

    public void Interact()
    {
        _onInteract?.Invoke();
    }
}
 

