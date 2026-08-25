using System.Collections;
using UnityEngine;

// Allows attached game object to be moved & reset
public class ObjectMover : MonoBehaviour
{
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private float _timeToMove = 30f;

    private Vector3 _originalPosition;
    private Coroutine _moveCoroutine;

    private void Awake()
    {
        _originalPosition = transform.localPosition;
    }

    private void Start()
    {
        MoveObjectToTargetPosition();
    }

    public void ResetObjectPosition()
    {
        StopMovement();
        transform.position = _originalPosition;
    }

    private void StopMovement()
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }
    }

    public void MoveObjectToTargetPosition()
    {
        _moveCoroutine = StartCoroutine(MoveObjectCoroutine());
    }

    private IEnumerator MoveObjectCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.localPosition;
        while (elapsedTime < _timeToMove)
        {
            elapsedTime += Time.deltaTime;
            float percentage = Mathf.Clamp01(elapsedTime / _timeToMove);
            transform.localPosition = Vector3.Lerp(startingPosition, _targetPosition, percentage);
            yield return null;
        }
        transform.localPosition = _targetPosition;
    }
}