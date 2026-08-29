using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ImageFloat : MonoBehaviour
{
    [Header("Float Settings")]
    [SerializeField] private float _moveDistance = 10f;
    [SerializeField] private float _moveSpeed = 2f;

    private RectTransform _rectTransform;
    private Vector2 _startingPosition;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _startingPosition = _rectTransform.anchoredPosition;
    }

    private void Update()
    {
        float yOffset =
            Mathf.Sin(Time.time * _moveSpeed) * _moveDistance;

        _rectTransform.anchoredPosition =
            _startingPosition + new Vector2(0f, yOffset);
    }

    private void OnDisable()
    {
        if (_rectTransform != null)
        {
            _rectTransform.anchoredPosition = _startingPosition;
        }
    }
}

