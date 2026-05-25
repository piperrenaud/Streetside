using UnityEngine;
using UnityEngine.Events;

public class ButtonGroup : MonoBehaviour
{
    [Header("Buttons in group")]
    public PressurePlate[] requiredButtons;

    [Header("Event when pressed")]
    public UnityEvent onAllButtonsPressed;
    public UnityEvent onButtonsReleased;

    private bool eventActive;

    public void CheckButtons()
    {
        foreach (PressurePlate button in requiredButtons)
        {
            if (!button.IsPressed)
            {
                if (eventActive)
                {
                    eventActive = false;
                    onButtonsReleased.Invoke();
                }

                return;
            }
        }

        if (!eventActive)
        {
            eventActive = true;
            onAllButtonsPressed.Invoke();
        }
    }
}
