using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private MischiefManager _mischiefManager;

    [Header("UI")]
    [SerializeField] private TMP_Text _difficultyText;
    [SerializeField] private TMP_Text _scoreText;

    private void Start()
    {
        if (_mischiefManager == null)
        {
            Debug.LogError(
                "MischiefManager is not assigned.",
                this
            );

            return;
        }

        _mischiefManager.OnScoreChanged += UpdateScore;

        UpdateDifficulty();
        UpdateScore(
            _mischiefManager.SuccessfulMischiefs
        );
    }

    private void UpdateDifficulty()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError(
                "GameManager was not found.",
                this
            );

            return;
        }

        _difficultyText.text =
            GameManager.Instance.Difficulty
                .ToString()
                .ToUpper();
    }

    private void UpdateScore(int score)
    {
        _scoreText.text = $"SCORE: {score}";
    }

    private void OnDestroy()
    {
        if (_mischiefManager != null)
        {
            _mischiefManager.OnScoreChanged -= UpdateScore;
        }
    }
}
