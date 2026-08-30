using UnityEngine;

public class TimePause : MonoBehaviour
{
    private bool _isPaused;

    public void Pause()
    {
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void TogglePause()
    {
        if (_isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void OnDestroy()
    {
        // Prevent the next scene from remaining paused.
        Time.timeScale = 1f;
    }
}
