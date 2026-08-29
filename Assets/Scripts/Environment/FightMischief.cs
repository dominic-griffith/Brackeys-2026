using UnityEngine;

public class FightMischief : MischiefAction
{
    [SerializeField] private StateSpriteController _stateSpriteController;
    [SerializeField] private Lure _lureOne;
    [SerializeField] private Lure _lureTwo;
    [SerializeField] private ObjectMover _catOne;
    [SerializeField] private ObjectMover _catTwo;

    protected override void PerformAction()
    {
        Debug.Log("FIGHT Mischief started!");
        _stateSpriteController.SetMischief();
        _lureOne.ResetLure();
        _lureTwo.ResetLure();
        _catOne.MoveObjectToTargetPosition();
        _catTwo.MoveObjectToTargetPosition();
    }

    protected override void ResetAction()
    {
        Debug.Log("FIGHT Mischief reset!");
        _catOne.ResetObjectPosition();
        _catTwo.ResetObjectPosition();
        _stateSpriteController.SetIdle();
    }

    public void FailFight()
    {
        Debug.Log("The player did ot stop the FIGHT in time!");
        FailAction();
    }

    public void SaveFight()
    {
        // The player can only save an active cup.
        if (CurrentState != MischiefState.InProgress)
            return;

        Debug.Log("The player broke up the FIGHT!");
        WinAction();
    }
}
