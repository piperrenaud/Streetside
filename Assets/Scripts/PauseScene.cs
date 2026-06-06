using UnityEngine;
using UnityEngine.InputSystem;

public class PauseScene : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseCanvas;

    [Header("Input")]
    [SerializeField] private InputActionReference pauseAction;

    [Header("Game Stats")]
    [SerializeField] private CanvasGraffitiSlot graffitiSlotOne;
    [SerializeField] private CanvasGraffitiSlot graffitiSlotTwo;

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
        pauseCanvas.SetActive(false);
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

        if (graffitiSlotOne == null || graffitiSlotTwo == null)
        {
            Debug.LogError("Canvas One or Canvas Two is not assigned on PauseScene.");
            return;
        }

        bool player1HasGraffiti = graffitiSlotOne.PlayerIdOnCanvas == 1 || graffitiSlotTwo.PlayerIdOnCanvas == 1;
        bool player2HasGraffiti = graffitiSlotOne.PlayerIdOnCanvas == 2 || graffitiSlotTwo.PlayerIdOnCanvas == 2;
        bool bothCanvasesHaveGraffiti = graffitiSlotOne.PlayerIdOnCanvas != -1 && graffitiSlotTwo.PlayerIdOnCanvas != -1;

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
