using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _playerSpeed = 5f;

    [Header("Direction Sprites")]
    [SerializeField] private Sprite _frontSprite;
    [SerializeField] private Sprite _backSprite;
    [SerializeField] private Sprite _leftSprite;
    [SerializeField] private Sprite _rightSprite;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _moveInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // Start facing forward/down.
        _spriteRenderer.sprite = _frontSprite;
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity =
            _moveInput * _playerSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        UpdateDirectionSprite();
    }

    private void UpdateDirectionSprite()
    {
        // Keep the current sprite when the player stops.
        if (_moveInput.sqrMagnitude < 0.01f)
        {
            return;
        }

        // Use whichever movement axis is strongest.
        if (Mathf.Abs(_moveInput.x) > Mathf.Abs(_moveInput.y))
        {
            if (_moveInput.x > 0f)
            {
                _spriteRenderer.sprite = _rightSprite;
            }
            else
            {
                _spriteRenderer.sprite = _leftSprite;
            }
        }
        else
        {
            if (_moveInput.y > 0f)
            {
                // Moving up.
                _spriteRenderer.sprite = _backSprite;
            }
            else
            {
                // Moving down.
                _spriteRenderer.sprite = _frontSprite;
            }
        }
    }
}
