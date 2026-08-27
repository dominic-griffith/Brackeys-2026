using UnityEngine;

public class StateSpriteController : MonoBehaviour
{
    public enum State
    {
        Idle,
        Failed,
        Saved,
        Mischief
    }

    [Header("State Objects")]
    [SerializeField] private GameObject _idleObject;
    [SerializeField] private GameObject _failedObject;
    [SerializeField] private GameObject _savedObject;
    [SerializeField] private GameObject _mischiefObject;

    [Header("Starting State")]
    [SerializeField] private State _startingState = State.Idle;

    private State _currentState;
    public State CurrentState => _currentState;

    private void Awake()
    {
        SetState(_startingState);
    }

    public void SetState(State newState)
    {
        _currentState = newState;

        _idleObject.SetActive(newState == State.Idle);
        _failedObject.SetActive(newState == State.Failed);
        _savedObject.SetActive(newState == State.Saved);
        _mischiefObject.SetActive(newState == State.Mischief);
    }

    // These methods can be selected directly from UnityEvents.

    public void SetIdle()
    {
        SetState(State.Idle);
    }

    public void SetFailed()
    {
        SetState(State.Failed);
    }

    public void SetSaved()
    {
        SetState(State.Saved);
    }

    public void SetMischief()
    {
        SetState(State.Mischief);
    }

}
