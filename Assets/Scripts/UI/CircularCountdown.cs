using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class CircularCountdown : MonoBehaviour
{
    [Header("Countdown")]
    [SerializeField] private Image _countdownImage;
    [SerializeField] private float _countdownDuration = 5f;

    [Header("Events")]
    [SerializeField] private UnityEvent _onCountdownFinished;

    private Coroutine _countdownCoroutine;

    private void Awake()
    {
        // The circle begins completely full.
        _countdownImage.fillAmount = 1f;
        _countdownImage.enabled = false;
    }

    public void StartCountdown()
    {
        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
        }
        _countdownImage.fillAmount = 1f;
        _countdownImage.enabled = true;

        _countdownCoroutine = StartCoroutine(CountdownCoroutine());
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
