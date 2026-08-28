using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ObjectMover))]
public class Lure : MonoBehaviour
{
    public enum CatSide
    {
        Left,
        Right
    }

    [Header("Cat")]
    [SerializeField] private CatSide _catSide;

    [Header("Treat")]
    [SerializeField] private Transform _treat;

    [Header("Lure Movement")]
    [SerializeField] private float _moveBackSpeed = 5f;

    private ObjectMover _objectMover;
    private Vector3 _originalPosition;
    private float _originalXDirection;

    private bool _lureIsActive;
    private bool _isMovingBack;

    private void Awake()
    {
        _objectMover = GetComponent<ObjectMover>();
        _originalPosition = transform.localPosition;

        _originalXDirection = transform.localScale.x < 0f ? -1f : 1f;
    }

    private void Update()
    {
        if (!_lureIsActive || _treat == null)
        {
            return;
        }

        bool treatIsOnLureSide = IsTreatOnLureSide();
        SetSpriteFlipped(treatIsOnLureSide);

        if (treatIsOnLureSide)
        {
            MoveBackTowardOriginalPosition();
        }
        else
        {
            ResumeNormalMovement();
        }
    }

    public void StartLure()
    {
        _lureIsActive = true;
    }

    public void StopLure()
    {
        _lureIsActive = false;
        SetSpriteFlipped(false);
        ResumeNormalMovement();
    }

    private void SetSpriteFlipped(bool shouldFlip)
    {
        Vector3 scale = transform.localScale;

        float direction = shouldFlip
            ? -_originalXDirection
            : _originalXDirection;

        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    private bool IsTreatOnLureSide()
    {
        if (_catSide == CatSide.Right)
        {
            // The right cat is lured when the treat is to its right.
            return _treat.position.x > transform.position.x;
        }

        // The left cat is lured when the treat is to its left.
        return _treat.position.x < transform.position.x;
    }

    private void MoveBackTowardOriginalPosition()
    {
        if (!_isMovingBack)
        {
            _objectMover.StopMovement();
            _isMovingBack = true;
        }

        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            _originalPosition,
            _moveBackSpeed * Time.deltaTime);
    }

    private void ResumeNormalMovement()
    {
        if (!_isMovingBack)
        {
            return;
        }

        _isMovingBack = false;
        _objectMover.MoveObjectToTargetPosition();
    }
}
