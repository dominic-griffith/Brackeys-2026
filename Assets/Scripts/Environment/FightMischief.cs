using System.Collections;
using UnityEngine;

public class FightMischief : MischiefAction
{
    [SerializeField] private StateSpriteController _stateSpriteController;
    [SerializeField] private Lure _lureOne;
    [SerializeField] private Lure _lureTwo;
    [SerializeField] private ObjectMover _catOne;
    [SerializeField] private ObjectMover _catTwo;

    [Header("Audio Configurations")]
    [SerializeField] private string[] _catOneHissNames;
    [SerializeField] private string[] _catTwoHissNames;
    [SerializeField] private float _minDelayBetweenHisses = 0.1f;
    [SerializeField] private float _maxDelayBetweenHisses = 0.8f;

    private Coroutine _catOneRoutine;
    private Coroutine _catTwoRoutine;

    protected override void PerformAction()
    {
        Debug.Log("FIGHT Mischief started!");
        _stateSpriteController.SetMischief();
        _lureOne.ResetLure();
        _lureTwo.ResetLure();
        _catOne.MoveObjectToTargetPosition();
        _catTwo.MoveObjectToTargetPosition();

        // Independent audio clips for both cats
        _catOneRoutine = StartCoroutine(HissLoop(_catOneHissNames, -1f));
        _catTwoRoutine = StartCoroutine(HissLoop(_catTwoHissNames, 1f));
    }

    protected override void ResetAction()
    {
        Debug.Log("FIGHT Mischief reset!");
        _catOne.ResetObjectPosition();
        _catTwo.ResetObjectPosition();
        _stateSpriteController.SetIdle();

        StopAllHissing();
    }

    public void FailFight()
    {
        Debug.Log("The player did not stop the FIGHT in time!");
        StopAllHissing();
        FailAction();
        AudioManager.GetInstance().Play("CatFightFail");
    }

    public void SaveFight()
    {
        // The player can only save an active cup.
        if (CurrentState != MischiefState.InProgress)
            return;

        Debug.Log("The player broke up the FIGHT!");
        StopAllHissing();
        AudioManager.GetInstance().Play("Completion");
        WinAction();
    }

    private IEnumerator HissLoop(string[] soundNames, float pan)
    {
        // Loop runs continuously while mischief is active
        while (CurrentState == MischiefState.InProgress)
        {
            if (soundNames.Length == 0) yield break;

            // Pick random clip and play it
            string randomHiss = soundNames[Random.Range(0, soundNames.Length)];

            AudioManager.GetInstance().SetPan(randomHiss, pan);
            AudioManager.GetInstance().Play(randomHiss);

            float clipLength = AudioManager.GetInstance().GetClipLength(randomHiss);

            // Calculate wait time
            float waitTime = clipLength + Random.Range(_minDelayBetweenHisses, _maxDelayBetweenHisses);

            // Pause this coroutine until the wait time completes
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void StopAllHissing()
    {
        // terminate loops
        if (_catOneRoutine != null) StopCoroutine(_catOneRoutine);
        if (_catTwoRoutine != null) StopCoroutine(_catTwoRoutine);

        // Stop active audio
        foreach (string hissName in _catOneHissNames)
        {
            AudioManager.GetInstance().Stop(hissName);
        }

        foreach (string hissName in _catTwoHissNames)
        {
            AudioManager.GetInstance().Stop(hissName);
        }
    }

}
