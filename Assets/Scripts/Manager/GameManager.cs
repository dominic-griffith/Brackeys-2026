using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum GameDifficulty
{
    Easy,
    Medium,
    Hard
}

public class GameManager : MonoBehaviour
{

    [Header("Game Settings")]
    [SerializeField] private GameDifficulty _difficulty = GameDifficulty.Medium;

    public static GameManager Instance { get; private set; }
    public GameDifficulty Difficulty => _difficulty;

    public void Awake()
    {
        //Singleton Design Pattern
        if (Instance == null)
        {

            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public void SetDifficulty(GameDifficulty difficulty)
    {
        _difficulty = difficulty;

        Debug.Log($"Difficulty selected: {_difficulty}");
    }

    public void SetEasyDifficulty()
    {
        SetDifficulty(GameDifficulty.Easy);
    }

    public void SetMediumDifficulty()
    {
        SetDifficulty(GameDifficulty.Medium);
    }

    public void SetHardDifficulty()
    {
        SetDifficulty(GameDifficulty.Hard);
    }
}
