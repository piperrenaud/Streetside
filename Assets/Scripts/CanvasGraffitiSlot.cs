using UnityEngine;
using UnityEngine.UI;

public class CanvasGraffitiSlot : MonoBehaviour
{
    [SerializeField] private Image displayImage;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PauseScene pauseScene;

    public int PlayerIdOnCanvas { get; private set; } = -1;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pauseScene = FindFirstObjectByType<PauseScene>();
    }

    public void SetPlayerGraffiti(Sprite playerGraffiti, int playerId)
    {
        displayImage.sprite = playerGraffiti;
        displayImage.enabled = true;

        PlayerIdOnCanvas = playerId;

        if (audioSource != null )
        {
            audioSource.Play();
        }

        if (pauseScene != null)
        {
            pauseScene.CheckLevelComplete();
        }
        else
        {
            Debug.LogError("PauseScene was not found in the scene.");
        }
    }

    public void ClearPlayerImage()
    {
        displayImage.sprite = null;
        displayImage.enabled = false;

        PlayerIdOnCanvas = -1;

        if (pauseScene != null)
        {
            pauseScene.CheckLevelComplete();
        }
        else
        {
            Debug.LogError("PauseScene was not found in the scene.");
        }
    }
}
