using System;
using UnityEngine;

public enum MischiefState
{
    Idle,
    InProgress,
}

public enum MischiefResult
{
    Success,
    Failure
}

public abstract class MischiefAction : MonoBehaviour
{
    public MischiefState CurrentState { get; private set; } = MischiefState.Idle;
    public MischiefResult? LastResult { get; private set; }
    //public bool IsActive => CurrentState == MischiefState.InProgress;

    public event Action<MischiefAction> OnStarted;
    public event Action<MischiefAction> OnEnded;

    protected abstract void PerformAction();

    protected abstract void ResetAction();

    public void StartAction()
    {
        if(CurrentState != MischiefState.Idle)
            return;

        CurrentState = MischiefState.InProgress;
        LastResult = null;

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
        if (CurrentState != MischiefState.InProgress)
            return;

        CurrentState = MischiefState.Idle;
        LastResult = result;

        ResetAction();
        OnEnded?.Invoke(this); //Notify subscribers that the action has ended
    }
}
