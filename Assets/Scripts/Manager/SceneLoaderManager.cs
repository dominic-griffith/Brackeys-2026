using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{

    public static SceneLoaderManager Instance;


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

    public static SceneLoaderManager GetInstance()
    {
        return Instance;
    }


    //Scene Loading
    public void LoadSceneByNumber(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void LoadSceneByName(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(GetCurrentScene());
    }


    //Helper Methods
    public int GetCurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }


    //Exit
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}