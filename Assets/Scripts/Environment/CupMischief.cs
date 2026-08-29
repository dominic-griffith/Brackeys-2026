using UnityEngine;

public class CupMischief : MischiefAction
{
    [System.Serializable]
    private class CupVariant
    {
        public GameObject variantParent;
        public StateSpriteController stateSpriteController;
        public ObjectMover objectMover;
        public ObjectResetter objectResetter;
    }

    [Header("Cup Variants")]
    [SerializeField] private CupVariant[] _cupVariants;

    private CupVariant _activeCup;

    private void Awake()
    {
        ChooseRandomCup();
    }

    protected override void PerformAction()
    {
        if (_activeCup == null)
        {
            ChooseRandomCup();
        }

        if (_activeCup == null)
        {
            return;
        }

        Debug.Log(
            $"Cup Mischief started using " +
            $"{_activeCup.stateSpriteController.name}!"
        );

        _activeCup.stateSpriteController.SetMischief();
        _activeCup.objectMover.MoveObjectToTargetPosition();
    }

    protected override void ResetAction()
    {
        Debug.Log("Cup Mischief reset!");

        if (_activeCup != null)
        {
            if (_activeCup.objectMover != null)
            {
                _activeCup.objectMover.ResetObjectPosition();
            }

            if (_activeCup.stateSpriteController != null)
            {
                _activeCup.stateSpriteController.SetIdle();
            }
        }

        // Randomly choose the setup for the next action.
        ChooseRandomCup();
    }

    private void ChooseRandomCup()
    {
        if (_cupVariants == null || _cupVariants.Length == 0)
        {
            Debug.LogWarning("No cup variants are assigned.", this);
            _activeCup = null;
            return;
        }

        // Disable every variant parent first.
        foreach (CupVariant cup in _cupVariants)
        {
            if (cup?.variantParent != null)
            {
                cup.variantParent.SetActive(false);

                Debug.Log(
                    $"Disabled variant parent: {cup.variantParent.name}",
                    cup.variantParent
                );
            }
        }

        int randomIndex = Random.Range(0, _cupVariants.Length);
        _activeCup = _cupVariants[randomIndex];

        if (_activeCup == null ||
            _activeCup.variantParent == null ||
            _activeCup.stateSpriteController == null ||
            _activeCup.objectMover == null ||
            _activeCup.objectResetter == null)
        {
            Debug.LogWarning(
                $"Cup variant {randomIndex} is not fully assigned.",
                this
            );

            _activeCup = null;
            return;
        }

        // Enable only the randomly selected variant.
        _activeCup.variantParent.SetActive(true);
        _activeCup.stateSpriteController.SetIdle();

        Debug.Log(
            $"Enabled variant parent: {_activeCup.variantParent.name}",
            _activeCup.variantParent
        );
    }

    public void FailCup()
    {
        if (_activeCup == null)
        {
            return;
        }

        Debug.Log("The player did not save the CUP!");

        // Reset this variant's object before showing its failed state.
        _activeCup.objectResetter.ResetObjectPosition();
        _activeCup.stateSpriteController.SetFailed();

        FailAction();
    }

    public void SaveCup()
    {
        if (CurrentState != MischiefState.InProgress ||
            _activeCup == null)
        {
            return;
        }

        Debug.Log("The player saved the CUP!");

        _activeCup.objectResetter.ResetObjectPosition();
        _activeCup.stateSpriteController.SetSaved();
        WinAction();
    }

}
