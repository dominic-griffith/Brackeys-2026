using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MischiefManager : MonoBehaviour
{
    [SerializeField] private List<MischiefAction> _mischiefActions;

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }


    [Serializable]
    private class DifficultySettings
    {
        public Difficulty difficulty;

        [Min(0f)]
        public float minimumTimeBetweenMischief = 5f;

        [Min(0f)]
        public float maximumTimeBetweenMischief = 10f;

        [Min(1f)]
        public int maximumActiveMischiefs = 1;
    }

    [Header("Difficulty")]
    [SerializeField] private Difficulty _currentDifficulty;
    [SerializeField] private List<DifficultySettings> _difficultySettings;

    private readonly List<MischiefAction> _activeMischiefs = new();
    private Coroutine _mischiefCoroutine;

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

        StartMischiefLoop();
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

    public void SetDifficulty(Difficulty newDifficulty)
    {
        _currentDifficulty = newDifficulty;
        StartMischiefLoop();
    }

    private DifficultySettings GetCurrentSettings()
    {
        return _difficultySettings.Find(
            settings => settings.difficulty == _currentDifficulty
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
        List<MischiefAction> validActions =
            _mischiefActions.FindAll(action => action != null);

        if (validActions.Count == 0)
        {
            Debug.LogWarning("There are no mischief actions available.");
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

        Debug.Log($"{action.name} started. " + $"Active mischiefs: {_activeMischiefs.Count}");
    }

    // Called when any mischief invokes its OnEnded event.
    private void HandleMischiefEnded(MischiefAction action)
    {
        // Remove the finished mischief from the active list.
        _activeMischiefs.Remove(action);

        Debug.Log($"{action.name} ended. " +$"Active mischiefs: {_activeMischiefs.Count}");
    }
}
