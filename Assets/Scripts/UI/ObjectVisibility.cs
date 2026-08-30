using UnityEngine;

public class ObjectVisibility : MonoBehaviour
{
    [SerializeField] private GameObject[] _targetObjects;

    private void Awake()
    {
        Hide();
    }

    public void Show()
    {
        SetObjectsActive(true);
    }

    public void Hide()
    {
        SetObjectsActive(false);
    }

    public void Toggle()
    {
        foreach (GameObject targetObject in _targetObjects)
        {
            if (targetObject == null)
            {
                continue;
            }

            // Use the first valid object's state for the whole group.
            SetObjectsActive(!targetObject.activeSelf);
            return;
        }
    }

    private void SetObjectsActive(bool isActive)
    {
        if (_targetObjects == null)
        {
            return;
        }

        foreach (GameObject targetObject in _targetObjects)
        {
            if (targetObject != null)
            {
                targetObject.SetActive(isActive);
            }
        }
    }
}
