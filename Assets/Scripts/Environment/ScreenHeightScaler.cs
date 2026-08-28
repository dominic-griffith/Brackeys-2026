using UnityEngine;

public class ScreenHeightScaler : MonoBehaviour
{

    [Header("Scaling")]
    [Range(0f, 1f)]
    [SerializeField] private float _minimumScaleMultiplier = 0.25f;

    [Range(0f, 1f)]
    [SerializeField] private float _shrinkEndHeight = 0.5f;

    private Camera _camera;
    private Vector3 _originalScale;

    private void Awake()
    {
        _originalScale = transform.localScale;

        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_camera == null)
        {
            return;
        }

        Vector3 viewportPosition =
            _camera.WorldToViewportPoint(transform.position);

        // Progress goes from 0 at the bottom to 1 at the middle.
        float progress = Mathf.InverseLerp(
            0f,
            _shrinkEndHeight,
            viewportPosition.y);

        float scaleMultiplier = Mathf.Lerp(
            1f,
            _minimumScaleMultiplier,
            progress);

        transform.localScale =
            _originalScale * scaleMultiplier;
    }
}
