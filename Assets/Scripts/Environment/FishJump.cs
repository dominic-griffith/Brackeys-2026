using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FishJump : MonoBehaviour
{
    [Header("Fish")]
    [SerializeField] private List<GameObject> _fishObjects = new();

    [Header("Jump Path")]
    [SerializeField] private Transform _apexPoint;
    [SerializeField] private Transform _landingPoint;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpDuration = 1.5f;

    [Header("Events")]
    [SerializeField] private UnityEvent _onFishLanded;

    private readonly Dictionary<GameObject, Vector3> _originalPositions = new();

    private Coroutine _jumpCoroutine;
    private GameObject _activeFish;

    private void Awake()
    {
        SaveOriginalFishPositions();

        //SetAllFishColliders(false);
        SetAllFishPhysics(false);
    }

    private void SaveOriginalFishPositions()
    {
        _originalPositions.Clear();

        foreach (GameObject fish in _fishObjects)
        {
            if (fish != null)
            {
                _originalPositions[fish] = fish.transform.position;
            }
        }
    }

    public void Jump()
    {
        if (_jumpCoroutine != null || _fishObjects.Count == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, _fishObjects.Count);
        _activeFish = _fishObjects[randomIndex];

        if (_activeFish != null)
        {
            // Disable physics while controlling the fish's movement.
            //SetFishColliders(_activeFish, false);
            SetFishPhysics(_activeFish, false);

            _jumpCoroutine = StartCoroutine(JumpCoroutine());
        }
    }

    public void ResetFishLocations()
    {
        if (_jumpCoroutine != null)
        {
            StopCoroutine(_jumpCoroutine);
            _jumpCoroutine = null;
        }

        // Disable physics before repositioning the fish.
        //SetAllFishColliders(false);
        SetAllFishPhysics(false);

        foreach (KeyValuePair<GameObject, Vector3> fish in _originalPositions)
        {
            if (fish.Key != null)
            {
                fish.Key.transform.position = fish.Value;
            }
        }

        _activeFish = null;
    }

    private void SetAllFishColliders(bool enabled)
    {
        foreach (GameObject fish in _fishObjects)
        {
            if (fish != null)
            {
                SetFishColliders(fish, enabled);
            }
        }
    }

    private void SetFishColliders(GameObject fish, bool enabled)
    {
        Collider2D[] colliders =
            fish.GetComponentsInChildren<Collider2D>(true);

        foreach (Collider2D fishCollider in colliders)
        {
            fishCollider.enabled = enabled;
        }
    }

    private void SetAllFishPhysics(bool enabled)
    {
        foreach (GameObject fish in _fishObjects)
        {
            if (fish != null)
            {
                SetFishPhysics(fish, enabled);
            }
        }
    }

    private void SetFishPhysics(GameObject fish, bool enabled)
    {
        Rigidbody2D[] rigidbodies =
            fish.GetComponentsInChildren<Rigidbody2D>(true);

        foreach (Rigidbody2D fishRigidbody in rigidbodies)
        {
            if (!enabled)
            {
                fishRigidbody.linearVelocity = Vector2.zero;
                fishRigidbody.angularVelocity = 0f;
            }

            fishRigidbody.simulated = enabled;
        }
    }

    private IEnumerator JumpCoroutine()
    {
        GameObject jumpingFish = _activeFish;

        Vector3 startPosition = jumpingFish.transform.position;
        Vector3 apexPosition = _apexPoint.position;
        Vector3 landingPosition = _landingPoint.position;

        // Keep the fish on its original Z plane.
        apexPosition.z = startPosition.z;
        landingPosition.z = startPosition.z;

        // Create a curve that passes directly through the apex.
        Vector3 controlPoint =
            (2f * apexPosition) -
            (0.5f * startPosition) -
            (0.5f * landingPosition);

        float elapsedTime = 0f;

        while (elapsedTime < _jumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _jumpDuration);

            Vector3 startToControl =
                Vector3.Lerp(startPosition, controlPoint, t);

            Vector3 controlToLanding =
                Vector3.Lerp(controlPoint, landingPosition, t);

            Vector3 newPosition =
                Vector3.Lerp(startToControl, controlToLanding, t);

            newPosition.z = startPosition.z;
            jumpingFish.transform.position = newPosition;

            yield return null;
        }

        jumpingFish.transform.position = landingPosition;

        // Enable collisions and physics once the fish lands.
        SetFishPhysics(jumpingFish, true);
        //SetFishColliders(jumpingFish, true);

        CircularCountdown countdown = jumpingFish.GetComponentInChildren<CircularCountdown>(true);

        if (countdown != null)
        {
            countdown.StartCountdown();
        }
        else
        {
            Debug.LogWarning($"{jumpingFish.name} does not have a CircularCountdown in its children.", jumpingFish);
        }

        _activeFish = null;
        _jumpCoroutine = null;

        _onFishLanded.Invoke();
    }
}
