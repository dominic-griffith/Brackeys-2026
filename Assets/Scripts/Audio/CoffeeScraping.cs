using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CoffeeScraping : MonoBehaviour
{
    [SerializeField] private float _movementThreshold = 0.1f;

    [SerializeField] private string _tableTag = "TableTop";
    [SerializeField] private string _catArmTag = "CatArm";

    private Rigidbody2D _rb;
    private bool _isOnTable = false;
    private bool _isPlayingSound = false;
    private bool _isTouchingCatArm = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        bool isMoving = _rb.linearVelocity.magnitude > _movementThreshold;

        if (_isOnTable && (isMoving || _isTouchingCatArm))
        {
            if (!_isPlayingSound)
            {
                // TODO: Tell Audio Manager to Play or RESUME Scrape sound
                AudioManager.GetInstance().Play("CoffeeScrape");
                //Debug.Log("CoffeeScrape is Playing!");
                _isPlayingSound = true;
            }
        }
        else
        {
            if (_isPlayingSound)
            {
                // TODO: Tell Audio Manager to Stop or PAUSE Scrape sound
                AudioManager.GetInstance().Stop("CoffeeScrape");
                //Debug.Log("CoffeeScrape is STOPPING!");
                _isPlayingSound = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(_tableTag))
        {
            _isOnTable = true;
        }
        else if (collision.gameObject.CompareTag(_catArmTag))
        {
            _isTouchingCatArm = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(_tableTag))
        {
            _isOnTable = false;
        }
        else if (collision.gameObject.CompareTag(_catArmTag))
        {
            _isTouchingCatArm = false;
        }
    }
}
