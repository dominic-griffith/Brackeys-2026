using UnityEngine;

public class CupMischief : MischiefAction
{
    [SerializeField] private StateSpriteController _stateSpriteController;
    [SerializeField] private ObjectMover _objectMover;

    //private void Start()
    //{
    //    _objectMover.OnMovementCompleted += HandleMovementCompleted;
    //}

    protected override void PerformAction()
    {
        Debug.Log("Cup Mischief started!");
        _stateSpriteController.SetMischief();
        _objectMover.MoveObjectToTargetPosition();
    }

    protected override void ResetAction()
    {
        Debug.Log("Cup Mischief reset!");
        _objectMover.ResetObjectPosition();
        _stateSpriteController.SetIdle();
    }

    //private void HandleMovementCompleted()
    //{
    //    Debug.Log("The player did not save the cup in time!");
    //    EndAction();
    //}

    public void FailCup()
    {
        Debug.Log("The player did not save the CUP in time!");
        _stateSpriteController.SetFailed();
        FailAction();
    }

    public void SaveCup()
    {
        // The player can only save an active cup.
        if (CurrentState != MischiefState.InProgress)
            return;

        Debug.Log("The player saved the CUP!");
        _stateSpriteController.SetSaved();
        WinAction();
    }

}
