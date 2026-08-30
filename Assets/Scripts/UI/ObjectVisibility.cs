using UnityEngine;

public class ObjectVisibility : MonoBehaviour
{
    [SerializeField] private GameObject _targetObject;

    private void Awake()
    {
        _targetObject.SetActive(false);
    }

    public void Show()
    {
        if (_targetObject != null)
        {
            _targetObject.SetActive(true);
        }
    }

    public void Hide()
    {
        if (_targetObject != null)
        {
            _targetObject.SetActive(false);
        }
    }

    public void Toggle()
    {
        if (_targetObject != null)
        {
            _targetObject.SetActive(
                !_targetObject.activeSelf
            );
        }
    }
}
