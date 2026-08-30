using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int _maximumHealth = 3;
    [SerializeField] private int _startingHealth = 3;

    [Header("Events")]
    [SerializeField] private UnityEvent _onHealthChanged;
    [SerializeField] private UnityEvent _onDeath;

    public int CurrentHealth { get; private set; }
    public int MaximumHealth => _maximumHealth;

    private void Awake()
    {
        CurrentHealth = Mathf.Clamp(
            _startingHealth,
            0,
            _maximumHealth
        );
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || CurrentHealth <= 0)
        {
            return;
        }

        CurrentHealth = Mathf.Max(
            CurrentHealth - damage,
            0
        );

        _onHealthChanged?.Invoke();

        if (CurrentHealth == 0)
        {
            _onDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || CurrentHealth >= _maximumHealth)
        {
            return;
        }

        CurrentHealth = Mathf.Min(
            CurrentHealth + amount,
            _maximumHealth
        );

        _onHealthChanged?.Invoke();
    }

    public void ResetHealth()
    {
        CurrentHealth = _maximumHealth;
        _onHealthChanged?.Invoke();
    }
}
