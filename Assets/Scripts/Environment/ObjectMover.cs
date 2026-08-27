using System.Collections;
using UnityEngine;
using System;

// Allows attached game object to be moved & reset
[RequireComponent(typeof(ObjectResetter))]
public class ObjectMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private float _timeToMove = 30f;

    //public event Action OnMovementCompleted;
    private ObjectResetter _objectResetter;
    //private Vector3 _originalPosition;
    private Coroutine _moveCoroutine;

    private void Awake()
    {
        //_originalPosition = transform.localPosition;
        _objectResetter = GetComponent<ObjectResetter>();
    }

    public void ResetObjectPosition()
    {
        StopMovement();
        //transform.localPosition = _originalPosition;
        _objectResetter.ResetObjectPosition();
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
        StopMovement();
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
        _moveCoroutine = null;
        //OnMovementCompleted?.Invoke();
    }
}