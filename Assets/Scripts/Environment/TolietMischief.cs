using UnityEngine;

public class TolietMischief : MischiefAction
{
    [SerializeField] private StateSpriteController _stateSpriteController;
    [SerializeField] private ObjectResetter _cat;
    [SerializeField] private CircularCountdown _countdown;
    protected override void PerformAction()
    {
        Debug.Log("TOLIET Mischief started!");
        _stateSpriteController.SetMischief();
        _cat.ResetObjectPosition();
        _countdown.ResetCountdown();
        _countdown.StartCountdown();
    }

    protected override void ResetAction()
    {
        Debug.Log("TOLIET Mischief reset!");
        _stateSpriteController.SetIdle();
    }

    public void FailToliet()
    {
        Debug.Log("The player did not stop the cat from DROWNING!");
        AudioManager.GetInstance().Play("FlushingToilet");
        AudioManager.GetInstance().Play("CatToiletPanic");
        _stateSpriteController.SetFailed();
        FailAction();
    }

    public void SaveToliet()
    {
        // The player can only save an active cup.
        if (CurrentState != MischiefState.InProgress)
            return;

        _stateSpriteController.SetSaved();
        Debug.Log("The player saved the cat from DROWNING!");
        AudioManager.GetInstance().Play("Completion");
        WinAction();
    }
}
