using System;
using UnityEngine;

public abstract class MischiefAction : MonoBehaviour
{
    public bool IsActive { get; private set; }

    public event Action<MischiefAction> OnStarted;
    public event Action<MischiefAction> OnEnded;

    protected abstract void PerformAction();

    protected abstract void ResetAction();

    public void StartAction()
    {
        if (IsActive)
            return;

        IsActive = true;

        PerformAction();
        OnStarted?.Invoke(this); //Notify subscribers that the action has started
    }

    public void EndAction()
    {
        if (!IsActive)
            return;

        ResetAction();

        IsActive = false;
        OnEnded?.Invoke(this); //Notify subscribers that the action has ended
    }
}
