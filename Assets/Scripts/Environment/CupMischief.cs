using UnityEngine;

public class CupMischief : MischiefAction
{
    [SerializeField] private ObjectMover _objectMover;

    private void Start()
    {
        _objectMover.OnMovementCompleted += HandleMovementCompleted;
    }

    protected override void PerformAction()
    {
        Debug.Log("Cup Mischief started!");
        _objectMover.MoveObjectToTargetPosition();
    }

    protected override void ResetAction()
    {
        Debug.Log("Cup Mischief reset!");
        _objectMover.ResetObjectPosition();
    }

    private void HandleMovementCompleted()
    {
        Debug.Log("The player did not save the cup in time!");
        EndAction();
    }

    public void SaveCup()
    {
        if (!IsActive)
            return;

        Debug.Log("The player saved the cup!");
        EndAction();
    }

}
