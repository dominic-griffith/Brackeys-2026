using UnityEngine;

public class FishMischief : MischiefAction
{
    [SerializeField] private FishJump _fishJump;
    [SerializeField] private StateSpriteController _stateSpriteController;

    protected override void PerformAction()
    {
        Debug.Log("Fish Mischief started!");
        _fishJump.Jump();
        _stateSpriteController.SetMischief();
    }

    protected override void ResetAction()
    {
        Debug.Log("Fish Mischief reset!");
        _stateSpriteController.SetIdle();
        // Implement the logic for resetting the fish mischief action here.
    }

    private void FailFish()
    {
        Debug.Log("The player did not save the FISH in time!");
        _stateSpriteController.SetFailed();
        FailAction();

    }

    public void SaveFish()
    {
        // The player can only save an active cup.
        if (CurrentState != MischiefState.InProgress)
            return;

        Debug.Log("The player saved the FISH!");
        _stateSpriteController.SetSaved();
        WinAction();
    }
}
