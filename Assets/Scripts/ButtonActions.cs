using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneController.Instance.OpenMainMenu();
    }

    public void GoToLevelSelect()
    {
        SceneController.Instance.OpenLevelSelect();
    }

    public void GoToLevelComplete()
    {
        SceneController.Instance.OpenLevelComplete();
    }

    public void LoadLevel(string levelSceneName)
    {
        SceneController.Instance.OpenLevel(levelSceneName);
    }

    public void RestartLevel()
    {
        SceneController.Instance.RestartCurrentScene();
    }

    public void RetryPreviousLevel()
    {
        SceneController.Instance.RetryPreviousLevel();
    }

    public void QuitGame()
    {
        SceneController.Instance.QuitGame();
    }
}
