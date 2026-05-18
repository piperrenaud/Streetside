using UnityEngine;
using UnityEngine.InputSystem;

public class PauseScene : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseCanvas;

    [Header("Input")]
    [SerializeField] private InputActionReference pauseAction;

    [Header("Game Stats")]
    [SerializeField] private CanvasGraffitiSlot canvasOne;
    [SerializeField] private CanvasGraffitiSlot canvasTwo;

    private bool isPaused = false;
    private bool levelCompleteLoaded = false;

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.Enable();
            pauseAction.action.performed += OnPausePressed;
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPausePressed;
            pauseAction.action.Disable();
        } 
    }

    private void Update()
    {
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    private void Start()
    {
        ResumeGame();
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void CheckLevelComplete()
    {
        if (levelCompleteLoaded) return;

        if (canvasOne == null || canvasTwo == null)
        {
            Debug.LogError("Canvas One or Canvas Two is not assigned on PauseScene.");
            return;
        }

        bool player1HasGraffiti = canvasOne.PlayerIdOnCanvas == 1 || canvasTwo.PlayerIdOnCanvas == 1;
        bool player2HasGraffiti = canvasOne.PlayerIdOnCanvas == 2 || canvasTwo.PlayerIdOnCanvas == 2;
        bool bothCanvasesHaveGraffiti = canvasOne.PlayerIdOnCanvas != -1 && canvasTwo.PlayerIdOnCanvas != -1;

        if (player1HasGraffiti && player2HasGraffiti && bothCanvasesHaveGraffiti)
        {
            levelCompleteLoaded = true;

            if (SceneController.Instance != null)
            {
                SceneController.Instance.OpenLevelComplete();
            }
            else
            {
                Debug.LogError("SceneController.Instance is null.");
            }
        }
    }
}
