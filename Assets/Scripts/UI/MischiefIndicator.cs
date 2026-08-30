using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RectTransform))]
public class MischiefIndicator : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _playerCamera;
    
    [Tooltip("Distance from the edge of the screen in pixels.")]
    [SerializeField] private float _edgePadding = 50f;

    [SerializeField] private float xDirOffset = 0.0f;
    [SerializeField] private float yDirOffset = 0.0f;

    private Camera _actualRenderCamera;
    private RectTransform _rectTransform;
    private Transform _targetArea;
    private Vector3 _originalScale;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalScale = _rectTransform.localScale;
        gameObject.SetActive(false);

        // Find the Unity Camera that possess the CinemachineBrain.
        CinemachineBrain brain = FindFirstObjectByType<CinemachineBrain>();
        if (brain != null)
        {
            _actualRenderCamera = brain.GetComponent<Camera>();
        }
        else
        {
            Debug.LogError("No CinemachineBrain found in scene.");
        }
    }

    // Event Subscription
    private void OnEnable()
    {
        CameraSwitcher.OnAnyCameraToggled += HandleCameraToggled;
    }

    private void OnDisable()
    {
        CameraSwitcher.OnAnyCameraToggled -= HandleCameraToggled;
    }

    private void HandleCameraToggled(bool isEventCameraActive)
    {
        // Hide UI by scaling to 0
        _rectTransform.localScale = isEventCameraActive ? Vector3.zero : _originalScale;
    }



    public void Activate(Transform targetArea)
    {
        _targetArea = targetArea;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _targetArea = null;
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (_targetArea == null || _actualRenderCamera == null) return;

        // 1. Get viewport position (0,0 is bottom left, 1,1 is top right)
        Vector3 viewportPos = _actualRenderCamera.WorldToViewportPoint(_targetArea.position);

        // 2. Calculate padding as a percentage of the screen
        float paddingX = _edgePadding / Screen.width;
        float paddingY = _edgePadding / Screen.height;

        // 3. Clamp coordinates to camera edges
        viewportPos.x = Mathf.Clamp(viewportPos.x, paddingX, 1f - paddingX);
        viewportPos.y = Mathf.Clamp(viewportPos.y, paddingY, 1f - paddingY);

        // 4. Force the projection depth to match the distance from the camera to the Canvas
        viewportPos.z = Mathf.Abs(_actualRenderCamera.transform.position.z - _rectTransform.position.z);

        // 5. Convert back to World Space
        Vector3 finalWorldPos = _actualRenderCamera.ViewportToWorldPoint(viewportPos);
        finalWorldPos.x = finalWorldPos.x + xDirOffset;
        finalWorldPos.y = finalWorldPos.y + yDirOffset;

        // 6. Lock the Z axis to its original depth so it does not clip behind backgrounds
        finalWorldPos.z = _rectTransform.position.z;

        _rectTransform.position = finalWorldPos;
    }
}
