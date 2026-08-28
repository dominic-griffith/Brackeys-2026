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

    [Header("Individual Cats")]
    [SerializeField] private GameObject[] _mischiefCats;
    [SerializeField] private GameObject[] _savedCats;

    [Header("Starting State")]
    [SerializeField] private State _startingState = State.Idle;

    private State _currentState;
    public State CurrentState => _currentState;

    private void Awake()
    {
        SetState(_startingState);

        foreach (GameObject savedCat in _savedCats)
        {
            savedCat.SetActive(false);
        }
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
        ResetIndividualCats();
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

    public void SetIndividualSaved(int catIndex)
    {
        if (catIndex < 0 ||
            catIndex >= _mischiefCats.Length ||
            catIndex >= _savedCats.Length)
        {
            return;
        }

        // Both parents must remain active while cats have different states.
        _mischiefObject.SetActive(true);
        _savedObject.SetActive(true);

        _mischiefCats[catIndex].SetActive(false);
        _savedCats[catIndex].SetActive(true);
    }
    private void ResetIndividualCats()
    {
        foreach (GameObject mischiefCat in _mischiefCats)
        {
            if (mischiefCat != null)
            {
                mischiefCat.SetActive(true);
            }
        }

        foreach (GameObject savedCat in _savedCats)
        {
            if (savedCat != null)
            {
                savedCat.SetActive(false);
            }
        }
    }


}
