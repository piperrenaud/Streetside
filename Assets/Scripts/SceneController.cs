using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [Header("Menu Scenes")]
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string levelSelectScene = "LevelSelect";
    [SerializeField] private string levelCompleteScene = "LevelComplete";

    private string previousLevelScene;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void OpenLevelSelect()
    {
        SceneManager.LoadScene(levelSelectScene);
    }

    public void OpenLevelComplete()
    {
        SceneManager.LoadScene(levelCompleteScene);
    }

    public void OpenLevel(string levelSceneName)
    {
        previousLevelScene = levelSceneName;
        SceneManager.LoadScene(levelSceneName);
    }

    public void RestartCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void RetryPreviousLevel()
    {
        if (!string.IsNullOrEmpty(previousLevelScene))
        {
            SceneManager.LoadScene(previousLevelScene);
        }
        else
        {
            Debug.LogWarning("No Previous level stored");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
