using UnityEngine;

public class TolietMischief : MischiefAction
{
    [SerializeField] private ObjectResetter _cat;
    [SerializeField] private CircularCountdown _countdown;
    protected override void PerformAction()
    {
        Debug.Log("TOLIET Mischief started!");
        _countdown.StartCountdown();
    }

    protected override void ResetAction()
    {
        Debug.Log("TOLIET Mischief reset!");
        _cat.ResetObjectPosition();
        _countdown.ResetCountdown();
    }

    public void FailToliet()
    {
        Debug.Log("The player did ot stop the cat from DROWNING!");
        FailAction();
    }

    public void SaveToliet()
    {
        // The player can only save an active cup.
        if (CurrentState != MischiefState.InProgress)
            return;

        Debug.Log("The player saved the cat from DROWNING!");
        WinAction();
    }
}
