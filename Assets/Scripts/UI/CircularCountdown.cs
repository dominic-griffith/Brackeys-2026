using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class CircularCountdown : MonoBehaviour
{
    [SerializeField] private MischiefManager _mischiefManager;

    [Header("Countdown")]
    [SerializeField] private Image _countdownImage;

    [Header("Events")]
    [SerializeField] private UnityEvent _onCountdownFinished;

    private Coroutine _countdownCoroutine;
    private bool _durationInitialized;
    private float _countdownDuration;

    private void Awake()
    {
        // The circle begins completely full.
        _countdownImage.fillAmount = 1f;
        _countdownImage.enabled = false;
    }

    public void StartCountdown()
    {
        if (!_durationInitialized)
        {
            SetCountdownDuration();
        }

        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
        }
        _countdownImage.fillAmount = 1f;
        _countdownImage.enabled = true;

        _countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }

    private void SetCountdownDuration()
    {
        if (_mischiefManager != null)
        {
            _countdownDuration =
                _mischiefManager.CompletionTime;
        }
        else
        {
            Debug.LogWarning(
                $"MischiefManager is not assigned. " +
                $"Using {_countdownDuration} seconds.",
                this
            );
        }

        _durationInitialized = true;
    }

    public void ResetCountdown()
    {
        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }

        _countdownImage.fillAmount = 1f;
        _countdownImage.enabled = false;
    }

    private IEnumerator CountdownCoroutine()
    {
        float elapsedTime = 0f;
        _countdownImage.fillAmount = 1f;

        while (elapsedTime < _countdownDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress =
                Mathf.Clamp01(elapsedTime / _countdownDuration);

            // Goes from completely full to completely empty.
            _countdownImage.fillAmount = 1f - progress;

            yield return null;
        }

        _countdownImage.fillAmount = 0f;
        _countdownCoroutine = null;

        _onCountdownFinished.Invoke();
    }
}
