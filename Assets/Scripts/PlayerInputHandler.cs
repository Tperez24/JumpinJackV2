using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerController _controller;
    private Player player;
    private InputAction movePointer;

    public void Initialize()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponentInParent<Player>();
      
        movePointer = _controller.Gamepad.Joystick;
        movePointer.performed += OnMovePointer;
    }

    public void SetController(PlayerController controller) => _controller = controller;
    
    public void OnMovePointer(CallbackContext context)
    {
       
        if (player != null && player.playerIndex == context.control.device.deviceId)
        {
            Debug.Log("player index " + player.playerIndex + " controller id " + context.control.device.deviceId);
            player.MovePointer(context);
        }
    }
}
