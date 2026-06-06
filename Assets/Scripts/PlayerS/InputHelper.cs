using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputHelper : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var playerInput = GetComponent<PlayerInput>();

        if (Keyboard.current != null)
        {
            InputUser.PerformPairingWithDevice(Keyboard.current, playerInput.user);
            
            playerInput.SwitchCurrentControlScheme(playerInput.defaultControlScheme, Keyboard.current);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
