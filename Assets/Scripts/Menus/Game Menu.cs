using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    [SerializeField] private string settingsSceneName;

    public void Play()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void Settings()
    {
        SceneManager.LoadScene(settingsSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}