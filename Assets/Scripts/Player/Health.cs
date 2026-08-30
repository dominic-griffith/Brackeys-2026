using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Difficulty")]
    [SerializeField] private MischiefManager _mischiefManager;

    [Header("Events")]
    [SerializeField] private UnityEvent _onHealthChanged;
    [SerializeField] private UnityEvent _onDeath;

    public int CurrentHealth { get; private set; }
    public int MaximumHealth { get; private set; }

    private void Awake()
    {
        if (_mischiefManager == null)
        {
            Debug.LogError(
                "MischiefManager is not assigned to Health.",
                this
            );

            return;
        }

        MaximumHealth =
            _mischiefManager.StartingLives;

        CurrentHealth = MaximumHealth;
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
        if (amount <= 0 ||
            CurrentHealth >= MaximumHealth)
        {
            return;
        }

        CurrentHealth = Mathf.Min(
            CurrentHealth + amount,
            MaximumHealth
        );

        _onHealthChanged?.Invoke();
    }

    public void ResetHealth()
    {
        CurrentHealth = MaximumHealth;
        _onHealthChanged?.Invoke();
    }
}
