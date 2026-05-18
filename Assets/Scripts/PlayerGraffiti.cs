using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGraffiti : MonoBehaviour
{
    [SerializeField] Sprite playerGraffiti;
    [SerializeField] int playerId;

    private CanvasGraffitiSlot currentCanvas;
    private CanvasGraffitiSlot canvasWithGraffiti;

    private void OnTriggerEnter(Collider other)
    {
        CanvasGraffitiSlot slot = other.GetComponent<CanvasGraffitiSlot>();

        if (slot != null)
        {
            currentCanvas = slot;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CanvasGraffitiSlot slot = other.GetComponent<CanvasGraffitiSlot>();

        if (slot != null && slot == currentCanvas)
        {
            currentCanvas = null;
        }
    }

    public void OnSprayGraffiti(InputValue value)
    {
        if (!value.isPressed) return;

        if (currentCanvas == null) return;

        if (canvasWithGraffiti != null && canvasWithGraffiti != currentCanvas)
        {
            canvasWithGraffiti.ClearPlayerImage();
        }

        currentCanvas.SetPlayerGraffiti(playerGraffiti, playerId);
        canvasWithGraffiti = currentCanvas;
    }
}
