using System;
using UnityEngine;
using System.Collections;

public enum MischiefState
{
    Idle,
    InProgress
}

public enum MischiefResult
{
    Success,
    Failure
}

public abstract class MischiefAction : MonoBehaviour
{
    [Header("Completion")]
    [SerializeField] private float _completionDelay = 2f;
    public MischiefState CurrentState { get; private set; } = MischiefState.Idle;
    public MischiefResult? LastResult { get; private set; }
    //public bool IsActive => CurrentState == MischiefState.InProgress;

    public event Action<MischiefAction> OnStarted;
    public event Action<MischiefAction> OnEnded;

    private bool _isCompleting;

    protected abstract void PerformAction();

    protected abstract void ResetAction();

    public void StartAction()
    {
        if(CurrentState != MischiefState.Idle)
            return;

        CurrentState = MischiefState.InProgress;
        LastResult = null;
        _isCompleting = false;

        PerformAction();
        OnStarted?.Invoke(this); //Notify subscribers that the action has started
    }

    public void WinAction()
    {
        CompleteAction(MischiefResult.Success);
    }

    public void FailAction()
    {
        CompleteAction(MischiefResult.Failure);
    }

    private void CompleteAction(MischiefResult result)
    {
        if (CurrentState != MischiefState.InProgress || _isCompleting)
            return;

        _isCompleting = true;
        StartCoroutine(CompleteActionAfterDelay(result));
    }

    private IEnumerator CompleteActionAfterDelay(MischiefResult result)
    {
        yield return new WaitForSeconds(_completionDelay);

        LastResult = result;
        CurrentState = MischiefState.Idle;

        ResetAction();

        _isCompleting = false;

        // Notify subscribers after the delay and reset.
        OnEnded?.Invoke(this);
    }
}
