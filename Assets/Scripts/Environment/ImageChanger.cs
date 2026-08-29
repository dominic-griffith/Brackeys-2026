using UnityEngine;

public class ImageChanger : MonoBehaviour
{
    [Header("State Images")]
    [SerializeField] private Sprite _stateOneSprite;
    [SerializeField] private Sprite _stateTwoSprite;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ShowStateOne()
    {
        _spriteRenderer.sprite = _stateOneSprite;
    }

    public void ShowStateTwo()
    {
        _spriteRenderer.sprite = _stateTwoSprite;
    }
}
