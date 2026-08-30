using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MischiefManager : MonoBehaviour
{
    [SerializeField] private List<MischiefAction> _mischiefActions;

    [Header("Player Health")]
    [SerializeField] private Health _playerHealth;
    [SerializeField, Min(1)] private int _damagePerFailure = 1;


    [Serializable]
    public struct MischiefLocationData
    {
        public string mischiefName;         // e.g., "CupMischief"
        public Transform interactableArea;  // Where the mischief happens
        public MischiefIndicator indicator; // The specific UI element for this mischief
    }
    [Header("UI Indicators")]
    [SerializeField] private List<MischiefLocationData> _mischiefLocations;


    [Serializable]
    private class DifficultySettings
    {
        public GameDifficulty difficulty;

        [Min(0f)]
        public float minimumTimeBetweenMischief = 5f;

        [Min(0f)]
        public float maximumTimeBetweenMischief = 10f;

        [Min(1f)]
        public int maximumActiveMischiefs = 1;

        [Min(0.1f)]
        public float completionTime = 10f;

        [Min(1)]
        public int startingLives = 3;
    }

    [Header("Difficulty")]
    [SerializeField] private List<DifficultySettings> _difficultySettings;

    private GameDifficulty _currentDifficulty = GameDifficulty.Medium;

    private readonly List<MischiefAction> _activeMischiefs = new();
    private Coroutine _mischiefCoroutine;


    public event Action<int> OnScoreChanged;
    public int SuccessfulMischiefs { get; private set; }
    public int FailedMischiefs { get; private set; }

    private void Start()
    {
        // Subscribe once to every mischief's start and end events.
        foreach (MischiefAction action in _mischiefActions)
        {
            if (action == null)
                continue;

            action.OnStarted += HandleMischiefStarted;
            action.OnEnded += HandleMischiefEnded;
        }

        if (GameManager.Instance != null)
        {
            _currentDifficulty =
                GameManager.Instance.Difficulty;
        }
        else
        {
            _currentDifficulty = GameDifficulty.Medium;

            Debug.LogWarning(
                "GameManager was not found. Using Medium difficulty.",
                this
            );
        }

        Debug.Log(
            $"Mischief difficulty: {_currentDifficulty}",
            this
        );

        StartMischiefLoop();
    }

    public float CompletionTime
    {
        get
        {
            DifficultySettings settings = GetCurrentSettings();

            if (settings != null)
            {
                return settings.completionTime;
            }

            Debug.LogWarning(
                $"No difficulty settings found for {_currentDifficulty}. " +
                "Using 10 seconds.",
                this
            );

            return 10f;
        }
    }

    public int StartingLives
    {
        get
        {
            DifficultySettings settings = GetCurrentSettings();

            if (settings != null)
            {
                return settings.startingLives;
            }

            Debug.LogError(
                $"No difficulty settings found for {_currentDifficulty}.",
                this
            );

            return 0;
        }
    }

    public void StartMischiefLoop()
    {
        if (_mischiefCoroutine != null)
            StopCoroutine(_mischiefCoroutine);

        _mischiefCoroutine = StartCoroutine(MischiefLoop());
    }

    public void StopMischiefLoop()
    {
        if (_mischiefCoroutine == null)
            return;

        StopCoroutine(_mischiefCoroutine);
        _mischiefCoroutine = null;
    }

    public void SetDifficulty(GameDifficulty newDifficulty)
    {
        _currentDifficulty = newDifficulty;
        StartMischiefLoop();
    }

    private DifficultySettings GetCurrentSettings()
    {
        if (GameManager.Instance != null)
        {
            _currentDifficulty =
                GameManager.Instance.Difficulty;
        }

        return _difficultySettings.Find(
            settings =>
                settings.difficulty == _currentDifficulty
        );
    }

    private IEnumerator MischiefLoop()
    {
        while (true)
        {
            DifficultySettings settings = GetCurrentSettings();

            if (settings == null)
            {
                Debug.LogError($"No settings exist for {_currentDifficulty} difficulty.");

                _mischiefCoroutine = null;
                yield break;
            }

            float randomWaitTime = UnityEngine.Random.Range(
                settings.minimumTimeBetweenMischief,
                settings.maximumTimeBetweenMischief
            );

            yield return new WaitForSeconds(randomWaitTime);

            // Only start another mischief if there is room for one.
            if (_activeMischiefs.Count <
                settings.maximumActiveMischiefs)
            {
                StartRandomMischief();
            }
        }
    }

    private void StartRandomMischief()
    {
        // Only Actions that are idle
        List<MischiefAction> validActions = _mischiefActions.FindAll(action => action != null && action.CurrentState == MischiefState.Idle);

        if (validActions.Count == 0)
        {
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, validActions.Count);
        validActions[randomIndex].StartAction();
    }

    // Called when any mischief invokes its OnStarted event.
    private void HandleMischiefStarted(MischiefAction action)
    {
        // Add it to the active list if it is not already there.
        if (!_activeMischiefs.Contains(action))
        {
            _activeMischiefs.Add(action);
        }

        // Find the mapped data for this active mischief
        MischiefLocationData data = _mischiefLocations.Find(loc => action.name.Contains(loc.mischiefName));

        if (data.indicator != null && data.interactableArea != null)
        {
            data.indicator.Activate(data.interactableArea);
        }

        Debug.Log($"{action.name} started. " + $"Active mischiefs: {_activeMischiefs.Count}");
    }

    // Called when any mischief invokes its OnEnded event.
    private void HandleMischiefEnded(MischiefAction action)
    {
        _activeMischiefs.Remove(action);

        // Find the mapped data and turn off its indicator
        MischiefLocationData data = _mischiefLocations.Find(loc => action.name.Contains(loc.mischiefName));

        if (data.indicator != null)
        {
            data.indicator.Deactivate();
        }

        // Check the result stored by MischiefAction.
        if (action.LastResult == MischiefResult.Success)
        {
            SuccessfulMischiefs++;

            OnScoreChanged?.Invoke(SuccessfulMischiefs);

            Debug.Log(
                $"{action.name} succeeded! " +
                $"Total successes: {SuccessfulMischiefs}"
            );
        }
        else if (action.LastResult == MischiefResult.Failure)
        {
            FailedMischiefs++;

            // Damage the player for failing the mischief.
            if (_playerHealth != null)
            {
                _playerHealth.TakeDamage(_damagePerFailure);
            }
            else
            {
                Debug.LogWarning(
                    "Player Health is not assigned to MischiefManager.",
                    this
                );
            }

            Debug.Log(
                $"{action.name} failed! " +
                $"Total failures: {FailedMischiefs}"
            );
        }

        Debug.Log(
            $"{action.name} ended. " +
            $"Active mischiefs: {_activeMischiefs.Count}"
        );
    }
}
