using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGraffiti : MonoBehaviour
{
    [SerializeField] Sprite playerGraffiti;
    [SerializeField] int playerId;

    [Header("Quick Time Event")]
    [SerializeField] private GraffitiQTE qte;
    [SerializeField] private QTEInputMode qteInputMode = QTEInputMode.Keyboard;

    private CanvasGraffitiSlot currentCanvas;
    private CanvasGraffitiSlot canvasWithGraffiti;
    private bool isDoingQTE;
    private bool hasCompletedQTE = false;

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
        if (hasCompletedQTE) return;
        if (currentCanvas == null) return;
        if (isDoingQTE) return;

        isDoingQTE = true;

        qte.StartQTE(
            qteInputMode,
            OnQTESuccess,
            OnQTEFail);
    }

    public void OnQTEKeyboard(InputValue value)
    {
        if (qte != null)
        {
            qte.OnQTEKeyboard(value);
        }
    }

    public void OnQTEController(InputValue value)
    {
        if (qte != null)
        {
            qte.OnQTEController(value);
        }
    }

    private void OnQTESuccess()
    {
        currentCanvas.SetPlayerGraffiti(playerGraffiti, playerId);
        canvasWithGraffiti = currentCanvas;

        hasCompletedQTE = true;
        isDoingQTE = false;
    }

    private void OnQTEFail()
    {
        Debug.Log("Graffiti QTE failed");
        isDoingQTE = false;
    }
}
