using UnityEngine;

public class FishMischief : MischiefAction
{
    [SerializeField] private FishJump _fishJump;

    protected override void PerformAction()
    {
        Debug.Log("Fish Mischief started!");
        _fishJump.Jump();
    }

    protected override void ResetAction()
    {
        Debug.Log("Fish Mischief reset!");
        // Implement the logic for resetting the fish mischief action here.
    }

    private void FailFish()
    {
        Debug.Log("The player did not save the FISH in time!");
        FailAction();
    }

    public void SaveFish()
    {
        // The player can only save an active cup.
        if (CurrentState != MischiefState.InProgress)
            return;

        Debug.Log("The player saved the FISH!");
        WinAction();
    }
}
