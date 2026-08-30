using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlungerStretch : MonoBehaviour
{
    [Header("Cat")]
    [SerializeField] private SpriteRenderer _catSprite;
    [SerializeField] private Rigidbody2D _catRigidbody;
    [SerializeField] private Collider2D _catCollider;
    [SerializeField] private Transform _catAttachmentPoint;

    [Header("Stretch")]
    [SerializeField] private float _stretchAmount = 1f;
    [SerializeField] private float _maximumYScale = 5f;


    private Vector3 _originalCatScale;
    private RigidbodyConstraints2D _originalCatConstraints;
    private Transform _originalCatParent;
    private CapsuleCollider2D _plungerCollider;

    private float _attachedY;

    private bool _isStretching;
    private bool _isCarrying;
    private bool _canAttach = true;

    private void Awake()
    {
        _plungerCollider = GetComponent<CapsuleCollider2D>();
        _originalCatScale = _catSprite.transform.localScale;
        _originalCatConstraints = _catRigidbody.constraints;
        _originalCatParent = _catRigidbody.transform.parent;
    }

    private void LateUpdate()
    {
        if (!_isStretching || _isCarrying)
        {
            return;
        }

        float distancePulledUp =
            transform.position.y - _attachedY;

        // Prevent downward movement from shrinking the cat.
        distancePulledUp = Mathf.Max(0f, distancePulledUp);

        float newYScale =
            _originalCatScale.y +
            distancePulledUp * _stretchAmount;

        newYScale = Mathf.Clamp(
            newYScale,
            _originalCatScale.y,
            _maximumYScale);

        Vector3 scale = _originalCatScale;
        scale.y = newYScale;

        _catSprite.transform.localScale = scale;

        if (newYScale >= _maximumYScale)
        {
            AttachCat();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != _catCollider || _isCarrying || !_canAttach)
        {
            return;
        }

        if (_catAttachmentPoint == null)
        {
            Debug.LogWarning("Cat attachment point is not assigned.", this);
            return;
        }

        // Bottom-center of the plunger collider.
        Vector2 plungerBottom = new Vector2(
            _plungerCollider.bounds.center.x,
            _plungerCollider.bounds.min.y
        );

        // Move the plunger so its bottom-center lines up
        // with the attachment point on the cat.
        Vector2 movementNeeded =
            (Vector2)_catAttachmentPoint.position - plungerBottom;

        transform.position += new Vector3(
            movementNeeded.x,
            movementNeeded.y,
            0f
        );

        _attachedY = transform.position.y;
        _isStretching = true;

        Debug.Log("Plunger connected to the cat attachment point.", this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other != _catCollider)
        {
            return;
        }

        _canAttach = true;

        if (!_isCarrying)
        {
            ResetStretch();
        }
    }

    private void AttachCat()
    {
        if (_catAttachmentPoint == null)
        {
            Debug.LogWarning("Cat attachment point is not assigned.", this);
            return;
        }

        _isStretching = false;
        _isCarrying = true;

        // Return the cat to its normal size.
        _catSprite.transform.localScale = _originalCatScale;

        // Unfreeze the cat's X and Y positions.
        _catRigidbody.constraints &=
            ~RigidbodyConstraints2D.FreezePositionX;

        _catRigidbody.constraints &=
            ~RigidbodyConstraints2D.FreezePositionY;

        _catRigidbody.linearVelocity = Vector2.zero;
        _catRigidbody.angularVelocity = 0f;

        // Disable physics while carrying the cat.
        _catRigidbody.simulated = false;

        // Find the bottom-center of the plunger.
        Vector2 plungerBottom = new Vector2(
            _plungerCollider.bounds.center.x,
            _plungerCollider.bounds.min.y
        );

        // Move the cat until its attachment point lines up
        // with the bottom-center of the plunger.
        Vector2 movementNeeded =
            plungerBottom - (Vector2)_catAttachmentPoint.position;

        _catRigidbody.transform.position += new Vector3(
            movementNeeded.x,
            movementNeeded.y,
            0f
        );

        // Parent the cat after aligning it.
        _catRigidbody.transform.SetParent(transform, true);

        Debug.Log("Cat attached using its attachment point.", this);
        AudioManager.GetInstance().Play("PlungerConnection");
    }

    public void DropCat()
    {
        if (!_isCarrying)
        {
            return;
        }

        // Detach while keeping the cat in its current world position.
        _catRigidbody.transform.SetParent(_originalCatParent, true);

        // Re-enable the cat's Rigidbody2D and physics.
        _catRigidbody.simulated = true;
        _catRigidbody.WakeUp();

        _catRigidbody.linearVelocity = Vector2.zero;
        _catRigidbody.angularVelocity = 0f;

        _catSprite.transform.localScale = _originalCatScale;

        _isCarrying = false;
        _isStretching = false;
        _canAttach = false;

        Debug.Log("Cat dropped and Rigidbody2D enabled.");
    }

    private void ResetStretch()
    {
        _isStretching = false;
        _catSprite.transform.localScale = _originalCatScale;
    }

    public void ResetCat()
    {
        _catRigidbody.transform.SetParent(_originalCatParent);

        _catRigidbody.simulated = true;
        _catRigidbody.constraints = _originalCatConstraints;
        _catRigidbody.linearVelocity = Vector2.zero;
        _catRigidbody.angularVelocity = 0f;

        _catSprite.transform.localScale = _originalCatScale;

        _isStretching = false;
        _isCarrying = false;
    }
}
