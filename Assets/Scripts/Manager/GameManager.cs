using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{

    [Header("Game Settings")]
    [SerializeField] private int _startingLives = 3;

    public static GameManager Instance { get; private set; }
    public int Lives { get; private set; }

    // Events (Subscribe to these events to get notified when lives change or when the game is over)
    public event Action<int> OnLivesChanged;
    public event Action OnGameOver;

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

    public void LoseLife()
    {
        if (Lives <= 0)
            return;

        Lives--;
        OnLivesChanged?.Invoke(Lives);

        if (Lives == 0)
            OnGameOver?.Invoke();
    }

    public void ResetGame()
    {
        Lives = _startingLives;
        OnLivesChanged?.Invoke(Lives);
    }
}
