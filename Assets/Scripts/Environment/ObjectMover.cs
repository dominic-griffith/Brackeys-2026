using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Events;

// Allows attached game object to be moved & reset
[RequireComponent(typeof(ObjectResetter))]
public class ObjectMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private float _timeToMove = 30f;

    [Header("Events")]
    [SerializeField] private UnityEvent _onTargetPositionReached;

    private ObjectResetter _objectResetter;
    private Coroutine _moveCoroutine;

    private Vector3 _originalPosition;
    private float _movementSpeed;

    private void Awake()
    {
        _objectResetter = GetComponent<ObjectResetter>();
        _originalPosition = transform.localPosition;

        CalculateMovementSpeed();
    }

    private void CalculateMovementSpeed()
    {
        float totalDistance = Vector3.Distance(
            _originalPosition,
            _targetPosition);

        if (_timeToMove > 0f)
        {
            _movementSpeed = totalDistance / _timeToMove;
        }
        else
        {
            _movementSpeed = 0f;
        }
    }

    public void MoveObjectToTargetPosition()
    {
        StopMovement();

        if (_timeToMove <= 0f)
        {
            ReachTargetImmediately();
            return;
        }

        _moveCoroutine = StartCoroutine(MoveObjectCoroutine());
    }

    public void StopMovement()
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }
    }

    public void ResetObjectPosition()
    {
        StopMovement();
        _objectResetter.ResetObjectPosition();
    }

    private IEnumerator MoveObjectCoroutine()
    {
        while (Vector3.Distance(
                   transform.localPosition,
                   _targetPosition) > 0.001f)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                _targetPosition,
                _movementSpeed * Time.deltaTime);

            yield return null;
        }

        transform.localPosition = _targetPosition;
        _moveCoroutine = null;

        _onTargetPositionReached.Invoke();
    }

    private void ReachTargetImmediately()
    {
        transform.localPosition = _targetPosition;
        _moveCoroutine = null;

        _onTargetPositionReached.Invoke();
    }
}