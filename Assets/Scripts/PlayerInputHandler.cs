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
    private InputAction movePointer,fireButton;

    public void Initialize()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponentInParent<Player>();

        movePointer = _controller.Gamepad.Joystick;
        fireButton = _controller.Gamepad.FireButton;
        
        movePointer.performed += OnMovePointer;
        fireButton.performed += Fire;
    }

    private void Fire(CallbackContext context)
    {
        if (IsActualDevice(context)) return;
        player.ApplyForce(context);
    }

    public void SetController(PlayerController controller) => _controller = controller;

    private void OnMovePointer(CallbackContext context)
    {
        if (IsActualDevice(context)) return;
        
        Debug.Log("player index " + player.playerIndex + " controller id " + context.control.device.deviceId);
        player.MovePointer(context.ReadValue<Vector2>());
    }

    private bool IsActualDevice(CallbackContext context) =>
        player == null || player.playerIndex != context.control.device.deviceId;
}
